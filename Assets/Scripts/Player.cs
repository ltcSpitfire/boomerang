using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //Player Health related
    public int maxHealth = 10;
    public int currentHealth;
    public HealthBar healthBar;
    public bool isDead = false;
    [SerializeField] Vector2 knockback = new Vector2(25f, 25f);
    /*[SerializeField]
    private Vector2 knockBackSpeed;
    public bool knockback;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    public int direction;*/

    // Config Variables
    [SerializeField] float runSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] PhysicsMaterial2D zeroFriction;
    [SerializeField] PhysicsMaterial2D maxFriction;
    [SerializeField] float slopeCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    // Private Variables
    private Vector2 colliderSize;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private bool isOnSlope;
    private Vector2 slopeNormalPerp;
    private float controlThrow;
    public GameObject[] players;


    

    [SerializeField] private bool isGrounded;    //Variable for ground check
    public Transform feetPos;   //Variable for ground check
    public float checkRadius;   //Variable for ground check

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public float jumpHorizontalMultiplier;


    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myCollider2D;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<CapsuleCollider2D>();

        colliderSize = myCollider2D.size;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) { return; } //When true player can no longer take input

        Run();
        Jump();
        SlopeCheck();
        FlipSprite();
        //Damage();
        //CheckKnockback();

        //States what bool isGrounded is
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }

    private void Run()
    {
        controlThrow = CrossPlatformInputManager.GetAxisRaw("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        if (controlThrow == 0)
        {
            myRigidBody.sharedMaterial = maxFriction;
        }
        else
        {
            myRigidBody.sharedMaterial = maxFriction;
        }

        // Checks if we are running, then changes animation state
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (isGrounded == true && CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            //Vector2 jumpVelocityToAdd = new Vector2(0f, jumpStrength);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x * jumpHorizontalMultiplier, jumpStrength);
            myAnimator.SetBool("isJumping", true);
        }

        if (CrossPlatformInputManager.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                //Vector2 jumpVelocityToAdd = new Vector2(0f, jumpStrength);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x * jumpHorizontalMultiplier, jumpStrength);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (isGrounded == true)
        {
            myAnimator.SetBool("isJumping", false);
        }
        else
        {
            myAnimator.SetBool("isJumping", true);
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (isOnSlope && controlThrow == 0.0f && isGrounded)
        {
            myRigidBody.sharedMaterial = maxFriction;
        }
        else
        {
            myRigidBody.sharedMaterial = zeroFriction;
        }
    }

    //Flips the player sprite depending on the direction of travel
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();

        players = GameObject.FindGameObjectsWithTag("Player");

        //if(players.Length > 1)
        {
            //Destroy(players[1]); 
        }


    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth >= 1)
        {
            //Apply knockback effect
            //myRigidBody.velocity = new Vector2(damageForce.x * direction, damageForce.y);
        }

        if (currentHealth == 0)
        {
            //Player is dead
            isDead = true;
            FindObjectOfType<GameManager>().GameOver();
        }
    }
    /*
    private void Damage()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            //isDead = false;
            //myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = knockback;
            TakeDamage(2);
        }
    }
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy damaged you!");
            TakeDamage(2);
            /*if (transform.position.x > collision.transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            Knockback(direction);*/
        }
    }
    /*
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        myRigidBody.velocity = new Vector2(knockBackSpeed.x * direction, knockBackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            myRigidBody.velocity = new Vector2(0.0f, myRigidBody.velocity.y);
        }
    }*/
}

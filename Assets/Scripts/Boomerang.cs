using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public BoomerangThrower bThrower;

    Rigidbody2D rb;

    public float throwForce;
    public float boomerangLifetime;
    private Transform throwPoint;
    private Transform maxRange;
    private Transform returnTarget;
    private Vector2 firstTarget;
    private Vector2 finalTarget;
    private Vector2 secondTarget;
    public bool movingToSecondTarget;
    public bool movingToFinalTarget;
    public bool hitGround;
    public bool canBeDestroyed;
    public bool hasHitEnemy;

    // Start is called before the first frame update
    void Start()
    {
        GameObject b = GameObject.FindGameObjectWithTag("Throw Point");
        bThrower = b.GetComponent<BoomerangThrower>();

        movingToSecondTarget = false;
        movingToFinalTarget = false;
        hitGround = false;
        canBeDestroyed = false;
        rb = GetComponent<Rigidbody2D>();
        
        throwPoint = GameObject.FindGameObjectWithTag("Throw Point").transform;
        maxRange = GameObject.FindGameObjectWithTag("Max Range").transform;
        returnTarget = GameObject.FindGameObjectWithTag("Return Target").transform;
        firstTarget = new Vector2(maxRange.position.x, maxRange.position.y);
        secondTarget = new Vector2(returnTarget.position.x, returnTarget.position.y);
        finalTarget = new Vector2(throwPoint.position.x, throwPoint.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHitEnemy)
        {
            if (!movingToSecondTarget && !movingToFinalTarget && !hitGround)
            {
                Debug.Log("Boomerang searching for first target");
                transform.position = Vector2.MoveTowards(transform.position, firstTarget, throwForce * Time.deltaTime);

                if (transform.position.x == firstTarget.x && transform.position.y == firstTarget.y)
                {
                    movingToSecondTarget = true;
                    canBeDestroyed = true;
                }
            }

            if (movingToSecondTarget && !hitGround)
            {
                Debug.Log("Boomerang searching for second target");
                gameObject.layer = 8;
                rb.isKinematic = true;
                transform.position = Vector2.MoveTowards(transform.position, secondTarget, throwForce * Time.deltaTime);

                if (transform.position.x == secondTarget.x && transform.position.y == secondTarget.y)
                {
                    movingToSecondTarget = false;
                    movingToFinalTarget = true;
                    canBeDestroyed = true;
                }
            }

            if (movingToFinalTarget && !hitGround)
            {
                Debug.Log("Boomerang searching for final target");
                gameObject.layer = 11;
                rb.isKinematic = false;
                transform.position = Vector2.MoveTowards(transform.position, throwPoint.position, throwForce * Time.deltaTime);
            }
        }

        if (hasHitEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, throwPoint.position, throwForce * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(1) && hitGround)
        {
            Debug.Log("Boomerang recalled");
            DestroyBoomerang();
        }
    }

    //Checks if the boomerang has hit the foreground layer then freezes the object
    private void OnCollisionEnter2D(Collision2D collision) //note to self: add check for tag later for dealing with enemies
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Hit ground");
            hitGround = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            gameObject.layer = 8; //Changes boomerang layer to "Ground" so the player can jump on it
            StartCoroutine(DestroyAfterTime(boomerangLifetime));
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy");
            hasHitEnemy = true;
            canBeDestroyed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Throw Point" && canBeDestroyed == true)
        {
            Debug.Log("Boomerang returned");
            DestroyBoomerang();
        }
    }

    private void DestroyBoomerang()
    {
        Debug.Log("Boomerang destroyed");
        Destroy(gameObject);
        bThrower.created = false;
    }

    IEnumerator DestroyAfterTime(float boomerangLifeTime)
    {
        yield return new WaitForSeconds(boomerangLifetime);
        DestroyBoomerang();
    }
}

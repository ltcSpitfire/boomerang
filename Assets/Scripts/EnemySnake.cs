using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    public GameObject player;
    private Transform playerPos;
    private Vector2 lastPos;
    public float moveThreshold = 0f;
    private float distanceToPlayer;
    public float aggroRange;
    public bool hasAggro;
    public bool triggerExitActivated;
    new SpriteRenderer renderer;

    Rigidbody2D myRigidBody;

    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 10;
    Vector2[] previousLocations = new Vector2[noMovementFrames];
    private bool isMoving;

    void Awake()
    {
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector2.zero;
        }
    }

    // Use this for initialization
    void Start()
    {
        hasAggro = false;
        triggerExitActivated = false;
        myRigidBody = GetComponent<Rigidbody2D>();
        playerPos = player.GetComponent<Transform>();
        //spawnPos = GetComponent<Transform>().position;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = transform.position;

        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                isMoving = true;
                break;
            }
            else
            {
                isMoving = false;
            }
        }



        //Debug.Log(transform.position);
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < aggroRange)
        {
            //Debug.Log("Enemy has aggro");
            hasAggro = true;
        }
        else
        {
            hasAggro = false;
        }

        if (hasAggro)
        {
            //Debug.Log("Chasing player");
            ChasePlayer();
        }
        else
        {
            //Debug.Log("Patrolling");
            Patrol();
        }

        if (!isMoving && !hasAggro) //Fail-safe to stop enemy being stuck after losing aggro
        {
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
        }

    }

    bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            triggerExitActivated = true;

            if (triggerExitActivated)
            {
                Debug.Log("Trigger exited ground");
                transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
                Debug.Log("Trigger set to false");
                triggerExitActivated = false;
            }
        }
    }

    private void Patrol()
    {
        renderer.color = new Color(1, 1, 1, 1); //normal colour when not chasing the player

        if (IsFacingLeft())
        {
            //Debug.Log("Enemy is moving left");
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            //Debug.Log("Enemy is moving right");
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void ChasePlayer()
    {
        renderer.color = new Color(1, 0, 0, 1); //red colour when chasing the player

        if (transform.position.x < player.transform.position.x)
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0); //enemy is to the left side of player, move enemy right
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0); //enemy is to the right side of player, move enemy left
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    public GameObject player;
    private Transform playerPos;
    private Vector2 spawnPos;
    private float distanceToPlayer;
    public float aggroRange;
    public bool hasAggro;
    public bool triggerExitActivated;
    new SpriteRenderer renderer;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        hasAggro = false;
        triggerExitActivated = false;
        myRigidBody = GetComponent<Rigidbody2D>();
        playerPos = player.GetComponent<Transform>();
        spawnPos = GetComponent<Transform>().position;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(myRigidBody.velocity);
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < aggroRange)
        {
            Debug.Log("Enemy has aggro");
            hasAggro = true;
        }
        else
        {
            hasAggro = false;
        }

        if (hasAggro)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        if (!hasAggro && triggerExitActivated)
        {
            triggerExitActivated = false;
            Flip();
        }
    }
    public void Flip() //Fail-safe if snake gets stuck after losing aggro
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
        Debug.Log("Trigger flipped");
    }

    bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Trigger exited ground");
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
            triggerExitActivated = true;
        }
    }

    private void Patrol()
    {
        renderer.color = new Color(1, 1, 1, 1); //normal colour when not chasing the player
        //triggerExitActivated = false;
        if (IsFacingLeft())
        {
            //Debug.Log("Enemy is moving left");
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            //Debug.Log("Enemy is moving right");
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
    }

    private void ChasePlayer()
    {
        renderer.color = new Color(1, 0, 0, 1); //red colour when chasing the player
        //triggerExitActivated = false;
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

        /*if (Vector2.Distance(transform.position, playerPos.position) < distanceToPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, spawnPos, moveSpeed * Time.deltaTime);
            hasAggro = false;
        }*/
    }
}
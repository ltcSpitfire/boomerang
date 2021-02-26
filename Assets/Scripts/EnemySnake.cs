using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    public GameObject player;
    private Transform playerPos;
    private Vector2 spawnPos;
    public float distanceToPlayer;
    private bool isFollowing;
    SpriteRenderer renderer;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start()
    {
        isFollowing = false;
        myRigidBody = GetComponent<Rigidbody2D>();
        playerPos = player.GetComponent<Transform>();
        spawnPos = GetComponent<Transform>().position;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFollowing)
        {
            renderer.color = new Color(1, 1, 1, 1);
            if (IsFacingLeft())
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
        }


        if (Vector2.Distance(transform.position, playerPos.position) < distanceToPlayer)
        {
            isFollowing = true;
        }
        
        if (isFollowing)
        {
            renderer.color = new Color(1, 0, 0, 1);

            if (Vector2.Distance(transform.position, playerPos.position) < distanceToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, spawnPos, moveSpeed * Time.deltaTime);
                isFollowing = false;
            }
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
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
        }
    }
}
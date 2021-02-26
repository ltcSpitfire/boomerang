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
        if (!movingToSecondTarget && !hitGround)
        {
            transform.position = Vector2.MoveTowards(transform.position, firstTarget, throwForce * Time.deltaTime);

            if (transform.position.x == firstTarget.x && transform.position.y == firstTarget.y)
            {
                movingToSecondTarget = true;
                canBeDestroyed = true;
            }
        }

        if (movingToSecondTarget && !hitGround)
        {
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
            gameObject.layer = 11;
            rb.isKinematic = false;
            transform.position = Vector2.MoveTowards(transform.position, throwPoint.position, throwForce * Time.deltaTime);

            if (transform.position.x == throwPoint.position.x && transform.position.y == throwPoint.position.y)
            {
                movingToFinalTarget = false;
                DestroyBoomerang();
            }
        }

        if (Input.GetMouseButtonDown(1) && hitGround)
        {
            DestroyBoomerang();
        }
    }

    //Checks if the boomerang has hit the foreground layer then freezes the object
    private void OnCollisionEnter2D(Collision2D collision) //note to self: add check for tag later for dealing with enemies
    {
        if (collision.gameObject.tag == "Ground")
        {
            hitGround = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            gameObject.layer = 8; //Changes boomerang layer to "Ground" so the player can jump on it
            StartCoroutine(DestroyAfterTime(boomerangLifetime));
        }

        if (collision.gameObject.tag == "Enemy")
        {
            movingToFinalTarget = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Throw Point") && canBeDestroyed == true)
        {
            DestroyBoomerang();
        }
    }

    private void DestroyBoomerang()
    {
        Destroy(gameObject);
        bThrower.created = false;
    }

    IEnumerator DestroyAfterTime(float boomerangLifeTime)
    {
        yield return new WaitForSeconds(boomerangLifetime);
        DestroyBoomerang();
    }
}

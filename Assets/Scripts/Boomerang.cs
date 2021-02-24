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
    private Vector2 target;
    private Vector2 returnTarget;
    public bool isReturning;
    public bool hitGround;
    public bool canBeDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        GameObject b = GameObject.FindGameObjectWithTag("Throw Point");
        bThrower = b.GetComponent<BoomerangThrower>();

        isReturning = false;
        hitGround = false;
        canBeDestroyed = false;
        rb = GetComponent<Rigidbody2D>();
        
        throwPoint = GameObject.FindGameObjectWithTag("Throw Point").transform;
        maxRange = GameObject.FindGameObjectWithTag("Max Range").transform;
        target = new Vector2(maxRange.position.x, maxRange.position.y);
        returnTarget = new Vector2(throwPoint.position.x, throwPoint.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isReturning && !hitGround)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, throwForce * Time.deltaTime);

            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                isReturning = true;
                canBeDestroyed = true;
            }
        }

        if (isReturning && !hitGround)
        {
            transform.position = Vector2.MoveTowards(transform.position, throwPoint.position, throwForce * Time.deltaTime);

            if (transform.position.x == throwPoint.position.x && transform.position.y == throwPoint.position.y)
            {
                isReturning = false;
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

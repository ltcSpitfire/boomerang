using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangThrower : MonoBehaviour
{
    public GameObject boomerang;
    //public Rigidbody2D boomerangRB;
    public float throwForce;
    public Transform throwPoint; //Where the boomerang spawns
    public Transform aimPosition; //The direction we want the boomerang to go
    public Transform maxRange; //Maximum distance boomerang can travel
    //public Transform boomerangTransform;
    private Vector2 rangeLimit;
    //public bool created = false;
    public float maxDistance;
    private float distanceTravelled;
    //public bool isReturning;
    private Vector3 lastPosition;
    //public Vector2 opposite;

    private void Start()
    {
        Vector2 throwPos = transform.position;
        Vector2 aimPos = aimPosition.transform.position;
        Vector2 throwDirection = aimPos - throwPos;
        transform.right = throwDirection;

        //boomerangRB = boomerang.GetComponent<Rigidbody2D>();
        //lastPosition = boomerang.transform.position;
        //opposite = -boomerangRB.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //distanceTravelled += Vector3.Distance(boomerang.transform.position, lastPosition);
        //lastPosition = boomerang.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            //distanceTravelled = 0;
            //isReturning = false;

            //if (!created)
            //{
                //ThrowBoomerang();
                //GameObject newBoomerang = 
                Instantiate(boomerang, throwPoint.position, throwPoint.rotation);
                //boomerangTransform = GameObject.FindGameObjectWithTag("Boomerang").transform;

                /*if(Vector2.Distance(transform.position.x, boomerangTransform.position.x) < maxDistance)
                {
                    boomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
                }
                else
                {
                    boomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
                }*/



                //newBoomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
                //lastPosition = newBoomerang.transform.position;
                //distanceTravelled += Vector3.Distance(newBoomerang.transform.position, lastPosition);
                //Debug.Log(distanceTravelled);

                /*if (newBoomerang.transform.position.x > maxRange.position.x)
                {
                    isReturning = true;
                    newBoomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
                }
                else(distanceTravelled > maxDistance)
                {
                    newBoomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
                    isReturning = true;
                }*/

                /*if (newBoomerang.transform.position.x < maxRange.x)
                {
                    newBoomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
                }
                else
                {
                    newBoomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
                    isReturning = true;
                }*/


                //created = true;
            //}
        }
    }

    private void ThrowBoomerang()
    {
        //Rigidbody2D clone;
        //clone = Instantiate(boomerangRB, throwPoint.position, throwPoint.rotation);
        //clone.velocity = transform.right * throwForce;
        
        
        GameObject newBoomerang = Instantiate(boomerang, throwPoint.position, throwPoint.rotation);
        lastPosition = newBoomerang.transform.position;
        distanceTravelled += Vector3.Distance(newBoomerang.transform.position, lastPosition);

        if (distanceTravelled < maxDistance)
        {
            newBoomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
        }
        else//(distanceTravelled > maxDistance)
        {
            newBoomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
            //isReturning = true;
        }

        //if (distanceTravelled == maxDistance)
        //{
        //    isReturning = true;
        //    boomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
        //}
    }
}

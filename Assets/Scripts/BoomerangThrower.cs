using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangThrower : MonoBehaviour
{
    public GameObject boomerang;
    public float throwForce;
    public Transform throwPoint; //Where the boomerang spawns
    public Transform aimPosition; //The direction we want the boomerang to go
    public bool created = false;
    public float maxDistance;
    private float distanceTravelled = 0;
    public bool isReturning;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = boomerang.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(boomerang.transform.position, lastPosition);
        lastPosition = boomerang.transform.position;

        Vector2 throwPos = transform.position;
        Vector2 aimPos = aimPosition.transform.position;
        Vector2 throwDirection = aimPos - throwPos;
        transform.right = throwDirection;

        if (Input.GetMouseButtonDown(0))
        {
            //distanceTravelled = 0;
            isReturning = false;

            if (!created)
            {
                ThrowBoomerang();
                created = true;
            }
        }
    }

    private void ThrowBoomerang()
    {
        GameObject newBoomerang = Instantiate(boomerang, throwPoint.position, throwPoint.rotation);
        newBoomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;

        if (distanceTravelled == maxDistance)
        {
            isReturning = true;
            boomerang.GetComponent<Rigidbody2D>().velocity = -transform.right * throwForce;
        }
    }
}

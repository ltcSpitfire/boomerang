using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangThrower : MonoBehaviour
{
    public Boomerang boomerangScript;

    public GameObject boomerang;
    public float throwForce;
    public Transform throwPoint; //Where the boomerang spawns
    public Transform aimPosition; //The direction we want the boomerang to go
    public Transform maxRange; //Maximum distance boomerang can travel
    private Vector2 rangeLimit;
    public bool created = false;
    public bool readyToReturn = false;
    public float maxDistance;
    private float distanceTravelled;
    private Vector3 lastPosition;

    private void Start()
    {
        GameObject boomerang = GameObject.FindGameObjectWithTag("Boomerang");
        //boomerangScript = b.GetComponent<Boomerang>();

        Vector2 throwPos = transform.position;
        Vector2 aimPos = aimPosition.transform.position;
        Vector2 throwDirection = aimPos - throwPos;
        transform.right = throwDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!created)
            {
                Instantiate(boomerang, throwPoint.position, throwPoint.rotation);
                created = true;
            }
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            if (created)
            {
                readyToReturn = true;
                created = false;
            }
        }*/
    }
}

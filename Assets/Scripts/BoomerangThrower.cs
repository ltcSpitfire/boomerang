using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangThrower : MonoBehaviour
{
    public GameObject boomerang;
    public float throwForce;
    public Transform throwPoint; //Where the boomerang spawns
    public Transform aimPosition; //The direction we want the boomerang to go
    public Transform maxRange; //Maximum distance boomerang can travel
    public bool created = false;

    private void Start()
    {
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
    }
}

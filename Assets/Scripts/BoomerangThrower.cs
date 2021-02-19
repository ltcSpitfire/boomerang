using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangThrower : MonoBehaviour
{
    public GameObject boomerang;
    public float throwForce;
    public Transform throwPoint; //Where the boomerang spawns
    public Transform aimPosition; //The direction we want the boomerang to go

    // Update is called once per frame
    void Update()
    {
        Vector2 throwPos = transform.position;
        Vector2 aimPos = aimPosition.transform.position;
        Vector2 throwDirection = aimPos - throwPos;
        transform.right = throwDirection;

        if (Input.GetMouseButtonDown(0))
        {
            ThrowBoomerang();
        }
    }

    private void ThrowBoomerang()
    {
        GameObject newBoomerang = Instantiate(boomerang, throwPoint.position, throwPoint.rotation);
        newBoomerang.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
    }
}

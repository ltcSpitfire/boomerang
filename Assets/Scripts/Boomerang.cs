using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    Rigidbody2D rb;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks if the boomerang has hit the foreground layer then freezes the object
    private void OnCollisionEnter2D(Collision2D collision) //note to self: add check for tag later for dealing with enemies
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        gameObject.layer = 8; //Changes boomerang layer to "Ground" so the player can jump on it
    }
}

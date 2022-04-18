using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sand : MonoBehaviour
{
    
    private Rigidbody2D rb;
    

    // Update is called once per frame
    void Update()
    {
        if (rb != null){
            rb.velocity = new Vector2(.9f * rb.velocity.x, rb.velocity.y);
            if (Math.Abs(rb.velocity.x) < .03){
                rb.velocity = new Vector2(0,0);
                rb.angularVelocity = 0;
            }
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other){
        rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null){
            rb.velocity = new Vector2(rb.velocity.x / Math.Abs(rb.velocity.x), 0);
        }
    }
    public void OnCollisionExit2D(Collision2D other){
        rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null){
            rb = null;
        }
    }
}

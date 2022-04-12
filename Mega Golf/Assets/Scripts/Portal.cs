using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour
{
    
    public GameObject orange;
    public GameObject blue;
    int delayCount;
    bool delay;
    double rotation;
    
    // Start is called before the first frame update
    void Start()
    {
        orange = this.transform.GetChild(0).gameObject;
        blue = this.transform.GetChild(1).gameObject;
        delay = false;
        
        rotation = -orange.transform.localEulerAngles.z + blue.transform.localEulerAngles.z - 180;
        rotation = rotation*Math.PI/180;

    }

    // Update is called once per frame
    void Update()
    {
        if (delay) delayCount++;
        if (delayCount > 10){
            delay=false;
            delayCount=0;
            
        } 
    }
    
    public void OnTriggerEnter2D(Collider2D other){
        
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        Vector2 v = rb.velocity;
        float cos = (float)Math.Cos(rotation);
        float sin = (float)Math.Sin(rotation);
        
        if (orange.GetComponent<Collider2D>().IsTouching(other) && !delay){
            rb.velocity = new Vector2(v.x*cos -v.y*sin, v.x*sin +v.y*cos);
            rb.position = blue.transform.position;
            delay = true;
        } 
        if (blue.GetComponent<Collider2D>().IsTouching(other) && !delay){
            rb.velocity = new Vector2(v.x*cos + v.y*sin, -v.x*sin +v.y*cos);;
            rb.position = orange.transform.position;
            delay = true;

        }

    }
    
    
    
}

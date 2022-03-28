using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallControl : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("a")){
            launch(10,5*Math.PI/6);
        }
        if (Input.GetKeyDown("s")){
            launch(10,2*Math.PI/3);
        }
        if (Input.GetKeyDown("d")){
            launch(10,Math.PI/3);
        }
        if (Input.GetKeyDown("f")){
            launch(10,Math.PI/6);
        }
        if (Input.GetKeyDown("space")){
            launch(10,Math.PI/2);
        }
        if (Input.GetKeyDown("g")){
            launch(20,Math.PI/4);
        }
    }
    
    //give the ball a speed and angle
    void launch(double v, double theta){
        float v_x = (float)(v * Math.Cos(theta));
        float v_y = (float)(v * Math.Sin(theta));
        rb.velocity = new Vector2(v_x,v_y);
         

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool collided;
    private bool stationary = false;
    public GameHandler gameHandlerObj;
    public  float shootPower = 10f;
    Vector2 startPos, endPos, direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        if (GameObject.FindWithTag("GameHandler") != null){
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
         }
        
    }
     public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Goal")){
            EndGame();
        }
        else{
            collided =true;
        }

        
    }
    
    public void OnCollisionExit2D(Collision2D other){
        collided =false;
    }
    
     void EndGame(){
          SceneManager.LoadScene("GameOver");

     }

    // Update is called once per frame
    void Update()
    {
        check_stationary();
        
    }
    
    //give the ball a speed and angle
    void launch(double v, double theta){
        float v_x = (float)(v * Math.Cos(theta));
        float v_y = (float)(v * Math.Sin(theta));
        rb.velocity = new Vector2(v_x,v_y);
        gameHandlerObj.AddStroke(1);
        
    }
    //check that the ball has stopped moving
    void check_stationary(){
        //reduce x velocity faster
        if (rb.velocity.y == 0 && Math.Abs(rb.velocity.x) < 1){
            rb.velocity = new Vector2(rb.velocity.x * 0.97f,0);
            if (Math.Abs(rb.velocity.x) < .03){
                rb.velocity = new Vector2(0,0);
                rb.angularVelocity = 0;
                stationary = true;
            }
        }
        
    }

     
     void OnMouseDown (){
        if(stationary && collided){
            if (Input.GetMouseButtonDown (0)) {
                startPos = Input.mousePosition;
            }
        } 

     }
     void OnMouseUp (){
         if(stationary && collided){
            if (Input.GetMouseButtonUp (0)) {
                 endPos = Input.mousePosition;
                direction = startPos - endPos;
                rb.isKinematic = false;
                rb.AddForce (direction * shootPower);
                gameHandlerObj.AddStroke(1);
             }
         }
        
     }
}
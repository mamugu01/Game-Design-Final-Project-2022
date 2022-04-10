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

    public Vector2 minPower;
    public Vector2 maxPower;

    Vector2 force;
    Vector2 startPos, endPos, direction;
    Vector2 ballPos;
    private string ballType = "standard";
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        if (GameObject.FindWithTag("GameHandler") != null){
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
         }
         
         ballType = this.tag;
         
        
    }
    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Goal")){
            EndGame();
        }
        else{
            collided =true;
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "water"){
            //reset ball and add a stroke penalty
            rb.position = ballPos;
            rb.velocity = new Vector2(0,0);
            rb.angularVelocity = 0;
            gameHandlerObj.AddStroke(1);
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
        gameHandlerObj.UpdateReady(stationary);
        if(Input.GetKeyDown("space")){
            Trigger();
        }
        
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
        
        if (rb.velocity != new Vector2(0,0)){
            stationary = false;
        }
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
                ballPos = rb.position;
            }
        } 

     }
     void OnMouseUp (){
         if(stationary && collided){
            if (Input.GetMouseButtonUp (0)) {
                 endPos = Input.mousePosition;
                direction = startPos - endPos;
                rb.isKinematic = false;
                force = new Vector2(Mathf.Clamp(direction.x, minPower.x, maxPower.x), Mathf.Clamp(direction.y, minPower.y, maxPower.y));
                rb.AddForce (force * shootPower);
                gameHandlerObj.AddStroke(1);
             }
         }
        
     }
     
     private void Trigger(){
         if (ballType == "grenade" && !stationary){
             Explode();
         }
     }
     private void Explode(){
         Debug.Log("BOOM!");
         // GameObject[] results = new GameObject[] {};
         Collider2D[] proximityCheck = Physics2D.OverlapCircleAll(rb.position, 1f);
         foreach(Collider2D box in proximityCheck){
             if (box.tag == "explodable"){
                 Destroy(box.gameObject);
             }
         }
         
     }
     
}

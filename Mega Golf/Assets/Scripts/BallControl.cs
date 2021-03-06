using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class BallControl : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool collided;
    private bool stationary = false;
    

    private bool sticky = false;
    private bool disable = false;
    public GameHandler gameHandlerObj;
    public  float shootPower = 5f;
    private float currGrav = 1;
    public GameObject CompleteLevelUI;
    
    
    // public Slider spinSlider;

    public Vector2 windPosition;
    public float maxSpeed;

    Vector2 force;
    Vector3 startPos, endPos, direction; 
    Vector2 ballPos;
    private string ballType = "standard";

    public TrajectoryLine tl;

    Camera cam;
    private bool powerUsed = false;

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private GameObject collision_part;

    [SerializeField]
    private GameObject splash;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        tl.EndLine();
        
        if (GameObject.FindWithTag("GameHandler") != null){
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
         }
         
         ballType = this.tag;
         
         //just makes it easier to edit the min max power in here
        
    }

    public void OnCollisionEnter2D(Collision2D other){
        Instantiate(collision_part, transform.position, transform.rotation);
        if(other.gameObject.CompareTag("Goal")){
            Debug.Log("done");
            CompleteLevelUI.SetActive(true);
            // gameHandlerObj.UpdateScorecard();

        }
        else{
            collided =true;
            if(ballType == "sticky" && !other.gameObject.CompareTag("portal")){
                rb.isKinematic = false;
                rb.velocity = new Vector2(0,0);
                rb.angularVelocity = 0;
                rb.gravityScale = 0;
            
            }
        }
    }

    
    public void OnTriggerEnter2D(Collider2D other){
        if((other.tag == "water" | other.tag == "enemy") || other.tag == "taxi"){
            //reset ball and add a stroke penalty
            
            if(other.tag == "water"){
                Instantiate(splash, transform.position, transform.rotation);
            }
            rb.position = ballPos;
            rb.velocity = new Vector2(0,0);
            rb.angularVelocity = 0;
            gameHandlerObj.AddStroke(1);
            if (ballType == "gravity" && powerUsed) {
                currGrav *= -1;
                rb.gravityScale = currGrav;
                powerUsed = false;
            }
            if (ballType == "sticky"){
                rb.isKinematic = false;
                rb.velocity = new Vector2(0,0);
                rb.angularVelocity = 0;
                rb.gravityScale = 0;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Wind"){
                    float windAngle = other.transform.rotation.eulerAngles.z*(float)(3.1416/180);
                    windPosition = new Vector2(-1*(float)Math.Sin(windAngle),(float)Math.Cos(windAngle));
                    rb.AddForce(windPosition*5000*Time.deltaTime);
                }
    }
    
    public void OnCollisionExit2D(Collision2D other){
        collided = false;
        
    }
    
     void EndGame(){
          SceneManager.LoadScene("GameOver");

     }

    // Update is called once per frame
    void Update()
    {
        check_stationary();
        check_reset();
        gameHandlerObj.UpdateReady(stationary);
        if(Input.GetKeyDown("space")){
            Trigger();
        }
        // if (stationary && collided) powerUsed = false;
        //removed && collided here, not sure if collided was necessary
        if(stationary){
            if (Input.GetMouseButtonDown (0) && !disable) {
                startPos = cam.ScreenToWorldPoint(Input.mousePosition);
                startPos.z = 15;

                ballPos = rb.position;
                disable = EventSystem.current.IsPointerOverGameObject();
            }

            if(Input.GetMouseButton(0)&& !disable){
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                currentPoint.z = 15;
                tl.RenderLine(currentPoint, startPos);
            }
            if (Input.GetMouseButtonUp (0)) {
                if (!disable){
                    endPos = cam.ScreenToWorldPoint(Input.mousePosition);
                    endPos.z = 15;
                    rb.gravityScale = currGrav;
                    
                    direction = (startPos - endPos);
                    sticky = false;
                    rb.isKinematic = false;
                    force = direction * shootPower;
                    if(force.magnitude > maxSpeed){
                         force = (Vector2)Vector3.ClampMagnitude((Vector3)force, maxSpeed);
                    }

                    rb.AddForce (force, ForceMode2D.Impulse);
                     rb.angularVelocity = -1000 * Math.Sign(direction.x) * currGrav * gameHandlerObj.getSpin();
                    Debug.Log(gameHandlerObj.getSpin());
                    tl.EndLine();
                    if (force != new Vector2(0,0)) {
                        gameHandlerObj.AddStroke(1);
                        gameHandlerObj.resetSpin();
                        powerUsed = false;
                        PlayAudio();
                    }
                }
                else disable = false;
            
                
             }




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
        if (Math.Abs(rb.velocity.y) < 1 && Math.Abs(rb.velocity.x) < 1){
            rb.velocity = new Vector2(rb.velocity.x * 0.97f, rb.velocity.y * 0.97f);
            if (Math.Abs(rb.velocity.x) < .08 && Math.Abs(rb.velocity.y) < .03){
                rb.velocity = new Vector2(0,0);
                rb.angularVelocity = 0;
                stationary = true;
            }
        }
        
        
    }

    //check if reset key is pressed
    void check_reset(){
        if (Input.GetKeyDown(KeyCode.R))
        {
            //reset ball and add a stroke penalty
            rb.position = ballPos;
            rb.velocity = new Vector2(0,0);
            rb.angularVelocity = 0;
            gameHandlerObj.AddStroke(1);
            if (ballType == "gravity" && powerUsed) {
                currGrav *= -1;
                rb.gravityScale = currGrav;
            }
        }
    }
     
     
 
     
     private void Trigger(){
         if (ballType == "grenade" && !stationary){
             Explode();
         }
         if ((ballType == "gravity" && !powerUsed ) && !stationary){
             Warp();
         }

        if (ballType == "sticky"){
            sticky = true;
        }
        if ((ballType == "freeze" && !powerUsed )&& !stationary) Freeze();
     }
     private void Explode(){
         Debug.Log("BOOM!");
         // GameObject[] results = new GameObject[] {};
        
         Instantiate(explosion, transform.position, transform.rotation);
         GetComponents<AudioSource>()[1].Play();
         Collider2D[] proximityCheck = Physics2D.OverlapCircleAll(rb.position, 1f);
         foreach(Collider2D box in proximityCheck){
             if ((box.tag == "explodable" || box.tag == "tumbleweed")|| box.tag == "enemy"){
                 Destroy(box.gameObject);
             }
         }
         
         
     }
     
     private void Warp(){
         Debug.Log("woohoo");
         rb.gravityScale = -rb.gravityScale;
         currGrav =  rb.gravityScale;
         powerUsed = true;
         // gameHandlerObj.AddStroke(1);
     }
     
    public bool isStationary(){return stationary;}
    
    private void Freeze(){
        rb.velocity = new Vector2(0,0);
        rb.angularVelocity = 0;
        powerUsed = true;

    }
    
    private void PlayAudio(){
        GetComponents<AudioSource>()[0].Play();
               // GameObject boomFX = Instantiate(hitVFX, transform.position, Quaternion.identity);
               // StartCoroutine(DestroyVFX(boomFX));
    }
    // IEnumerator DestroyVFX(GameObject theEffect){
    //      yield return new WaitForSeconds(0.5f);
    //      Destroy(theEffect);
    //      gameObject.GetComponent<AudioSource>().Stop();
    // }
    // 
    
     
}

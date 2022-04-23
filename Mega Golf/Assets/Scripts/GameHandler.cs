using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameHandler : MonoBehaviour {
      
      public GameObject strokeText, speedText, finalScoreText;
      private int stroke_count = 0;
      private int[] scores = new int[3];
      private int currHole = 0;
      private float spin = 0;
      public Slider spinSlider;
      public GameObject[] balls;
      private int ballIndex = 0;
      private GameObject cam;

      
      void Start(){
            
            
            stroke_count = 0;
            for(int i =0; i< 3; i++){
                scores[i] = GlobalControl.Instance.scorecard[i];
            }
            currHole = GlobalControl.Instance.currHole;
            UpdateStrokes();
            
            if (SceneManager.GetActiveScene().name == "GameOver") PrintScorecard();
            
            balls = new GameObject[] {GameObject.Find("Ball_Standard"), 
                      GameObject.Find("Ball_Bouncy Variant"), GameObject.Find("Ball_Grenade"), GameObject.Find("Ball_Gravity"), 
                      GameObject.Find("Ball_Sticky"), GameObject.Find("Ball_Freeze")};
                      
            for (int i = 0; i < 6; i++) Debug.Log(balls[i].tag);
            
            cam = GameObject.FindWithTag("MainCamera");
            // SetActiveObject(2);
            // balls[0].GetComponent<Rigidbody2D>().velocity = new Vector2(3,3);
            
            
      }
      void Update(){
          SetActiveObject(ballIndex);
          if(Input.GetKeyDown("b")){
              Switch();
          }
      }

      public void AddStroke(int num){
            stroke_count += num;
            UpdateStrokes();
      }

      void UpdateStrokes(){
            Text strokeTextB = strokeText.GetComponent<Text>();
            strokeTextB.text = "Strokes: " + stroke_count;
            SaveData();
      }
      
      public void UpdateReady(bool ready){
          Text speedTextB = speedText.GetComponent<Text>();
          if(ready){
              speedTextB.text = "Ready";
              speedTextB.color = Color.green;
          }
          else {
              speedTextB.text = "Wait";
              speedTextB.color = Color.red;
          }
          
      }
      
      public void UpdateScorecard(){
          
          Debug.Log(currHole);
          Debug.Log(scores);
          scores[currHole] = stroke_count;
          currHole++;
          
          SaveData();

      }
      
      void PrintScorecard(){
          Debug.Log("In Print scorecard");

          int sum = scores[0]+scores[1]+scores[2];
          Text finalScoreTextB = finalScoreText.GetComponent<Text>();
          finalScoreTextB.text = "Hole:  1   2   3   Tot.  \n      " 
                                +scores[0]+ "   "+ scores[1] + "   "+ scores[2]
                                +"   "+ sum;
                                
        currHole = 0;
        for(int i =0; i< 3; i++){
            scores[i] = 0;
        }
        SaveData();
          
      }
      
      public void SaveData(){
          for(int i =0; i< 3; i++){
              GlobalControl.Instance.scorecard[i] = scores[i];
          }
          GlobalControl.Instance.currHole = currHole;

      }
      
      public void OnValueChanged(){
          spin = spinSlider.value;
      }
      
      public float getSpin(){
          return spin;
      }
      
      public void resetSpin(){
          spin = 0;
          spinSlider.value = 0;
      }
      
      public void SetActiveObject(int aIndex){
          ballIndex = aIndex;
          for(int i = 0; i < balls.Length; i++) if(balls[i] != null) balls[i].SetActive(i == ballIndex);
          // cam.GetComponent<Camera_Follow>().target = balls[ballIndex].transform;
      }
      
      private void Switch(){
          SetActiveObject((ballIndex + 1) % 6);
          Debug.Log(ballIndex);
          Debug.Log(cam);
          cam.GetComponent<Camera_Follow>().target = balls[ballIndex].transform;

          
      }
}
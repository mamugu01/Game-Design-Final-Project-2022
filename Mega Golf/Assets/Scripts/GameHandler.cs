using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameHandler : MonoBehaviour {
      
      public GameObject strokeText, speedText, finalScoreText, typeText;
      private int stroke_count = 0;
      private int[] scores = new int[3];
      private int currHole = 0;
      private float spin = 0;
      public Slider spinSlider;
      private GameObject[] balls;
      private int ballIndex = 0;
     GameObject cam;

      
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
                      
            for (int i = 0; i < 6; i++) if(balls[i] != null) Debug.Log(balls[i].tag);
            
            cam = GameObject.FindWithTag("MainCamera");
            UpdateTypeText();
      }
      void Update(){
          SetActiveObject(ballIndex);
          if(Input.GetKeyDown("b")){
              if (balls[ballIndex].GetComponent<BallControl>().isStationary()) Switch();
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
          Debug.Log(spin);
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
      }
      
      private void Switch(){
          Vector2 pos = balls[ballIndex].transform.position;
          int nextIndex = FindNextIndex(ballIndex);
          Debug.Log(nextIndex);
          SetActiveObject(nextIndex);
          balls[ballIndex].transform.position = pos;
          cam.GetComponent<Camera_Follow>().SetTarget(balls[ballIndex].transform); 
          UpdateTypeText();
      }
      
      private int FindNextIndex(int currIndex){
          for (int i = 1; i < 6; i++){
             if(balls[(currIndex + i) % 6] != null) return (currIndex + i) % 6;
          }
          return currIndex;
      }
      
      
      public void UpdateTypeText(){
          string type = "";
          string tag = balls[ballIndex].tag;
          
          if (tag == "standard") type = "Standard";
          if (tag == "grenade") type = "Grenade";
          if (tag == "sticky") type = "Sticky";
          if (tag == "freeze") type = "Freeze";
          if (tag == "gravity") type = "Gravity Warp";
          if (tag == "bouncy") type = "Bouncy";
         
          Text TypeTextB = typeText.GetComponent<Text>();
          TypeTextB.text = "Ball Type: " + type;
      }
}
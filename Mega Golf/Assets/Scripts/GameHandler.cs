using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameHandler : MonoBehaviour {
      
      public GameObject strokeText, speedText;
      private int stroke_count = 0;

      
      void Start(){
            stroke_count = GlobalControl.Instance.strokes;
            UpdateStrokes();
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
      
      public void SaveData(){
          GlobalControl.Instance.strokes = stroke_count;
      }
}
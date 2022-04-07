using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {
      
      public GameObject strokeText;
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
      
      public void SaveData(){
          GlobalControl.Instance.strokes = stroke_count;
      }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public GameObject finalScoreText;
    private int[] scores = new int[9];
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i< 9; i++){
            scores[i] = GlobalControl.Instance.scorecard[i];
            // Debug.Log(scores[i]);
        }
        PrintScorecard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void PrintScorecard(){
        Debug.Log("In Print scorecard");

        int sum = scores[0]+scores[1]+scores[2]+ scores[3];
        Text finalScoreTextB = finalScoreText.GetComponent<Text>();
        finalScoreTextB.text = "Hole:  1   2   3   4   Tot.  \n      " 
                              +scores[0]+ "   "+ scores[1] + "   "+ scores[2]
                              + "   "+ scores[3]+"   "+ sum;
                              
      
      for(int i =0; i< 9; i++){
          scores[i] = 0;
      }
      // SaveData();
        
    }
}

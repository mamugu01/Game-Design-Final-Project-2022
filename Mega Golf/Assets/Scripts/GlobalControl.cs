using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    
    public static GlobalControl Instance;
    public int currHole;
    public int[] scorecard = new int[4];

    void Awake (){
        if (Instance == null){
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this){
            Destroy (gameObject);
        }
      }
    
    
}

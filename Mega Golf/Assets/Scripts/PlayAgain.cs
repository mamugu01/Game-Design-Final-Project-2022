using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public void Restart()
    {
        GlobalControl.Instance.scorecard = new int[]{0,0,0,0,0,0,0,0,0};
        GlobalControl.Instance.currHole =0;
        SceneManager.LoadScene("scene1");
    }
}

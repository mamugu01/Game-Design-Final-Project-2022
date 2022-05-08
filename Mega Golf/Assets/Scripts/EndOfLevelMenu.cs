using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelMenu : MonoBehaviour
{

public void LoadNextLevel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
}

public void RestartLevel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

public void LoadMainMenu()
{
    SceneManager.LoadScene(0);
}

}

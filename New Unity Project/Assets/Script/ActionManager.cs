using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionManager 
{
    public void Restart()
    {
        GameManager.Instance.playTime = 0f;
        Time.timeScale = 0f;
        SceneManager.LoadScene("Game1");
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void GoMenu()
    {
        GameManager.Instance.playTime = 0f;
        Time.timeScale = 0f;
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }

}

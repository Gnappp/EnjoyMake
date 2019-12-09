﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseui;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseui.active)
            {
                Time.timeScale = 0f;
                pauseui.SetActive(true);
            }
            else if(pauseui.active)
            {
                Resume();
            }
        }

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Resume()
    {
        pauseui.SetActive(false);
        Time.timeScale = 1f;
    }

}
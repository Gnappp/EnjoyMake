using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI_ClickEvent : MonoBehaviour
{
    public GameObject pauseui;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        string sceneName;
        sceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(sceneName);
        Debug.Log("??");
    }

    public void Resume()
    {
        pauseui.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("??");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameoverUI : MonoBehaviour
{
    public GameObject gameoverUI;

    private string gameoverTime;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Get_gameover() && !gameoverUI.active)
        {
            Time.timeScale = 0f;
            gameoverUI.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && gameoverUI.active && GameManager.Instance.Get_gameover())
        {
            Restart();
        }
        else if(Input.GetKeyUp(KeyCode.Escape) && gameoverUI.active && GameManager.Instance.Get_gameover())
        {
            GoMenu();
        }
        
    }

    

    public void GoMenu()
    {
        SceneManager.LoadSceneAsync("Start");
        GameManager.Instance.Set_gameover(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        GameManager.Instance.Set_gameover(false);
        SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1f;
    }
    
}

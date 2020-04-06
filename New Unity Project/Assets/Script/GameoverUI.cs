using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameoverUI : MonoBehaviour
{
    public GameObject gameoverUI;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Get_gameover() && !gameoverUI.active)
        {
            Time.timeScale = 0f;
            gameoverUI.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && gameoverUI.active)
        {
            Restart();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && gameoverUI.active)
        {
            GoMenu();
        }
        
    }
    public void Restart()
    {
        
        GameManager.Instance.Set_gameover(false);
        gameoverUI.gameObject.SetActive(false);
        GameManager.Instance.actionManager.Restart();
    }

    public void GoMenu()
    {
        GameManager.Instance.Set_gameover(false);
        gameoverUI.gameObject.SetActive(false);
        GameManager.Instance.actionManager.GoMenu();
    }




}

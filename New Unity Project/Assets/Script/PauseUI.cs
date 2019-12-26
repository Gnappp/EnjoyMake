using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{

    public GameObject pauseui;
    public Sprite texture;

    private Sprite normalSprite;
    private string sceneName;
    private string menuScene = "Strat";
    private int focusBtn=0;
    private List<Button> button=new List<Button>();
    private int panelNo;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        for(int i=0;i<pauseui.transform.childCount;i++)
        {
            if (pauseui.transform.GetChild(i).name == "BtnPanel")
            {
                panelNo = i;
                break;
            }
        }
        
        for (int i=0;i<pauseui.transform.GetChild(panelNo).childCount;i++)
        {
            button.Add(pauseui.transform.GetChild(panelNo).GetChild(i).GetComponent<Button>());
        }

        normalSprite = button[focusBtn].image.sprite;
        button[focusBtn].image.sprite = texture;
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
        if(pauseui.active)
        {
            ButtonFocus();
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

    public void GoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Start");
    }

    void ButtonFocus()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (focusBtn == button.Count - 1)
            {
                button[focusBtn].image.sprite = normalSprite;
                focusBtn = 0;
                button[focusBtn].image.sprite = texture;
            }
            else
            {
                button[focusBtn].image.sprite = normalSprite;
                focusBtn++;
                button[focusBtn].image.sprite = texture;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (focusBtn == 0)
            {
                button[focusBtn].image.sprite = normalSprite;
                focusBtn = button.Count - 1;
                button[focusBtn].image.sprite = texture;
            }
            else
            {
                button[focusBtn].image.sprite = normalSprite;
                focusBtn--;
                button[focusBtn].image.sprite = texture;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (button[focusBtn].name)
            {
                case "Restart":
                    Restart();
                    break;
                case "Menu":
                    GoMenu();
                    break;
                case "Resume":
                    Resume();
                    break;
            }
        }
    }

}

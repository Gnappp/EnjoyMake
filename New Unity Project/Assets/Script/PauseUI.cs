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
    private GameObject curtain;
    private Image childImg;
    private bool loadScene = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = "Game1";
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
        curtain = GameObject.Find("Curtain(Clone)");
        childImg = curtain.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curtain == null)
        {
            curtain = GameObject.Find("Curtain(Clone)");
            childImg = curtain.transform.GetChild(0).GetComponent<Image>();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale!=0f)
        {
            if (!pauseui.active)
            {
                Time.timeScale = 0f;
                pauseui.SetActive(true);
            }
        }
        else if (pauseui.active && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        else if(pauseui.active)
        {
            ButtonFocus();
        }

        if (childImg.canvasRenderer.GetAlpha() == 254f && loadScene)
        {
            SceneManager.LoadScene(sceneName);
            GameManager.Instance.canvasAlpha = childImg.canvasRenderer.GetAlpha();
            GameManager.Instance.playTime = 0f;
            loadScene = false;
        }

    }

    public void Restart()
    {
        pauseui.SetActive(false);
        sceneName = "Game1";
        GameManager.Instance.playTime = 0f;
        Time.timeScale = 0f;
        childImg.canvasRenderer.SetAlpha(1f);
        childImg.CrossFadeAlpha(254f, 2f, true);
        loadScene = true;
        Debug.Log(GameObject.Find("Curtain(Clone)"));
    }

    public void Resume()
    {
        pauseui.SetActive(false);
        button[focusBtn].image.sprite = normalSprite;
        focusBtn = 0;
        button[focusBtn].image.sprite = texture;
        Time.timeScale = 1f;
    }

    public void GoMenu()
    {
        pauseui.SetActive(false);
        sceneName = "Start";
        GameManager.Instance.playTime = 0f;
        Time.timeScale = 0f;
        childImg.canvasRenderer.SetAlpha(1f);
        childImg.CrossFadeAlpha(254f, 2f, true);
        loadScene = true;
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

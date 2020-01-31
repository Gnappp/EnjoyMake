using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameoverUI : MonoBehaviour
{
    public GameObject gameoverUI;

    private string gameoverTime;
    private string sceneName = "";
    private GameObject curtain;
    private Image childImg;
    private bool loadScene = false;

    // Start is called before the first frame update
    void Start()
    {
        curtain = GameObject.Find("Curtain(Clone)");
        childImg = curtain.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if( curtain == null)
        {
            curtain = GameObject.Find("Curtain(Clone)");
            childImg = curtain.transform.GetChild(0).GetComponent<Image>();
        }
        if (GameManager.Instance.Get_gameover() && !gameoverUI.active)
        {
            Time.timeScale = 0f;
            gameoverUI.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && gameoverUI.active && GameManager.Instance.Get_gameover())
        {
            sceneName = "Game1";
            FadeScene();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && gameoverUI.active && GameManager.Instance.Get_gameover())
        {
            sceneName = "Menu";
            FadeScene();
        }
        if (childImg.canvasRenderer.GetAlpha() == 254f && loadScene)
        {
            SceneManager.LoadScene(sceneName);
            GameManager.Instance.Set_gameover(false);
            GameManager.Instance.canvasAlpha = childImg.canvasRenderer.GetAlpha();
            GameManager.Instance.playTime = 0f;
            loadScene = false;
        }
    }



    public void FadeScene()
    {
        gameoverUI.SetActive(false);
        GameManager.Instance.playTime = 0f;
        Time.timeScale = 0f;
        childImg.canvasRenderer.SetAlpha(1f);
        childImg.CrossFadeAlpha(254f, 2f, true);
        loadScene = true;
    }


}

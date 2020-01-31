using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TestServer : MonoBehaviour
{
    public Canvas rankCanvas;
    public Canvas btnCanvas;
    public Sprite texture;
    public Scene gameScene;
    private RankResult data;
    private List<Button> button = new List<Button>();
    private Text btnCanvas_Text;
    private Sprite normalSprite;
    private int focusBtn = 0;
    private GameObject curtain;
    private Image childImg;

    [Serializable]
    public class RankResult
    {
        public List<Rank> rank;
    }

    [Serializable]
    public class Rank
    {
        public string name;
        public float time;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PostNetworkingWithWebRequest());
        int panelNo=-1;
        for (int i = 0; i < btnCanvas.gameObject.transform.childCount; i++)
        {
            if (btnCanvas.gameObject.transform.GetChild(i).name == "btnPanel")
                panelNo = i;
            if (btnCanvas.gameObject.transform.GetChild(i).name == "btnCanvas_Text")
                btnCanvas_Text = btnCanvas.gameObject.transform.GetChild(i).GetComponent<Text>();
        }
        if (panelNo != -1)
        {
            int count = btnCanvas.gameObject.transform.GetChild(panelNo).transform.childCount;
            for (int i = 0; i < count; i++)
                button.Add(btnCanvas.gameObject.transform.GetChild(panelNo).GetChild(i).GetComponent<Button>());

            normalSprite = button[focusBtn].image.sprite;
            button[focusBtn].image.sprite = texture;
        }
        else
            Debug.Log("Not btnCanvas panel");

        curtain = GameObject.Find("Curtain(Clone)");
        childImg = curtain.transform.GetChild(0).GetComponent<Image>();
    }   

    // Update is called once per frame
    void Update()
    {
        ButtonFocus();
        InputRankText();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (rankCanvas.gameObject.active)
                ExitRanking();
        }
        if(childImg.canvasRenderer.GetAlpha()==254f)
        {
            SceneManager.LoadScene("Game1");
            GameManager.Instance.canvasAlpha = childImg.canvasRenderer.GetAlpha();
        }
    }

    void FixedUpdate()
    {
        
    }

   
    public void ExitRanking()
    {
        rankCanvas.gameObject.SetActive(false);
        btnCanvas.gameObject.SetActive(true);
    }

    void InputRankText()
    {
        if (rankCanvas.gameObject.active && data != null)
        {
            int panelNo = -1;
            for (int i = 0; i < rankCanvas.gameObject.transform.childCount; i++)
            {
                if (rankCanvas.gameObject.transform.GetChild(i).name == "RankTextPanel")
                    panelNo = i;
            }
            for (int i = 0; i < data.rank.Count; i++)
            {
                if (i == 0)
                    rankCanvas.gameObject.transform.GetChild(0).GetComponent<Text>().text = (i + 1) + ". " + data.rank[i].name.ToString() + "   " + data.rank[i].time.ToString();
                else if (i == 1)
                    rankCanvas.gameObject.transform.GetChild(1).GetComponent<Text>().text = (i + 1) + ". " + data.rank[i].name.ToString() + "   " + data.rank[i].time.ToString();
                else if (i >= 2 && i < 6)
                    rankCanvas.gameObject.transform.GetChild(panelNo).GetChild(0).GetComponent<Text>().text += (i + 1) + ". " + data.rank[i].name.ToString() + "   " + data.rank[i].time.ToString() + "\n\n";
                else if (i >= 6 && i < 10)
                    rankCanvas.gameObject.transform.GetChild(panelNo).GetChild(1).GetComponent<Text>().text += (i + 1) + ". " + data.rank[i].name.ToString() + "   " + data.rank[i].time.ToString() + "\n\n";
            }
            data = null;
        }
    }


    public void RankView()
    {
        button[focusBtn].image.sprite = normalSprite;
        focusBtn = 2;
        button[focusBtn].image.sprite = texture;
        StartCoroutine(PostNetworkingWithWebRequest());
        btnCanvas.gameObject.SetActive(false);
        rankCanvas.gameObject.SetActive(true);
    }

    public void GameStart()
    {
        Time.timeScale = 0f;
        childImg.canvasRenderer.SetAlpha(1f);
        childImg.CrossFadeAlpha(254f, 2f, true);
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
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            switch(button[focusBtn].name)
            {
                case "Ranking":
                    RankView();
                    break;
                case "GameStart":
                    GameStart();
                    break;

            }
        }
    }

    IEnumerator PostNetworkingWithWebRequest()
    {
        //List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //form.Add(new MultipartFormDataSection("name", "KAWK"));
        //form.Add(new MultipartFormDataSection("score", "123"));
        //UnityWebRequest webRequest = UnityWebRequest.Post("https://kwaksboard.herokuapp.com/process/addrank", form);
        //yield return webRequest.SendWebRequest();
        //string result = webRequest.downloadHandler.text;

        //WWWForm form = new WWWForm();
        ////form.AddField("name", "kawk");
        ////form.AddField("score", "1123");
        //WWW www = new WWW("https://kwaksboard.herokuapp.com/process/listrank", form);

        //yield return www;
        //string result = www.text;
        //Debug.Log(result);

        Time.timeScale = 0f;
        WWWForm form = new WWWForm();
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:3000/GetRank", form);

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error");
        }
        else
        {
            string result = webRequest.downloadHandler.text;
            Debug.Log(result);
            result = "{\"rank\":" + result + "}";
            data = JsonUtility.FromJson<RankResult>(result);
            Debug.Log(data.rank[0].name + "  " + data.rank[0].time);
            Time.timeScale = 1f;
        }
    }
}

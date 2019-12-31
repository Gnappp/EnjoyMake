using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Gam : MonoBehaviour
{
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

    public Text timeText;
    public InputField inputName;
    public Canvas insertUI;
    public Canvas gameoverUI;
    public Text gameoveText;

    private string clearTime;
    private string userName;
    private RankResult data;
    private bool clear=false;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && gameoverUI.gameObject.active)
        {
            Restart();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && gameoverUI.gameObject.active)
        {
            GoMenu();
        }
    }

    public void InsertName()
    {
        Debug.Log("SpawnScript and object is active " + gameObject.activeInHierarchy);
        StartCoroutine(SetRank());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clearTime = timeText.text;
            Time.timeScale = 0f;
            StartCoroutine(GetRank());
            gameoverUI.gameObject.SetActive(true);
            gameoveText.text = "!!CLEAR!!";
            clear = true;
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadSceneAsync("Start");
        clear = false;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        GameManager.Instance.Set_gameover(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    void CompareRanking()
    {
        if (data.rank.Count <10 || data.rank[9].time > float.Parse(clearTime)) //꼴등이 삭제되어야하기때문에 꼴등보다 크면 랭킹진입
        {
            Debug.Log("ranker!");
            insertUI.gameObject.SetActive(true);
            Debug.Log(inputName.text);
        }
    }

    IEnumerator SetRank()
    {
        userName = inputName.textComponent.text;
        Time.timeScale = 0f;
        WWWForm form = new WWWForm();
        Rank rank = new Rank();
        RankResult rankResult = new RankResult();
        form.AddField("name", userName.ToString());
        form.AddField("time", clearTime.ToString());
        WWW www = new WWW("http://127.0.0.1:3000/SetRank", form);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        if (result!="error")
        {
            insertUI.gameObject.SetActive(false);
        }


        //string json = JsonUtility.ToJson(rank);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        //UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:3000/SetRank", form);
        ////webRequest.SetRequestHeader("Content-Type", "application/json");

        //yield return webRequest.SendWebRequest();
        //string result = webRequest.downloadHandler.text;
        //Debug.Log(webRequest.error);
    }

    IEnumerator GetRank()
    {
        Time.timeScale = 0f;
        WWWForm form = new WWWForm();
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:3000/GetRank", form);

        yield return webRequest.SendWebRequest();
        Debug.Log("SpawnScript and object is active " + gameObject.activeInHierarchy);
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        result = "{\"rank\":" + result + "}";
        data = JsonUtility.FromJson<RankResult>(result);
        CompareRanking();
    }
}

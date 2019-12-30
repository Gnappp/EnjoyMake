using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

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

    private string clearTime;
    private string userName;
    private RankResult data;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertName()
    {
        //userName = inputName.text;
        StartCoroutine(GetRank());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clearTime = timeText.text;
            StartCoroutine(GetRank());
        }
    }

    void CompareRanking()
    {
        if (data.rank[data.rank.Count - 1].time > float.Parse(clearTime)) //꼴등이 삭제되어야하기때문에 꼴등보다 크면 랭킹진입
        {
            Debug.Log("ranker!");
            insertUI.gameObject.SetActive(true);
        }
    }

    IEnumerator SetRank()
    {
        Time.timeScale = 0f;
        WWWForm form = new WWWForm();
        Rank rank = new Rank();
        RankResult rankResult = new RankResult();
        form.AddField("name", "sss");
        form.AddField("time", clearTime.ToString());
        WWW www = new WWW("http://127.0.0.1:3000/SetRank", form);
        yield return www;
        string result = www.text;
        Debug.Log(result);


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
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        result = "{\"rank\":" + result + "}";
        data = JsonUtility.FromJson<RankResult>(result);
        CompareRanking();
    }
}

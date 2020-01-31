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
    [Serializable]
    public class ErrorResult
    {
        public List<Error> error;
    }

    [Serializable]
    public class Error
    {
        public string code;
        public int errno;
        public string sqlMessage;
        public string sqlState;
        public int index;
        public string sql;
    }

    public Canvas insertUI;
    public Canvas gameclearUI;

    private InputField inputName;
    private Text errorText;
    private string clearTime;
    private string userName;
    private RankResult data;
    private ErrorResult errorResult;
    private bool clear=false;
   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && clear)
        {
            Restart();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && clear)
        {
            GoMenu();
        }
    }

    public void InsertName()
    {
        StartCoroutine(SetRank());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clearTime = GameManager.Instance.playTime.ToString();
            Time.timeScale = 0f;
            StartCoroutine(GetRank());
            Canvas inst = Instantiate(gameclearUI) as Canvas;
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
        SceneManager.LoadSceneAsync("Game1");
        Time.timeScale = 1f;
    }

    void CompareRanking()
    {
        if (data.rank.Count <10 || data.rank[9].time > float.Parse(clearTime)) //꼴등이 삭제되어야하기때문에 꼴등보다 크면 랭킹진입
        {
            Debug.Log("ranker!");
            Canvas inst = Instantiate(insertUI) as Canvas;
            inputName = inst.transform.GetChild(0).transform.GetChild(3).GetComponent<InputField>();
            errorText = inst.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
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
        if (result== "\"success\"")
        {
            insertUI.gameObject.SetActive(false);
        }
        else if(result!="success")
        {
            result = "{\"error\":[" + result + "]}";
            errorResult = JsonUtility.FromJson<ErrorResult>(result);
            Debug.Log(errorResult.error.Count);
            switch(errorResult.error[0].errno)
            {
                case 1062: //중복된 name이 있을경우
                    inputName.text = " ";
                    errorText.text = "Duplicate name";
                    break;
                case 1406: //이름이 길때
                    inputName.text = " ";
                    errorText.text = "name is long";
                    break;
            }
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
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        result = "{\"rank\":" + result + "}";
        data = JsonUtility.FromJson<RankResult>(result);
        CompareRanking();
    }
}

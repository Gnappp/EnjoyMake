using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
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


    public float playTime = 0f;
    public float canvasAlpha = 1f;

    public Canvas insertUI;
    public Canvas gameclearUI;

    
    private Canvas insert_ptr;
    private Canvas gameclear_ptr;
    private InputField inputName;
    private Text errorText;
    private string clearTime;
    //private string userName;
    private RankResult data;
    private ErrorResult errorResult;
    private bool clear = false;
    private bool ranker = false;
    public ActionManager actionManager = new ActionManager();

    private bool gameover;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance==null)
            {
                var obj = FindObjectOfType<GameManager>();
                if(obj!=null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("GameManager").AddComponent<GameManager>();
                    instance = newSingleton;
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
        
    }

    private void Awake()
    {
        
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        gameover = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.clear && GameManager.Instance.gameclear_ptr.gameObject.active)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GameManager.Instance.gameclear_ptr.gameObject.SetActive(false);
                GameManager.Instance.actionManager.GoMenu();
            }
        }
    }



    public bool Get_gameover()
    {
        return gameover;
    }
    public void Set_gameover(bool set_gameover)
    {
        gameover = set_gameover;
    }

    public void GameClear()
    {
        Time.timeScale = 0f;
        clearTime = GameManager.Instance.playTime.ToString();
        clear = true;
        StartCoroutine(GetRank());
        if (insert_ptr == null)
        {
            Canvas inst = Instantiate(gameclearUI) as Canvas;
            gameclear_ptr = inst;
            inst.gameObject.SetActive(true);
        }
        else
            gameclear_ptr.gameObject.SetActive(true);
    }


    public void OnClickInsertName()
    {
        GameManager.Instance.StartCoroutine(SetRank(GameManager.Instance.inputName.text, GameManager.Instance.Get_clearTime()));
    }

    IEnumerator SetRank(String userName, string cleartime)
    {
        Time.timeScale = 0f;
        WWWForm form = new WWWForm();
        Rank rank = new Rank();
        RankResult rankResult = new RankResult();
        form.AddField("name", userName.ToString());
        form.AddField("time", cleartime);
        WWW www = new WWW("http://127.0.0.1:3000/SetRank", form);
        yield return www;
        string result = www.text;

        if (result == "\"success\"")
        {
            GameManager.Instance.insert_ptr.gameObject.SetActive(false);
            Debug.Log(result);
        }
        else if (result != "success")
        {
            result = "{\"error\":[" + result + "]}";
            errorResult = JsonUtility.FromJson<ErrorResult>(result);
            Debug.Log(errorResult.error.Count);
            switch (errorResult.error[0].errno)
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

    void CompareRanking()
    {
        if (data.rank.Count < 10 || data.rank[9].time > float.Parse(clearTime)) //꼴등이 삭제되어야하기때문에 꼴등보다 크면 랭킹진입
        {
            Debug.Log("ranker!");

            if (insert_ptr == null)
            {
                Canvas inst = Instantiate(insertUI) as Canvas;
                insert_ptr = inst;
                inst.gameObject.SetActive(true);
            }
            else
                insert_ptr.gameObject.SetActive(true);

            ranker = true;
            //inputName = inst.transform.GetChild(0).transform.GetChild(3).GetComponent<InputField>();
            //errorText = inst.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            inputName = insert_ptr.transform.GetChild(1).GetComponent<InputField>();
            errorText = insert_ptr.transform.GetChild(3).GetComponent<Text>();
            if (inputName.isFocused == false)
            {
                EventSystem.current.SetSelectedGameObject(inputName.gameObject, null);
                inputName.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
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
        Debug.Log(data != null);
        CompareRanking();
    }
    public string Get_clearTime()
    {
        return clearTime;
    }
}

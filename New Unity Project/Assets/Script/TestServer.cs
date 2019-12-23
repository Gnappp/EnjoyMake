using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class TestServer : MonoBehaviour
{
    public Text[] text;
    public Button[] button;
    public Sprite texture;
    private Sprite normalSprite;
    private int focusBtn = 0;

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
        normalSprite = button[focusBtn].image.sprite;
        button[focusBtn].image.sprite = texture;
    }

    // Update is called once per frame
    void Update()
    {
        ButtonFocus();
    }

    void ButtonFocus()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (focusBtn == button.Length - 1)
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
                focusBtn = button.Length - 1;
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
            Debug.Log(focusBtn + 1);
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


        WWWForm form = new WWWForm();
        //form.AddField("name", "kawk");
        //form.AddField("score", "1123");
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:3000/GetRank", form);

        yield return webRequest.SendWebRequest();
        string result = webRequest.downloadHandler.text;
        Debug.Log(result);
        result= "{\"rank\":" + result + "}";
        RankResult data = JsonUtility.FromJson<RankResult>(result);
        Debug.Log(data.rank[0].name + "  " + data.rank[0].time); 
    }
}

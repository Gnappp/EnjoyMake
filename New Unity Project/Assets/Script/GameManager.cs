using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
       
    }

    public void GameOver()
    {
        gameover = true;
    }

    public bool Get_gameover()
    {
        return gameover;
    }
    public void Set_gameover(bool set_gameover)
    {
        gameover = set_gameover;
    }
}

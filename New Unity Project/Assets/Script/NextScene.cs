using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
   
    public Text timeText;
    public Vector3[] randomDoorPos;
    
    private string nextScene = "Boss";
    private HashSet<Vector3> ranPos;
    private bool inPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //ranPos.Add(new Vector3(-16.28f, 2.502f, 0));
        //ranPos.Add(new Vector3(12.312f, 7.108f, 0));
        
        gameObject.transform.position = randomDoorPos[Random.Range(0, 1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            Time.timeScale = 0f;
            //GameManager.Instance.playTime += float.Parse(timeText.text);
            inPlayer = true;
            SceneManager.LoadScene("Boss");
            Time.timeScale = 1f;
            Input.ResetInputAxes();
        }
    }

}

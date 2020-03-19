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
    private GameObject curtain;
    private Image childImg;
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
        if(curtain==null)
        {
            curtain = GameObject.Find("Curtain(Clone)");
            Debug.Log(curtain);
            childImg = curtain.transform.GetChild(0).GetComponent<Image>();
        }
        if (childImg.canvasRenderer.GetAlpha() == 254f && inPlayer)
        {
            SceneManager.LoadScene(nextScene);
            GameManager.Instance.canvasAlpha = childImg.canvasRenderer.GetAlpha();
            inPlayer = false;
        }
    }

    private void FixedUpdate()
    {
       
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            Time.timeScale = 0f;
            childImg.canvasRenderer.SetAlpha(1f);
            childImg.CrossFadeAlpha(254f, 2f, true);
            inPlayer = true;
            Input.ResetInputAxes();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    public Canvas curtain;
    public Text timeText;

    private string nextScene = "Boss";
    private HashSet<Vector3> ranPos;
    private Image childImg;
    private Color c;
    private bool inPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //ranPos.Add(new Vector3(-16.28f, 2.502f, 0));
        //ranPos.Add(new Vector3(12.312f, 7.108f, 0));

        childImg = curtain.transform.GetChild(0).GetComponent<Image>();
        c = childImg.color;
        Debug.Log(childImg.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(inPlayer && c.a<1)
        {
            c.a += Time.deltaTime /2;
            childImg.color = c;
        }
        else if(inPlayer&&c.a>1)
        {
            SceneManager.LoadSceneAsync(nextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            inPlayer = true;
            Input.ResetInputAxes();
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<CapsuleCollider2D>().enabled = false;
            timeText.GetComponent<TimeText>().StopTimer();
        }
    }

}

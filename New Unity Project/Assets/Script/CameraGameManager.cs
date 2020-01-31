using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraGameManager : MonoBehaviour
{
    public Canvas curtain;

    private GameObject player;
    private Image childImg;
    private bool openCartain=false;

    // Start is called before the first frame update
    void Start()
    {
        player = player = GameObject.Find("Player");
        Canvas inst = Instantiate(curtain) as Canvas;
        childImg = inst.transform.GetChild(0).GetComponent<Image>();

        //SetCartain();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = pos;
        if(Time.timeScale==0 && childImg.canvasRenderer.GetAlpha() == 1f && openCartain)
        {
            Time.timeScale = 1f;
            GameManager.Instance.canvasAlpha = childImg.canvasRenderer.GetAlpha();
            openCartain = false;
        }
        if(GameManager.Instance.canvasAlpha == 254f && !openCartain)
        {
            openCartain = true;
            SetCartain();
        }
    }

    void SetCartain()
    {
        if(GameManager.Instance.canvasAlpha==254f)
        {
            Time.timeScale = 0f; 
            childImg.canvasRenderer.SetAlpha(254f);
            childImg.CrossFadeAlpha(1f, 2f, true);
            openCartain = true;
        }
    }
}

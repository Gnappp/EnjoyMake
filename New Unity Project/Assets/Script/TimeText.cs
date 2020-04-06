using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    private float timeText;
    private bool stopTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GameManager.Instance.playTime;
        Debug.Log(GameManager.Instance.playTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeText += Time.deltaTime;
        GameManager.Instance.playTime = timeText;
    }
    private void FixedUpdate()
    {
        gameObject.GetComponent<Text>().text = (GameManager.Instance.playTime).ToString("N3");
    }

    public float Get_timeText()
    {
        return timeText;
    }

    public void Reset_timeTest()
    {
        
    }
    public void StopTimer()
    {
        stopTimer = true;
        GameManager.Instance.playTime = timeText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    private float timeText=0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeText += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        gameObject.GetComponent<Text>().text = (timeText).ToString("N3");
    }

    public float Get_timeText()
    {
        return timeText;
    }
}

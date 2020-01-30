using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : MonoBehaviour
{
    public GameObject solarBomb;

    private float bombTime;
    private GameObject player;
    private GameObject inst = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inst == null)
        {
            inst = Instantiate(solarBomb) as GameObject;
            inst.transform.position = transform.position;
            inst.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        else if (inst != null)
        {
            if (bombTime > 3)
            {
                player = GameObject.Find("Player");
                Vector2 pos = player.transform.position - transform.position;
                float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
                inst.GetComponent<Rigidbody2D>().velocity = pos.normalized * 2f;
                inst = null;
                bombTime = 0f;
            }
            else if (bombTime < 3)
            {
                bombTime += Time.deltaTime;
                inst.transform.localScale += new Vector3(Time.deltaTime / 3, Time.deltaTime / 3, Time.deltaTime / 3);
            }
        }
    }
}
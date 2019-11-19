using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float last_pos;
    // Start is called before the first frame update
    void Start()
    {
        DestroyObject(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Wall"||collision.transform.tag=="Bottom")
        {
            DestroyObject(this.gameObject);
        }
    }

}

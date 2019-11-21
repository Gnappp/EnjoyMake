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
        if(collision.transform.tag=="Monster")
        {
            if(collision.gameObject.name=="Monster_Eagle")
            {
                Monster_Eagle mob = collision.gameObject.GetComponent<Monster_Eagle>();
                mob.Set_hp(1);
            }
            else if (collision.gameObject.name == "Monster_Opossum")
            {
                Debug.Log("hit opos");
                Monster_Opossum mob = collision.gameObject.GetComponent<Monster_Opossum>();
                mob.Set_hp(1);
            }
        }
        if(!(collision.transform.tag=="Player"))
            DestroyObject(this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float last_pos;
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        DestroyObject(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(collision.transform.tag=="Player" || collision.transform.tag == "Debuff"))
            DestroyObject(this.gameObject);
    }
    public int Get_Damage()
    {
        return damage;
    }

}

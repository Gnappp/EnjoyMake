using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameObject gas;

    private float createTime = 0f;
    private BoxCollider2D bc2d;
    private int hp = 1;

    // Start is called before the first frame update
    void Start()
    {
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        bc2d.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bc2d.enabled)
        {
            if (createTime > 1)
                bc2d.enabled = true;
            else
                createTime += Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==8)///Layer No.8 is Player
        {
            GameObject inst = Instantiate(gas) as GameObject;
            inst.gameObject.transform.position = gameObject.transform.position;
            inst.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            DestroyObject(this.gameObject);
        }
        if (collision.transform.tag == "Bullet")
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            Set_hp(bullet.Get_Damage());
        }
    }

    public void Set_hp(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            DestroyObject(this.gameObject);
        }
    }
}

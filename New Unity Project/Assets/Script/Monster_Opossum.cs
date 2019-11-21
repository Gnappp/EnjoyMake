using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Opossum : MonoBehaviour
{
    private int hp;
    private int life;
    private float deathTime;
    private Rigidbody2D rd2d;
    private BoxCollider2D bc2d;
    private bool right;
    private bool death;


    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
        life = 3;
        rd2d = gameObject.GetComponent<Rigidbody2D>();
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        right = false;
        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (death)
        {
            if(Time.time-deathTime>2)
            {
                life--;
                hp = 3;
                bc2d.enabled = true;
                death=false;
            }
        }
        if (!death)
        {
            Moving();
        }
    }

    private void Moving()
    {
        if (right)
        {
            rd2d.velocity = new Vector2(1f, rd2d.velocity.y);
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (!right)
        {
            rd2d.velocity = new Vector2(-1f, rd2d.velocity.y);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Wall")
        {
            if (right)
                right = false;
            else if (!right)
                right = true;
        }
    }
    public void Set_hp(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if (hp <= 0)
        {
            death = true;
            deathTime = Time.time;
            rd2d.velocity = new Vector2(0f,0f);
            bc2d.enabled = false;
        }
        if(life<=0)
        {
            DestroyObject(this.gameObject);
        }
    }
}

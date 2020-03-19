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
    private Animator animator;
    private bool right;
    private bool death;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
        life = 3;
        rd2d = gameObject.GetComponent<Rigidbody2D>();
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        right = false;
        death = false;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("Death"))
        {
            if (death)
            {
                if (Time.time - deathTime > 2)
                {
                    life--;
                    hp = 3;
                    bc2d.enabled = true;
                    death = false;
                }
            }
            if (!death)
            {
                Moving();
            }
        }
    }

    private void Moving()
    {
        int layerMask = ~(LayerMask.GetMask("Monster"));
        Vector2 pos_ptr = Vector2.zero;
        if (right)
            pos_ptr = new Vector2(transform.position.x + 0.05f, transform.position.y);
        else if (!right)
            pos_ptr = new Vector2(transform.position.x - 0.05f, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(pos_ptr, Vector2.down, 0.5f, layerMask);
        if (hit.collider == null )
        {
            right = !(right);
        }
        
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag=="Wall")
        {
            if (right)
                right = false;
            else if (!right)
                right = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bullet" )
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
            death = true;
            deathTime = Time.time;
            rd2d.velocity = new Vector2(0f,0f);
            bc2d.enabled = false;
        }
        if(life<=0)
        {
            rd2d.velocity = new Vector2(0f, 0f);
            bc2d.enabled = false;
            animator.SetBool("Death", true);
            DestroyObject(this.gameObject,0.5f);
        }
    }

    public void Test_Debug()
    {
        Debug.Log("RRRR");
    }
}

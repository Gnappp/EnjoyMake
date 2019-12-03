using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Frog : MonoBehaviour
{
    public GameObject gas;
    private Animator animator;
    private Rigidbody2D rd2d;
    private BoxCollider2D bc2d;
    private bool jumping;
    private bool right;
    private float idelTime;
    private float jumpTime;
    private float landTime;
    private Vector2 jumpOffset;
    private Vector2 jumpSize;
    private Vector2 idelOffset;
    private Vector2 idelSize;
    private int hp;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rd2d = gameObject.GetComponent<Rigidbody2D>();
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        idelTime = Time.time;
        jumpOffset = new Vector2(7.450581e-08f, 0.02858178f);
        jumpSize = new Vector2(0.2859314f, 0.2055036f);
        idelOffset = new Vector2(7.450581e-08f, 0.01142399f);
        idelSize = new Vector2(0.2859314f, 0.2398192f);
        hp = 1;
        right = false;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!jumping)
        {
            float x = player.transform.position.x - gameObject.transform.position.x;
            float y = player.transform.position.y - gameObject.transform.position.y;
            if (Mathf.Abs(x)<3 && Mathf.Abs(y)<3)
            {
                if (x > 0)
                    right = true;
                else
                    right = false;
            }
            else
            {
                int ran = Random.Range(0, 1);
                if (ran == 0)
                    right = false;
                else if (ran == 1)
                    right = true;
            }
        }
        if (Time.time-idelTime > 2 && !animator.GetBool("Down") && !animator.GetBool("Jump"))
        {
            animator.SetBool("Jump",true);
            jumpTime = Time.time;
            bc2d.size = jumpSize;
            bc2d.offset = jumpOffset;
            jumping = true;
            if (!right)
            {
                rd2d.velocity = new Vector2(-0.7f, 0.5f);
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (right)
            {
                rd2d.velocity = new Vector2(0.7f, 0.5f);
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }

        }

        if (animator.GetBool("Jump") && Time.time-jumpTime>0.5)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Down", true);
            landTime = Time.time;
            bc2d.size = idelSize;
            bc2d.offset = idelOffset;

            if (!right)
            {
                rd2d.velocity = new Vector2(-0.3f, -0.6f);
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (right)
            {
                rd2d.velocity = new Vector2(0.3f, -0.6f);
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            

        }

        if(animator.GetBool("Down")&& !jumping)
        {
            rd2d.velocity = new Vector2(0f, 0f);
            animator.SetBool("Down", false);
            idelTime = Time.time;
            bc2d.size = idelSize;
            bc2d.offset = idelOffset;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Bottom")
        {
            jumping = false;
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
            GameObject inst= Instantiate(gas) as GameObject;
            inst.transform.position = transform.position;
        }
    }
}

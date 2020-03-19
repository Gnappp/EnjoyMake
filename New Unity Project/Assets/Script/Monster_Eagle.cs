using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Eagle : MonoBehaviour
{
    private int hp;
    private int damage;
    private Rigidbody2D rd2d;
    private BoxCollider2D bc2d;
    private Animator animator;
    private bool findPlayer;
    private bool right;
    private float attackTime;
    private float findPlayerTime;
    private bool attackTurn;
    private float posTime;
    private float playerPos;
    private bool turning=false;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        hp = 1;
        damage = 1;
        rd2d = gameObject.GetComponent<Rigidbody2D>();
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        findPlayer = false;
        right = false;
        playerPos = 0f;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("Death"))
        {
            if (Time.time - attackTime > 2 && findPlayer && !attackTurn)
            {
                findPlayer = false;
            }

            Moving();
            Finding(Vector3.left);
            Finding(Vector3.right);

            Vector2 pos_ptr=Vector2.zero;
            if (right)
                pos_ptr = new Vector2(transform.position.x + 0.1f, transform.position.y);
            else if (!right)
                pos_ptr = new Vector2(transform.position.x - 0.1f, transform.position.y);

            int layerMask = ~(LayerMask.GetMask("Monster"));
            RaycastHit2D hit = Physics2D.Raycast(pos_ptr, Vector2.down, 0.5f,layerMask);
            if (hit.collider == null && !turning)
            {
                rd2d.velocity = Vector2.zero;
                turning = true;
                posTime += 0.3f;
                if (right)
                    right = false;
                else if (!right)
                    right = true;
            }
            else if (hit.collider != null)
            {
                if (hit.collider.transform.tag == "Bottom" || hit.collider.transform.tag == "Wall")
                {
                    turning = false;
                }
            }
        }
    }

    private void Moving()
    {
        if (!findPlayer)
        {
            if (Time.time - posTime > 1.5f && !findPlayer)
            {
                posTime = Time.time;
                if (right)
                    right = false;
                else if (!right)
                    right = true;
            }
            if (right)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
                rd2d.velocity = new Vector2(0.5f, rd2d.velocity.y);
                //  right = false;
            }
            else if (!right)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rd2d.velocity = new Vector2(-0.5f, rd2d.velocity.y);
                //  right = true;
            }
        }

       

        if(findPlayer)
        {
            if(right)
            {
                if(playerPos<transform.position.x)
                {
                    rd2d.velocity = new Vector2(0f, 0f);
                    animator.SetBool("Find", false);
                }
            }
            else if(!right)
            {
                if(playerPos>transform.position.x)
                {
                    rd2d.velocity = new Vector2(0f, 0f);
                    animator.SetBool("Find", false);
                }
            }
        }
    }

    private void Finding(Vector3 vec)
    {
        RaycastHit2D hit;
        if (!findPlayer)
        {
            int layerMask = ~(LayerMask.GetMask("Monster"));
            hit = Physics2D.Raycast(transform.position, vec, 1.5f,layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    animator.SetBool("Find", true);
                    findPlayerTime = Time.time;
                    attackTurn = true;
                    findPlayer = true;
                    playerPos = hit.transform.position.x;
                    if (playerPos - transform.position.x > 0)
                    {
                        right = true;
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                        rd2d.velocity = new Vector2(0f, rd2d.velocity.y);
                        playerPos += 0.5f;
                    }
                    else if (playerPos - transform.position.x < 0)
                    {
                        right = false;
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        rd2d.velocity = new Vector2(0f, rd2d.velocity.y);
                        playerPos -= 0.5f;
                    }
                }
            }
        }
        else if (Time.time - findPlayerTime > 1 && attackTurn)
            Attack();
    }

    private void Attack()
    {
        attackTime = Time.time;
        attackTurn = false;
        if (right && !turning)
        {
            rd2d.velocity = new Vector2(3f, rd2d.velocity.y);
        }
        else if(!right && !turning)
        {
            rd2d.velocity = new Vector2(-3f, rd2d.velocity.y);
        }
        animator.SetBool("Find", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            rd2d.velocity = new Vector2(0f, 0f);
            bc2d.enabled = false;
            rd2d.isKinematic = true;
            animator.SetBool("Death",true);
            DestroyObject(this.gameObject, 0.5f);
        }
    }
}

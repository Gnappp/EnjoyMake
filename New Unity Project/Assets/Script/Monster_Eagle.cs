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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - attackTime > 3 && findPlayer && !attackTurn)
        {
            findPlayer = false;
        }

        Moving();
        Finding(Vector3.left);
        Finding(Vector3.right);
    }

    private void Moving()
    {
        if (Time.time - posTime > 1.5f && !findPlayer)
        {
            posTime = Time.time;
            if (right)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
                rd2d.AddForce(new Vector2(2f, 0), ForceMode2D.Impulse);
                right = false;
            }
            else if (!right)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rd2d.AddForce(new Vector2(-2f, 0), ForceMode2D.Impulse);
                right = true;
            }
        }
    }

    private void Finding(Vector3 vec)
    {
        RaycastHit2D hit;
        if (!findPlayer)
        {
            bc2d.enabled = false;
            hit = Physics2D.Raycast(transform.position, vec, 1.5f);
            bc2d.enabled = true;
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    animator.SetBool("Find", true);
                    findPlayerTime = Time.time;
                    attackTurn = true;
                    findPlayer = true;
                    Debug.Log("빔");
                    if (hit.transform.position.x - transform.position.x > 0)
                    {
                        right = true;
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                    }
                    else if (hit.transform.position.x - transform.position.x < 0)
                    {
                        right = false;
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                    }
                }
            }
        }
        else if (Time.time - findPlayerTime > 1 && attackTurn)
            Attack();
    }

    private void Attack()
    {
        animator.SetBool("Find", false);
        attackTime = Time.time;
        attackTurn = false;
        animator.enabled = true;
        if (right)
        {
            rd2d.AddForce(new Vector2(4f, 0f), ForceMode2D.Impulse);
        }
        else if(!right)
        {
            rd2d.AddForce(new Vector2(-4f, 0f), ForceMode2D.Impulse);

        }
    }
}

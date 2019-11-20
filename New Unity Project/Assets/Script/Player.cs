using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;

    float collision_pos_x;
    private bool right;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2;
    private BoxCollider2D bc2;
    private bool jump;
    private bool jumpchance;
    private Vector2 Crouch_Collider_Size;
    private Vector2 Crouch_Collider_Offset;
    private Vector2 Idel_Collider_Size;
    private Vector2 Idel_Collider_Offset;
    private bool crash_wall;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        bc2 = gameObject.GetComponent<BoxCollider2D>();
        jump = true;
        jumpchance = false;
        right = true;
        Idel_Collider_Size = new Vector2(0.1823192f, 0.2176738f);
        Idel_Collider_Offset = new Vector2(-0.008840397f, -0.05000013f);
        Crouch_Collider_Size = new Vector2(0.1823192f, 0.1088369f);
        Crouch_Collider_Offset = new Vector2(-0.008840397f, -0.1000003f);
    }

  
    // Update is called once per frame
    void Update()
    {
        Shooting();
        Jump();
        Moving();
        Crouching();
    }

    private void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject inst = Instantiate(bullet) as GameObject;
            if (right)
            {
                inst.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
                inst.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f, 0), ForceMode2D.Impulse);
            }
            else if (!right)
            {
                inst.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
                inst.GetComponent<Rigidbody2D>().AddForce(new Vector2(-4f, 0), ForceMode2D.Impulse);
            }
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !jump)
        {
            jumpchance = true;
            jump = true;
            bc2.isTrigger = true;
            animator.SetTrigger("Player_Jump");
            Debug.Log("Player Jump!");
        }
        if (rb2.velocity.y < -0 && !animator.GetBool("Player_Down"))
        {
            animator.SetBool("Player_Down", true);
            bc2.isTrigger = false;
            jump = true;
            jumpchance = false;
            Debug.Log("false");
        }

        Jumping();
    }
    
    private void Moving()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!animator.GetBool("Player_Move") && !jump)
                animator.SetBool("Player_Move", true);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-0.1f, 0) * 10 * Time.deltaTime;
                transform.rotation = new Quaternion(0, 180, 0, 0);
                right = false;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0.1f, 0) * 10 * Time.deltaTime;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                right = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (animator.GetBool("Player_Move"))
                animator.SetBool("Player_Move", false);
        }


    }
    private void Crouching()
    {
        if (Input.GetKey(KeyCode.DownArrow) && !jump)
        {
            if (!animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", true);
                Debug.Log("Crouch");
                bc2.size = Crouch_Collider_Size;
                bc2.offset = Crouch_Collider_Offset;
            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", false);
                Debug.Log("DownArrow Key Up!");
                bc2.size = Idel_Collider_Size;
                bc2.offset = Idel_Collider_Offset;
            }
        }
    }
    private void Jumping()
    {
        if (!jumpchance)
        {
            return;
        }
        rb2.velocity = Vector2.zero;
        Vector2 jumpVelo = new Vector2(0, 4f);
        rb2.AddForce(jumpVelo, ForceMode2D.Impulse);
        jumpchance = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bottom" && animator.GetBool("Player_Down"))
        {
            bc2.isTrigger = false;
            crash_wall = false;
            Debug.Log("Bottom");
            jump = false;
            animator.SetBool("Player_Down", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            collision_pos_x = transform.position.x ;
            transform.position = new Vector3(collision_pos_x, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            if(!crash_wall)
            {
                collision_pos_x = transform.position.x;
                crash_wall = true;
            }
            transform.position = new Vector3(collision_pos_x, transform.position.y, transform.position.z);
        }
    }

}

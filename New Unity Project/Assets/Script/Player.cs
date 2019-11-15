using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2;
    private BoxCollider2D bc2;
    private bool jump;
    private bool jumpchance;
    private bool stay_bottom;
    private float max_height;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        bc2 = gameObject.GetComponent<BoxCollider2D>();
        jump = true;
        jumpchance = false;
        stay_bottom = false;
        max_height = gameObject.transform.position.y;
    }

  
    // Update is called once per frame
    void Update()
    {
        Jump();
        Moving();
        Crouching();
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && stay_bottom)
        {
            jumpchance = true;
            jump = true;
            stay_bottom = false;
            /*if (animator.GetBool("Player_Move"))
            {
                animator.SetBool("Player_Move", false);
                Debug.Log("Player_Move false");
            }
            if (animator.GetBool("Player_Crouch"))
                animator.SetBool("Player_Crouch", false);*/

            animator.SetTrigger("Player_Jump");
            Debug.Log("Player Jump!");
        }
        if (rb2.velocity.y < -0 && !animator.GetBool("Player_Down"))
        {
            animator.SetBool("Player_Down", true);
            jump = true;
            jumpchance = false;
            stay_bottom = false;
            Debug.Log("false");
        }

        Jumping();
    }
    
    private void Moving()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!animator.GetBool("Player_Move"))
                animator.SetBool("Player_Move", true);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-0.1f, 0) * 10 * Time.deltaTime;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0.1f, 0) * 10 * Time.deltaTime;
                transform.rotation = new Quaternion(0, 0, 0, 0);
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
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //if (stay_bottom)
            //{
                if (!animator.GetBool("Player_Crouch"))
                {
                    animator.SetBool("Player_Crouch", true);
                    Debug.Log("Crouch");
                }
            //}
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", false);
                Debug.Log("DownArrow Key Up!");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bottom")
        {
            Debug.Log("Bottom");
            stay_bottom = true;
            jump = false;
            if (animator.GetBool("Player_Move") || animator.GetBool("Player_Crouch"))
            {
                Moving();
                Crouching();
                animator.SetBool("Player_Down", false);
                Debug.Log("Landing move");
            }
            animator.SetBool("Player_Down", false);
        }
    }

    
    

}

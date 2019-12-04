using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;

    private bool crushWall;
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
    private float jumpTime=0f;
    private float jumpingTime=0.4f;

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
        Jump2();
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
                inst.transform.position = new Vector3(bc2.bounds.center.x, bc2.bounds.center.y, 0);
                inst.GetComponent<Rigidbody2D>().velocity = new Vector2(4f, 0);
            }
            else if (!right)
            {
                inst.transform.position = new Vector3(bc2.bounds.center.x, bc2.bounds.center.y, 0);
                inst.GetComponent<Rigidbody2D>().velocity = new Vector2(-4f, 0);
            }
        }
    }

    //private void Jump2()
    //{
    //    if (Input.GetAxisRaw("Vertical") > 0f && jumpTime <= 0f)
    //    {
    //        Jump3(Input.GetAxisRaw("Vertical"));
    //        jumpTime = jumpingTime;
    //    }
    //    else
    //        jumpTime -= Time.deltaTime;
    //}

    //private void Jump3(float jumpVelocity)
    //{
    //    rb2.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);

    //}
    
    

    private void Moving()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            if (!animator.GetBool("Player_Move") && !jump)
                animator.SetBool("Player_Move", true);

            if (!crushWall)
            {
                if (moveHorizontal < 0)
                {
                    rb2.velocity = new Vector2(moveHorizontal * 1.5f, rb2.velocity.y);
                    if (right)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                        right = false;
                    }
                }
                else if (moveHorizontal > 0)
                {
                    rb2.velocity = new Vector2(moveHorizontal * 1.5f, rb2.velocity.y);
                    if (!right)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                        right = true;
                    }
                }
            }
            else if (crushWall)
            {
                if (moveHorizontal < 0 && right)
                {
                    rb2.velocity = new Vector2(moveHorizontal * 1.5f, rb2.velocity.y);
                    if (right)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                        crushWall = false;
                        right = false;
                    }
                }
                else if (moveHorizontal > 0 && !right)
                {
                    rb2.velocity = new Vector2(moveHorizontal * 1.5f, rb2.velocity.y);
                    if (!right)
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);

                        crushWall = false;
                        right = true;
                    }
                }
            }
        }
        

        if (moveHorizontal==0 && !jump)
        {
            if (animator.GetBool("Player_Move"))
            {
                animator.SetBool("Player_Move", false);
            }
        }

    }
    private void Crouching()
    {
        if (Input.GetKey(KeyCode.DownArrow) && !jump)
        {
            if (!animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", true);
                bc2.size = Crouch_Collider_Size;
                bc2.offset = Crouch_Collider_Offset;
            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", false);
                bc2.size = Idel_Collider_Size;
                bc2.offset = Idel_Collider_Offset;
            }
        }
    }

    private void Jump2()
    {
        float moveVertical = Input.GetAxis("Vertical");
        if(moveVertical>0)
        {
            if (Time.time - jumpTime > 0.5 && !jump)
            {
                rb2.AddForce(Vector2.up * 1f, ForceMode2D.Impulse);
                jumpTime = Time.time;
            }
            else if(Time.time-jumpTime<0.5 && moveVertical==0)
            {
                rb2.AddForce(Vector2.up * 1f, ForceMode2D.Impulse);
            }

            if (!jump)
            {
                jumpchance = true;
                jump = true;
                animator.SetTrigger("Player_Jump");
            }
        }

        if (rb2.velocity.y < -0.1 && !animator.GetBool("Player_Down"))
        {
            animator.SetBool("Player_Down", true);
            jump = true;
            jumpchance = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !jump)
        {
            jumpchance = true;
            jump = true;
            animator.SetTrigger("Player_Jump");
        }
        if (rb2.velocity.y < -0.1 && !animator.GetBool("Player_Down"))
        {
            animator.SetBool("Player_Down", true);
            jump = true;
            jumpchance = false;
        }

        Jumping();
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
        if (collision.gameObject.tag == "Bottom" && animator.GetBool("Player_Down"))
        {
            jump = false;
            animator.SetBool("Player_Down", false);
        }

        if(collision.gameObject.tag=="Wall")
        {
            crushWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {
            crushWall = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.transform.tag == "Debuff")
        {
            Debug.Log("Debuff");
        }

        if (collision.transform.tag == "Monster")
        {
            Debug.Log("Hit");
        }
    }

    

    //void OnCollisionEnter2D(Collision2D collision)
    //{ 
    //    if (collision.gameObject.tag == "Bottom" && jump &&!animator.GetBool("Player_Down"))
    //    {
    //        Collider2D collider = gameObject.GetComponent<Collider2D>();
    //        Debug.Log("??");
    //        Physics2D.IgnoreCollision(collision.collider, collider, true);
    //    }
    //}
}
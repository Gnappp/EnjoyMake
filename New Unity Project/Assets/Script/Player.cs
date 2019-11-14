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
    private bool jumping;
    private bool moving;
    private bool crouching;
    private Vector2 jump_height;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        bc2 = gameObject.GetComponent<BoxCollider2D>();
        jump = true;
        jumping = false;
        moving = false;
        crouching = false;
        jump_height = new Vector3(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if (!crouching)
            {
                animator.SetTrigger("KeyDown_Down");
                crouching = true;
            }
            Debug.Log("DownArrow Key Down!");
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (crouching)
            {
                animator.SetTrigger("KeyUp_Down");
                crouching = false;
            }
            Debug.Log("DownArrow Key Up!");
        }

        else if(Input.GetKey(KeyCode.RightArrow))
        {
            if (!moving)
            {
                animator.SetTrigger("KeyDown_Move");
                moving = true;
            }
            Debug.Log("RightArrow Key Down!");
            transform.position += new Vector3(0.1f, 0) * 10 * Time.deltaTime;
            transform.rotation = new Quaternion(0,0,0,0);
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            if(moving)
            {
                animator.SetTrigger("KeyUp_Move");
                moving = false;
            }
            Debug.Log("RightArrow Key Up!");
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!moving)
            {
                animator.SetTrigger("KeyDown_Move");
                moving = true;
            }
            Debug.Log("LeftArrow Key Down!");
            transform.position += new Vector3(-0.1f, 0) * 10 * Time.deltaTime;
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (moving)
            {
                animator.SetTrigger("KeyUp_Move");
                moving = false;
            }
            Debug.Log("LeftArrow Key Up!");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !jump)
        {
            jump = true;
            jumping = true;
            Debug.Log("UpArrow Key Down");
        }
        Jumping();
    }

    private void Jumping()
    {
        if (!jumping)
        {
            return;
        }
        rb2.velocity = Vector2.zero;
        Vector2 jumpVelo = new Vector2(0, 4f);
        rb2.AddForce(jumpVelo, ForceMode2D.Impulse);
        jumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Bottom")
            jump = false;
    }
}

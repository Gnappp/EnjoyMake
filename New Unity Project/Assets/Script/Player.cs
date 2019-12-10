using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private class Debuff
    {
        public string Name
        { get; }
        public float ContiuousTime
        { get; set; }
        public float StartTime
        { get; set; }

       
        public Debuff(string str,float s)
        {
            Name = str;
            ContiuousTime = s;
            StartTime = Time.time;
        }
    }

    public GameObject bullet;

    private bool crushWall;
    private bool right;
    private Animator animator;
    private Rigidbody2D rb2;
    private BoxCollider2D bc2;
    private bool jump;
    private bool jumpchance;
    private Vector2 Crouch_Collider_Size;
    private Vector2 Crouch_Collider_Offset;
    private Vector2 Idel_Collider_Size;
    private Vector2 Idel_Collider_Offset;
    private HashSet<Debuff> debuffSet;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        bc2 = gameObject.GetComponent<BoxCollider2D>();
        jump = true;
        jumpchance = false;
        right = true;
        Idel_Collider_Size = new Vector2(0.1823192f, 0.2176738f);
        Idel_Collider_Offset = new Vector2(-0.008840397f, -0.05000013f);
        Crouch_Collider_Size = new Vector2(0.1823192f, 0.1088369f);
        Crouch_Collider_Offset = new Vector2(-0.008840397f, -0.1000003f);
        debuffSet = new HashSet<Debuff>();
    }


    // Update is called once per frame
    void Update()
    {

        Shooting();
        Jump();
        Moving();
        Crouching();
        DebuffTimeCheck();
        DebuffCheck();
    }

    private void DebuffTimeCheck()
    {
        if (debuffSet.Count != 0)
        {
            foreach (Debuff de in debuffSet)
            {
                if (Time.time - de.StartTime > de.ContiuousTime)
                {
                    switch (de.Name)
                    {
                        case "RightLeftReverse":
                            right = !right;
                            break;
                        case "Paralysis":
                            rb2.constraints = RigidbodyConstraints2D.None;
                            rb2.freezeRotation = true;
                            animator.enabled = true;
                            break;
                        case "DubbleGravity":
                            rb2.gravityScale = rb2.gravityScale / 2;
                            break;
                    }
                    debuffSet.Remove(de);
                    return;
                }
                foreach (Debuff de2 in debuffSet) // 중복되는 것 남은 시간이 적은것 삭제
                {
                    if (de.Name == de2.Name && de != de2)
                    {
                        if ((de.ContiuousTime + de.StartTime - Time.time) < (de2.ContiuousTime + de2.StartTime - Time.time))
                        {
                            debuffSet.Remove(de);
                        }
                        else if ((de.ContiuousTime + de.StartTime - Time.time) > (de2.ContiuousTime + de2.StartTime - Time.time))
                        {
                            debuffSet.Remove(de2);
                        }
                        return;
                    }
                }

            }
        }
    }
    private void DebuffCheck()
    {
        if(debuffSet.Count!=0)
        {
            foreach(Debuff de in debuffSet)
            {
                switch(de.Name)
                {
                    case "RightLeftReverse":
                        right = !right;
                        break;
                    case "Paralysis":
                        
                        animator.enabled = false;
                        break;
                    case "DubbleGravity":
                        rb2.gravityScale = rb2.gravityScale * 2;
                        break;
                }
            }
        }
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

    private void Jump()
    {
        float moveVertical = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.UpArrow) && jumpchance)
        {
            if(jump&&!animator.GetBool("Player_Down"))
            {
                rb2.velocity= rb2.velocity+new Vector2(0f,(1-moveVertical) * 0.2f);
            }

            if (!jump)
            {
                jumpchance = true;
                jump = true;
                rb2.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
                if (rb2.velocity.y > 0.5)
                {
                    Debug.Log(rb2.velocity.y);
                    animator.SetTrigger("Player_Jump");
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.UpArrow) || 1-moveVertical==0)
        {
            jumpchance = false;
        }
        if(!jump && Input.GetKeyUp(KeyCode.UpArrow))
        {
            jumpchance = true;
        }

        if (rb2.velocity.y < -0.1 && !animator.GetBool("Player_Down"))
        {
            animator.SetBool("Player_Down", true);
            jump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bottom" && animator.GetBool("Player_Down"))
        {
            jump = false;
            jumpchance = true;
            animator.SetBool("Player_Down", false);
        }

        if(collision.gameObject.tag=="Wall")
        {
            crushWall = true;
        }
        
        if (collision.transform.tag == "Monster")
        {
            GameManager.Instance.GameOver();
            bc2.enabled = false;
        }
    }
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Bottom" && !animator.GetBool("Player_Down"))
    //    {
    //        jump = false;
    //        jumpchance = true;
    //        animator.SetBool("Player_Down", false);
    //    }
    //}

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
            Gas debuf = collision.gameObject.GetComponent<Gas>();
            Debuff debuff = new Debuff(debuf.Get_debuffName(), debuf.Get_sec());
            debuffSet.Add(debuff);
            Debug.Log(debuf.Get_debuffName());
        }
    }
}
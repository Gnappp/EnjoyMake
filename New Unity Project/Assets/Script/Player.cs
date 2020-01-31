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

    private bool right;
    private Animator animator;
    private Rigidbody2D rb2;
    private BoxCollider2D bc2;
    private CapsuleCollider2D cc2d;
    private bool jump;
    private bool jumpchance;
    private HashSet<Debuff> debuffSet;
    private float speed = 50f;
    private float jumpPower = 1f;
    private bool isGround;
    private float moveVertical = 0f;
    private float moveHorizontal = 0f;
    private bool crouch;



    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        bc2 = gameObject.GetComponent<BoxCollider2D>();
        cc2d = gameObject.GetComponent<CapsuleCollider2D>();
        jump = true;
        jumpchance = false;
        right = true;
        debuffSet = new HashSet<Debuff>();
    }


    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.UpArrow) && (moveVertical != 0 && !jump))
        {
            jump = true;
            jumpchance = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || moveVertical == 1)
        {
            jumpchance = false;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !jump)
        {
            crouch = true;
            rb2.mass = 100;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            crouch = false;
            rb2.mass = 1;
        }
    }

    void FixedUpdate()
    {
        Jump();
        Moving();
        Crouching();
        Landing();
        Shooting();
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
                            speed = 1f;
                            break;
                        case "Paralysis":
                            rb2.constraints = RigidbodyConstraints2D.None;
                            rb2.freezeRotation = true;
                            animator.enabled = true;
                            speed = 1f;
                            jumpPower = 1f;
                            break;
                        case "DubbleGravity":
                            rb2.gravityScale = 1f;
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
                        } else if ((de.ContiuousTime + de.StartTime - Time.time) > (de2.ContiuousTime + de2.StartTime - Time.time))
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
                        speed = -1f;
                        break;
                    case "Paralysis":
                        rb2.constraints = RigidbodyConstraints2D.FreezeAll;
                        speed = 0f;
                        jumpPower = 0f;
                        animator.enabled = false;
                        break;
                    case "DubbleGravity":
                        rb2.gravityScale = 2f;
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
            if (!crouch)
            {
                if (right)
                {
                    inst.transform.position = new Vector3(bc2.bounds.center.x, bc2.bounds.center.y, 0);
                    inst.GetComponent<Rigidbody2D>().velocity = new Vector2(4f, 0);
                    //Vector2 pos;
                    //pos = new Vector3(3.1f, 1.9f) - transform.position;
                    //float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
                    //inst.GetComponent<Rigidbody2D>().velocity = pos.normalized * 4f;
                }
                else if (!right)
                {
                    inst.transform.position = new Vector3(bc2.bounds.center.x, bc2.bounds.center.y, 0);
                    inst.GetComponent<Rigidbody2D>().velocity = new Vector2(-4f, 0);
                }
            }
            else if(crouch)
            {
                if (right)
                {
                    inst.transform.position = new Vector3(cc2d.bounds.center.x, cc2d.bounds.center.y, 0);
                    inst.GetComponent<Rigidbody2D>().velocity = new Vector2(4f, 0);
                }
                else if (!right)
                {
                    inst.transform.position = new Vector3(cc2d.bounds.center.x, cc2d.bounds.center.y, 0);
                    inst.GetComponent<Rigidbody2D>().velocity = new Vector2(-4f, 0);
                }
            }
        }
    }

    private void Moving()
    {
        if (Mathf.Abs(moveHorizontal) >= 0)
        {
            if (!animator.GetBool("Player_Move") && !jump)
                animator.SetBool("Player_Move", true);

            if (moveHorizontal < 0)
            {
                rb2.velocity = new Vector2(moveHorizontal * speed * Time.fixedDeltaTime, rb2.velocity.y);
                if (right)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                    right = false;
                }
            } else if (moveHorizontal > 0)
            {
                rb2.velocity = new Vector2(moveHorizontal * speed * Time.fixedDeltaTime, rb2.velocity.y);
                if (!right)
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    right = true;
                }
            } else if(moveHorizontal==0)
            {
                rb2.velocity = new Vector2(0, rb2.velocity.y);
            }

            if (moveHorizontal == 0 && !jump)
            {
                if (animator.GetBool("Player_Move"))
                {
                    animator.SetBool("Player_Move", false);
                }
            }

        }
    }
    private void Crouching()
    {
        if (crouch)
        {
            if (!animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", true);
                bc2.enabled = false;
            }

        }
        else if (!crouch)
        {
            if (animator.GetBool("Player_Crouch"))
            {
                animator.SetBool("Player_Crouch", false);
                bc2.enabled = true;
            }
        }
    }

    private void Landing()
    {
        if (rb2.velocity.y < -0.1 && !isGround)
        {
            jump = true;
            jumpchance = false;
            if (!animator.GetBool("Player_Down"))
            {
                animator.SetBool("Player_Jump", false);
                animator.SetBool("Player_Down", true);
            }
        }
    }

    private void Jump()
    {
        if(jump && jumpchance)
        {
            if (jump && !animator.GetBool("Player_Jump"))
            {

                jumpchance = true;
                jump = true;
                animator.SetBool("Player_Jump", true);
                rb2.AddForce(Vector2.up * 3f , ForceMode2D.Impulse);
            }
            if (jumpchance)
            {
                rb2.velocity = rb2.velocity + new Vector2(0f, 6f  *jumpPower * Time.deltaTime);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject == cc2d.IsTouching(collision.collider))
        {   
            jump = false;
            jumpchance = false;
            isGround = true;
            animator.SetBool("Player_Down", false);
        }
        if (collision.gameObject.layer == 11 || collision.gameObject.layer==13) //Layer No.11 is Monster, No.13 PassBottomMonster
        {
            GameOver();
        }
    }

    

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer==9) //Layer No.9 is Bottom
        {
            isGround = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.tag == "Debuff")
    //    {
    //        Gas debuf = collision.gameObject.GetComponent<Gas>();
    //        Debuff debuff = new Debuff(debuf.Get_debuffName(), debuf.Get_sec());
    //        debuffSet.Add(debuff);
    //    }
    //}

    public void debuffSet_Set(string debuffName, float sec)
    {
        Debuff debuff = new Debuff(debuffName, sec);
        debuffSet.Add(debuff);
    }

    public void GameOver()
    {
        GameManager.Instance.Set_gameover(true);
        bc2.enabled = false;
    }
}
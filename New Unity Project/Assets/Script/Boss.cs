using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float jump_delay = 4f;
    public GameObject seed;
    public GameObject solar;

    private int hp = 50;
    private GameObject player;
    private bool jumping = true;
    private float jump_time = 0f;
    private float createseedTime = 0f;
    private float freezeTime = 0f;
    private Rigidbody2D rb2d;
    private bool solarPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (gameObject.name != "Boss_Monster_Plant(Clone)")
            hp = 25;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        CreateSeed();
        Phase2();
    }

    void Jump()
    {
        if (Mathf.Abs(rb2d.velocity.y) > 1)
        {
            jumping = true;
            float ptr_pos_x = player.transform.position.x - gameObject.transform.position.x;
            rb2d.velocity = new Vector2(ptr_pos_x / 2f, rb2d.velocity.y);
        }
        else if (Mathf.Abs(rb2d.velocity.y) < 1)
        {
            jumping = false;
        }

        if (jump_time > jump_delay && !jumping)
        {
            float ptr_pos_x = player.transform.position.x - gameObject.transform.position.x;
            rb2d.AddForce(new Vector2(ptr_pos_x / 1.5f, 7.5f), ForceMode2D.Impulse);
            Debug.Log(ptr_pos_x / 1f + 8f);
            jump_time = 0f;
            jumping = true;
        }

        if (!jumping)
        {
            jump_time += Time.deltaTime;
        }

    }

    void CreateSeed()
    {
        if (createseedTime > 5 && gameObject.name != "Boss_Monster_Plant(Clone)")
        {
            GameObject inst = Instantiate(seed) as GameObject;
            inst.transform.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            createseedTime = 0f;
        }
        else
            createseedTime += Time.deltaTime;
    }
    void Phase2()
    {
        if (hp < 25 && gameObject.name != "Boss_Monster_Plant(Clone)" && !solarPhase)
        {
            if (jumping && rb2d.velocity.y < -0.1f)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                //GameObject inst = Instantiate(solar) as GameObject;
                //inst.transform.position = gameObject.transform.position;
                solarPhase = true;
            }
        }
        if (rb2d.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            if (freezeTime > 2f)
            {
                rb2d.constraints = RigidbodyConstraints2D.None;
                rb2d.freezeRotation = true;
            }
            else
                freezeTime += Time.deltaTime;
        }
    }
}

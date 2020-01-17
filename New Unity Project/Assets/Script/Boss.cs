using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float jump_delay = 4f;
    public GameObject seed;

    private int hp;
    private GameObject player;
    private bool jumping = true;
    private float jump_time = 0f;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        CreateSeed();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
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
            rb2d.AddForce(new Vector2(ptr_pos_x / 1.5f, 8f), ForceMode2D.Impulse);
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
        GameObject inst = Instantiate(seed) as GameObject;
        inst.transform.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }
}

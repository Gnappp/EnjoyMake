using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushObject : MonoBehaviour
{

    public GameObject mushroom;

    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8)
        {
            BushAction bushAction = collision.GetComponent<BushAction>();
            if(!bushAction.Get_hideBush())
            {
                sprite.color = Color.green;
                bushAction.Set_hideBush(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8)
        {
            BushAction bushAction = collision.GetComponent<BushAction>();
            if (!bushAction.Get_hideBush())
            {
                sprite.color = Color.green;
                bushAction.Set_hideBush(true);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8)
        {
            BushAction bushAction = collision.GetComponent<BushAction>();
            if (bushAction.Get_hideBush())
            {
                sprite.color = new Color(255f, 255f, 255f, 1f);
                bushAction.Set_hideBush(false);
            }
        }
    }
   
}

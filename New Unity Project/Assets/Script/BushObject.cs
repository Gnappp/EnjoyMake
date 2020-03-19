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
        Bush_Enter_Stay(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Bush_Enter_Stay(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Bush_Exit(collision);
    }

    void Bush_Enter_Stay(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 13)
        {
            StateAction stateAction = collision.GetComponent<StateAction>();
            if (!stateAction.Get_hideBush())
            {
                sprite.color = Color.green;
                stateAction.Set_hideBush(true);
            }
        }
    }

    void Bush_Exit(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 8 || collision.gameObject.layer == 13)
        {
            StateAction stateAction = collision.GetComponent<StateAction>();
            if (stateAction.Get_hideBush())
            {
                sprite.color = new Color(255f, 255f, 255f, 1f);
                stateAction.Set_hideBush(false);
            }
        }
    }

}

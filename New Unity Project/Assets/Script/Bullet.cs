using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float last_pos;
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        DestroyObject(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int passLayermask = (1 << LayerMask.NameToLayer("Player")) + (1 << LayerMask.NameToLayer("Bush")); //Layer No.8 is Player, No.15 DontDestoryObject, NO.14 Bush
        passLayermask = ~passLayermask;
        int attackLayermask = ~((1 << 10) | (1 << 11) | (1 << 13)); //Layer N0.10 DestructiveObject, No.11 Monster, No,13 PassBottomMonster
        if (!(collision.gameObject.layer == 8 || collision.gameObject.layer == 14 || collision.gameObject.layer == 15))
        {
            DestroyObject(this.gameObject);
        }
    }
    public int Get_Damage()
    {
        return damage;
    }

}

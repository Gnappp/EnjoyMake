using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAction : MonoBehaviour
{
    private bool hideBush = false;
    private BoxCollider2D bc2d;
    private Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Fall")
        {
            transform.position = startPos;
        }
    }

    public void Set_hideBush(bool boolean)
    {
        hideBush = boolean;
    }
    public bool Get_hideBush()
    {
        return hideBush;
    }



    //public void TagToFind(string name,int dmg)
    //{
    //    switch(name)
    //    {
    //        case "Monster_Opossum":
    //            GetComponent<Monster_Opossum>().Set_hp(dmg);
    //            break;
    //        case "Monster_Frog":
    //            GetComponent<Monster_Frog>().Set_hp(dmg);
    //            break;
    //        case "Monster_Eagle":
    //            GetComponent<Monster_Eagle>().Set_hp(dmg);
    //            break;
    //        case "Boss_Monster_Plant":
    //            GetComponent<Boss>().Set
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    private string debuffName;
    private float sec;

    // Start is called before the first frame update
    void Start()
    {
        DestroyObject(this.gameObject,3f);
        RandomDebuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomDebuff()
    {
        int ran = Random.Range(0, 2);
        sec = 4f;
        debuffName = "Paralysis";
        //switch(ran)
        //{
        //    case 0:
        //        debuffName = "RightLeftReverse";
        //        break;
        //    case 1:
        //        debuffName = "Paralysis";
        //        break;
        //    case 2:
        //        debuffName = "DubbleGravity";
        //        break;

        //}
    }
    public float Get_sec()
    {
        return sec;
    }
    public string Get_debuffName()
    {
        return debuffName;
    }
}

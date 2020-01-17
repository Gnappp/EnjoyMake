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
        int ran = Random.Range(0, 100)%3;
        sec = 3f;
        Debug.Log(ran);
        switch (ran)
        {
            case 0:
                debuffName = "RightLeftReverse";
                break;
            case 1:
                debuffName = "Paralysis";
                break;
            case 2:
                debuffName = "DubbleGravity";
                break;

        }
    }
    public float Get_sec()
    {
        return sec;
    }
    public string Get_debuffName()
    {
        return debuffName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==8) //Layer No.8 is Player
        {
            collision.gameObject.GetComponent<Player>().debuffSet_Set(debuffName, sec);
            DestroyObject(this.gameObject,1f);
        }
    }
}

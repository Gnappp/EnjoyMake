using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMonster : MonoBehaviour
{
    public GameObject[] monsters;

    private int randomNo;

    // Start is called before the first frame update
    void Start()
    {
        randomNo = Random.Range(0, monsters.Length );
        GameObject inst = Instantiate(monsters[randomNo]) as GameObject;
        inst.transform.position = transform.position;
        DestroyObject(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

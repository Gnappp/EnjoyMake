using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameManager : MonoBehaviour
{
    public GameObject GameManager;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = pos;
    }
}

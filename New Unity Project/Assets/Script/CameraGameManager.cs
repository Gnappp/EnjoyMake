using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraGameManager : MonoBehaviour
{
    private GameObject player;
    private bool openCartain=false;

    // Start is called before the first frame update
    void Start()
    {
        player = player = GameObject.Find("Player");

        //SetCartain();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Start")
        {
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = pos;
        }
    }

}

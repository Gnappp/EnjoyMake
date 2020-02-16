using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushAction : MonoBehaviour
{
    private bool hideBush = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_hideBush(bool boolean)
    {
        hideBush = boolean;
    }
    public bool Get_hideBush()
    {
        return hideBush;
    }
}

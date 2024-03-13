using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMove : MonoBehaviour
{

    public Transform moveTo;

    // Update is called once per frame
    void Update()
    {
        
        transform.position = moveTo.position;

    }
}

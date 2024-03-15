using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMove : MonoBehaviour
{

    [HideInInspector] public Transform moveTo;

    // Update is called once per frame
    void Update()
    {
        if (moveTo != null)
            transform.position = moveTo.position;

    }
}

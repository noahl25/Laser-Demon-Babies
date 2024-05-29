using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHat : MonoBehaviour
{
    public float speed = 10;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}

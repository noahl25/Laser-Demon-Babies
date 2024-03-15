using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject cam;

    public void IsLocalPlayer() {
        movement.enabled = true;
        cam.SetActive(true);
        Debug.Log("Set local.");
    }
}

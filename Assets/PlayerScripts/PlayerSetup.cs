using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject cam;
    public WallRunning wallrun;
    public Jetpack jetpack;
    public GameObject orientation;

    public void IsLocalPlayer() {
        movement.enabled = true;
        wallrun.enabled = true;
        jetpack.enabled = true;
        cam.SetActive(true);
        orientation.SetActive(true);
        Debug.Log("Set local.");
    }
}

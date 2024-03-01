using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public new Camera camera;

    public void IsLocalPlayer() {
        movement.enabled = true;
        camera.enabled = true;
        Debug.Log("Set local.");
    }
}

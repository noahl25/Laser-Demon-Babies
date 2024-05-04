using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivatePostProcessing : MonoBehaviour
{

    public PostProcessVolume volume;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            volume.enabled = !volume.enabled;
        }
    }
}

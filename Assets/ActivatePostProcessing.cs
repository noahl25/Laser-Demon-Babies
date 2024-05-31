using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivatePostProcessing : MonoBehaviour
{

    public PostProcessVolume volume;

    // Update is called once per frame
    void Start()
    {
        if (Settings.PostProcessingOff)
            volume.enabled = false;
        else 
            volume.enabled = true;
        
    }
}

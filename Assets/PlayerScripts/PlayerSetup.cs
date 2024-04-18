using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject cam;
    public WallRunning wallrun;
    public Jetpack jetpack;
    public GameObject orientation;
   // public AudioListener listener;
    [Space]
    public Image overlay;
    public float fadeInDur;

    public void IsLocalPlayer() {
        Application.targetFrameRate = -1;
        movement.enabled = true;
        wallrun.enabled = true;
        jetpack.enabled = true;
       // listener.enabled = true;
        cam.SetActive(true);
        orientation.SetActive(true);
        Debug.Log("Set local.");
        StartCoroutine("FadeIn");
        
    }

    private IEnumerator FadeIn() {

        yield return new WaitForSeconds(1f);
        overlay.CrossFadeAlpha(0, fadeInDur, false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    [Header("Refs")]
    public Movement movement;
    public GameObject cam;
    public WallRunning wallrun;
    public Jetpack jetpack;
    public GameObject orientation;
    public AudioListener listener;
    [Header("Setup")]
    public Image overlay;
    public float fadeInDur;
    public TextMeshPro nameText;
    public GameObject nameHolder;

    [HideInInspector] public string playerName;

    public void IsLocalPlayer() {
        Application.targetFrameRate = -1;
        movement.enabled = true;
        wallrun.enabled = true;
        jetpack.enabled = true;
        listener.enabled = true;
        cam.SetActive(true);
        orientation.SetActive(true);
        Debug.Log("Set local.");
        StartCoroutine("FadeIn");
        
    }

    private IEnumerator FadeIn() {

        yield return new WaitForSeconds(1f);
        overlay.CrossFadeAlpha(0, fadeInDur * Time.deltaTime, false);

    }

    public void HideName() {
        nameHolder.SetActive(false);
    }

    [PunRPC]
    public void SetName(string _name, Health.Team team) {
        playerName = _name;
        nameText.text = _name;

        if (team == Health.Team.RED) {
            nameText.color = new Color(255, 0, 0, 255);
        }
        else if (team == Health.Team.BLUE) {
            nameText.color = new Color(0, 0, 255, 255);
        }

    }
}

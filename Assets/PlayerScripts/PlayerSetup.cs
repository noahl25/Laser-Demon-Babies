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
    public Transform syncedWeaponHolder;
    [Header("Setup")]
    public Image overlay;
    public float fadeInDur;
    public TextMeshPro nameText;
    public GameObject nameHolder;
    public GameObject demonBabyMesh;
    public PlayerPhotonAnimationManager photonAnimationManager;

    [HideInInspector] public string playerName;

    public void IsLocalPlayer() {

        syncedWeaponHolder.gameObject.SetActive(false);

        Application.targetFrameRate = -1;
        movement.enabled = true;
        wallrun.enabled = true;
        jetpack.enabled = true;
        orientation.SetActive(true);
        cam.SetActive(true);
        Debug.Log("Set local.");
        StartCoroutine("FadeIn");

        GetComponent<PhotonView>().RPC("SetupSyncedWeapons", RpcTarget.OthersBuffered, 0);
        
    }

    private IEnumerator FadeIn() {

        yield return new WaitForSeconds(1f);
        overlay.CrossFadeAlpha(0, fadeInDur * Time.deltaTime, false);

    }

    public void HideName() {
        nameHolder.SetActive(false);
    }

    [PunRPC]
    public void SetupSyncedWeapons(int weaponIndex) {

        foreach (Transform weapon in syncedWeaponHolder) {
            weapon.gameObject.SetActive(false);
        }

        syncedWeaponHolder.GetChild(weaponIndex).gameObject.SetActive(true);

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

    [PunRPC]
    public void SetupMeshes() {
        demonBabyMesh.SetActive(true);
    }

    [PunRPC]
    public void SetupAnimations() {
        photonAnimationManager.Init();
    }
}

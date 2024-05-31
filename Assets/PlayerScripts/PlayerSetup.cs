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
    //need a second overlay becuase first doesn't become opaque again (hacky but works)
    public GameObject overlay2;
    public float fadeInDur;
    public TextMeshPro nameText;
    public GameObject nameHolder;
    public GameObject demonBabyMesh;
    public PlayerPhotonAnimationManager photonAnimationManager;
    public GameObject laserHolder;
    public GameObject weaponCamera;

    [HideInInspector] public string playerName;

    public void IsLocalPlayer() {

        syncedWeaponHolder.gameObject.SetActive(false);

        Application.targetFrameRate = -1;
        movement.enabled = true;
        wallrun.enabled = true;
        jetpack.enabled = true;
        orientation.SetActive(true);
        cam.SetActive(true);
        StartCoroutine("FadeIn");

        GetComponent<PhotonView>().RPC("SetupSyncedWeapons", RpcTarget.OthersBuffered, 0);
        
    }

    private IEnumerator FadeIn() {

        yield return new WaitForSeconds(1f);
        while (overlay.color.a > 0) {
            overlay.CrossFadeAlpha(0f, fadeInDur, true);
            yield return null;
        }

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

        // foreach (Transform weapon in laserHolder.transform) {
        //     weapon.gameObject.layer = LayerMask.NameToLayer("Default");
        // }

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
        weaponCamera.layer = LayerMask.NameToLayer("Default");
        Transform weaponsParent = weaponCamera.transform.GetChild(0);
        foreach (Transform child in weaponsParent) {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void FadeInOverlay() {
        overlay.CrossFadeAlpha(1, 2f, false);
    }

    public void FadeFully() {
        overlay2.SetActive(true);
    }


    public void End() {
        movement.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic  = true;
        laserHolder.SetActive(false);
    }

    public void LobbySetup()
    {
        demonBabyMesh.SetActive(true);
        //set name
        //playerName = _name;
        //nameText.text = _name;
    }

}

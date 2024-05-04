using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WeaponSwitch : MonoBehaviour
{

    public PhotonView playerPV;

    public GameObject[] weapons;
    [Space]
    public Animation anim;
    public AnimationClip switchGun;
    [Space]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magText;

    [HideInInspector] public bool isSwitching = false;
    private GameObject currentWeapon;

    void Start() {
        currentWeapon = weapons[0];
        currentWeapon.SetActive(true);
    }

    void Update() {

        if (anim.isPlaying) {
            isSwitching = true;
        }
        else {
            isSwitching = false;
        }

        int weaponSelection = -1;

        if (Input.GetKeyDown("i")) {
            weaponSelection = 0;
        }
        if (Input.GetKeyDown("o")) {            
            weaponSelection = 1;
        }
        if (Input.GetKeyDown("p")) {
            weaponSelection = 2;
        }

        if (weaponSelection != -1 && weaponSelection < weapons.Length && !anim.isPlaying) {
            
            playerPV.RPC("SetupSyncedWeapons", RpcTarget.OthersBuffered, weaponSelection);
            
            anim.Play(switchGun.name); 

            currentWeapon.SetActive(false);
            weapons[weaponSelection].SetActive(true);
            currentWeapon = weapons[weaponSelection];

            magText.text = currentWeapon.GetComponent<Weapon>().mag.ToString();
            ammoText.text = currentWeapon.GetComponent<Weapon>().ammo + "/" + currentWeapon.GetComponent<Weapon>().magAmmo;
        }
        
        
    }

}
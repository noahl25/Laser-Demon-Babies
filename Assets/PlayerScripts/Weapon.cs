using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Camera cam;
    public CameraShake camShake;

    [Header("VFX")]
    public GameObject hitVFX;
    [Space]
    public GameObject muzzleFlash;
    public Transform muzzleFlashSpawn;
    [Space]
    public Vector3 shakeMag;
    public float shakeDur;
    [Header("Other")]
    public GameObject owner;
    public float moveForce;

    private float nextFire;

    // Update is called once per frame
    void Update()
    {

        if (nextFire > 0)
            nextFire -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && nextFire <= 0) {
            nextFire = 1 / fireRate;
            Fire();
        }

    }

    void Fire() {

        MuzzleFlash();
        CameraShake();

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f)) {

            if (hit.transform.gameObject != owner) {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            }
            
            if (hit.transform.gameObject.GetComponent<Health>() && hit.transform.gameObject != owner) {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Moveable")) {

                hit.rigidbody.AddForceAtPosition(ray.direction * moveForce, hit.point);
              //  Debug.Log("Applying");

            }


        }

    }

    void MuzzleFlash() {

        GameObject flash = PhotonNetwork.Instantiate(muzzleFlash.name, muzzleFlashSpawn.position, Quaternion.identity);
        flash.GetComponent<FlashMove>().moveTo = muzzleFlashSpawn;

    }

    void CameraShake() {

        StartCoroutine(camShake.shake(shakeMag, shakeDur));

    }
}

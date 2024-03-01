using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public new Camera camera;

    [Header("VFX")]
    public GameObject hitVFX;

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
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f)) {

            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            
            if (hit.transform.gameObject.GetComponent<Health>()) {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

            }

        }

    }
}

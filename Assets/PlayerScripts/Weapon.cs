using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using DG.Tweening;
using Photon.Pun.UtilityScripts;

public class Weapon : MonoBehaviour
{
    public GameObject basicgun;
    public GameObject scopedgun;

    public int damage;
    public float fireRate;
    public Camera cam;

    [Header("VFX")]
    public GameObject hitVFX;
    [Space]
    public GameObject muzzleFlash;
    public Transform muzzleFlashSpawn;
    [Space]
    public AudioClip laserSFX;
    public AudioClip outSFX;
    public Transform playAt;
    [Space]
    public float shakeMag;
    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;
    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;
    [Header("Other")]
    public GameObject owner;
    public float moveForce;
    public Animation anim;
    public AnimationClip reloadAnimation;
    public PlayerPhotonSoundManager playerPhotonSoundManager;
    [Header("Recoil")]
    [Range(0, 2)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 1f;
    public float recoilBack = 0f;
    [Space]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float snappiness;
    public float returnSpeed;
 
    private Vector3 originalPos;
    private Vector3 recoilVelocity = Vector3.zero;
    private bool recoiling;
    private bool recovering;
    private float recoilLength;
    private float recoverLength;

    private Vector3 currentRot;
    private Vector3 targetRot;

    private MouseLook ml;

    private float nextFire;

    public static bool doingAction = false;

    void Start() {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
        originalPos = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;

        ml = cam.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!anim.isPlaying) {
            doingAction = false;
        }
        if (nextFire > 0)
            nextFire -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && !anim.isPlaying) {
            nextFire = 1 / fireRate;
            ammo--;
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;
            //doingAction = true;
            Fire();
        }

        if (Input.GetButtonDown("Fire1") && ammo == 0) {
            AudioSource.PlayClipAtPoint(outSFX, playAt.position);
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0 && !anim.isPlaying) {
          //  doingAction = true;
            Reload();
        }

        if (recoiling) {
            Recoil();
        }
        if (recovering) {
            Recover();
        }


        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, snappiness * Time.fixedDeltaTime);

        ml.SetNextTween(currentRot);



    }

    void Reload() {

        anim.Play(reloadAnimation.name);

        if (mag > 0) {
            mag--;
            ammo = magAmmo;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
       // doingAction = true;
    }

    void Fire() {

        //doingAction = true;
        FX();
        CameraShake();

        recoiling = true;
        recovering = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;

       // PhotonNetwork.LocalPlayer.AddScore(1);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f)) {

            PhotonView view = hit.transform.gameObject.GetComponent<PhotonView>();
            Health health = hit.transform.gameObject.GetComponent<Health>();

            if (hit.transform.gameObject != owner) {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            }
            
            if (health && hit.transform.gameObject != owner) {


                if ((string)PhotonNetwork.CurrentRoom.CustomProperties["gamemode"] == "tdm") {

                    if (health.team != owner.GetComponent<Health>().team) {

                        PhotonNetwork.LocalPlayer.AddScore(damage);

                        if (damage >= hit.transform.gameObject.GetComponent<Health>().health) {
                            PhotonNetwork.LocalPlayer.AddScore(100);
                        }

                        view.RPC("TakeDamage", RpcTarget.All, damage);

                    }

                }

                else {
                
                    PhotonNetwork.LocalPlayer.AddScore(damage);

                    if (damage >= hit.transform.gameObject.GetComponent<Health>().health) {
                        PhotonNetwork.LocalPlayer.AddScore(100);
                    }

                    view.RPC("TakeDamage", RpcTarget.All, damage);
                    
                }
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Moveable")) {

                hit.rigidbody.AddForceAtPosition(ray.direction * moveForce, hit.point);

            }


        }

    }

    void FX() {

        GameObject flash = PhotonNetwork.Instantiate(muzzleFlash.name, muzzleFlashSpawn.position, Quaternion.identity);
        flash.GetComponent<FlashMove>().moveTo = muzzleFlashSpawn;

        playerPhotonSoundManager.PlayLaserSFX();

    }

    void CameraShake() {

       // CameraShaker.Instance.ShakeOnce(shakeMag, 3.0f, 0.1f, 0.3f);

    }

    void Recoil() {

        Vector3 finalPos = new Vector3(originalPos.x, originalPos.y + recoilUp, originalPos.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPos) {
            recoiling = false;
            recovering = true;
        }

        // cam.transform.DOLocalRotate(new Vector3(cam.transform.localEulerAngles.x - 1.0f,0,0), 0.25f);

        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

    }

    void Recover() {

        Vector3 finalPos = originalPos;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPos) {
            recoiling = false;
            recovering = false;
        }

    }
    
}

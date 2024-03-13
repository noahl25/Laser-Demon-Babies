using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : MonoBehaviour
{
    //public Animator animator;
    private bool isScoped = false;
    public GameObject Scope;
    public GameObject GunCam; //for this, you need to make a seperate layer for weapons, a cam for it, and change the culling mask to only weapons
    public Camera mainCam;
    public float scopedFOV = 15f;
    private float normalFOV;
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //isScoped = !isScoped;
            //animator.SetBool("Scoped", isScoped);
            Scope.SetActive(isScoped);

            if (isScoped)
                OnScoped();
            else
                UnScoped();
        }
    }
    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);

        Scope.SetActive(true);
        GunCam.SetActive(false);
        normalFOV = mainCam.fieldOfView;
        mainCam.fieldOfView = scopedFOV;

    }
    void UnScoped()
    {
        Scope.SetActive(false);
        GunCam.SetActive(true);
        mainCam.fieldOfView = normalFOV;
    }
}

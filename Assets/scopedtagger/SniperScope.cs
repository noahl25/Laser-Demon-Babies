using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : MonoBehaviour
{
    public Animator animator;
    private bool isScoped = false;
    public GameObject Scope;
    //public GameObject GunCam; //for this, you need to make a seperate layer for weapons, a cam for it, and change the culling mask to only weapons
    public MouseLook ml;
    public int scopedFOV = 15;
    private float normalFOV;
    public GameObject gun;
    public GameObject rip;
    void Update()
    {
        if (isScoped == false)
            Scope.SetActive(false);

        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);
            Scope.SetActive(isScoped);

            //  Debug.Log("scope");

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                UnScoped();
        }
    }
    IEnumerator OnScoped()
    {
        float timer = 0.0f;
        float waitTime = 0.05f;

        while (timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        ml.OverrideFov(scopedFOV, 0.1f);
        gun.SetActive(false);
        rip.SetActive(false);
        Scope.SetActive(true);
        //GunCam.SetActive(false);
       

    }
    void UnScoped()
    {
        Scope.SetActive(false);
        rip.SetActive(true);
        gun.SetActive(true);
        //GunCam.SetActive(true);
        ml.UnOverride();
    }
}

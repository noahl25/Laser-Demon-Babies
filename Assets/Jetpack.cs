using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    [Header("Variables")]
    public Image jetpackUI;
    public LayerMask ground;
    public float playerHeight;
    public float jetpackForce;

    private bool grounded;
    private float initJetpackUIPosY;

    private float maxJetpackFuel;
    private Rigidbody rb;
    private bool jetpackActive;

    // Start is called before the first frame update
    void Start()
    {
        initJetpackUIPosY = jetpackUI.rectTransform.position.y;
        maxJetpackFuel = jetpackUI.rectTransform.sizeDelta.y;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jetpackActive = false;
    }

    void FixedUpdate() {

        if (jetpackActive && !grounded) 
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Force);

         
    }

    // Update is called once per frame
    void Update()
    {
   
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        if (Input.GetKey(KeyCode.Space)) {

            jetpackUI.rectTransform.sizeDelta -= new Vector2(0.0f, 0.5f);
 
            if (jetpackUI.rectTransform.sizeDelta.y >= 0) 
                jetpackActive = true;
            else
                jetpackActive = false;


        }
        else {
            jetpackActive = false;
            if (grounded)
                jetpackUI.rectTransform.sizeDelta += new Vector2(0.0f, 0.5f);

        }

        jetpackUI.rectTransform.sizeDelta = new Vector2(jetpackUI.rectTransform.sizeDelta.x, Mathf.Clamp(jetpackUI.rectTransform.sizeDelta.y, 0, maxJetpackFuel));
        jetpackUI.rectTransform.position = new Vector3(jetpackUI.rectTransform.position.x, initJetpackUIPosY + (jetpackUI.rectTransform.sizeDelta.y / 2), jetpackUI.rectTransform.position.z);
        


        
    }
}

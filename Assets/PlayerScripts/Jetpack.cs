using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    [Header("Variables")]
    public RectTransform jetpackUI;
    public float jetpackForce;
    public float maxJetpackFuel;
    public float depletionRate;
    public float repletionRate;
    public float groundedRefuelDelay;
    public float maxYVel;
    public Movement movement;
    

    private float initJetpackUIPosY;
    private float initJetpackUIScaleY;
    private Rigidbody rb;
    private bool jetpackActive;
    private float currentFuel;
    private float groundedRefuelDelayTimer;

    // Start is called before the first frame update
    void Start()
    {
        initJetpackUIScaleY = jetpackUI.localScale.y;

        rb = GetComponent<Rigidbody>();
        jetpackActive = false;
        currentFuel = maxJetpackFuel;
    }

    void FixedUpdate() {

        if (jetpackActive && !movement.grounded) {
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Force);
        }


    }

    // Update is called once per frame
    void Update()
    {
        JetpackInput();
        HandleFuel();
        YSpeedControl();
        UpdateUI();
        
    }

    void JetpackInput() {
        if (Input.GetKey(movement.jumpKey) && currentFuel >= 1)
            jetpackActive = true;
        else
            jetpackActive = false;
    }

    void HandleFuel() {
        if (jetpackActive) {
            groundedRefuelDelayTimer = 0;
            currentFuel -= depletionRate;
            if (currentFuel < 0)
                currentFuel = 0;
        }
        else if (movement.grounded) {

            groundedRefuelDelayTimer += Time.deltaTime;

            if (groundedRefuelDelayTimer > groundedRefuelDelay) {

                currentFuel += repletionRate;
                if (currentFuel > maxJetpackFuel)
                    currentFuel = maxJetpackFuel;

            }
        }
    }

    void YSpeedControl() {

        if (rb.velocity.y > maxYVel && jetpackActive) {

            rb.velocity = new Vector3(rb.velocity.x, maxYVel, rb.velocity.z);

        }
        
    }

    void UpdateUI() {

        float pctFuel = currentFuel / maxJetpackFuel;

        jetpackUI.localScale = new Vector2(jetpackUI.transform.localScale.x, (initJetpackUIScaleY * pctFuel <= 0.01) ? 0 : initJetpackUIScaleY * pctFuel);


    }
}

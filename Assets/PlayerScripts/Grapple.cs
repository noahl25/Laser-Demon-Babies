using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    
    LineRenderer lineRenderer;
    [Header("Refs")]
    public GameObject spawn;
    public Camera cam;
    public LayerMask grappleable;
    public Rigidbody rb;
    public GameObject owner;
    public GameObject grappleGun;

    [Header("Grapple")]
    public float grappleForce;
    public float maxDistance;
    public float upwardForce;

    private Vector3 grappleHit;
    private GameObject grappleObjectHit;
    public bool set;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        GrappleInput();
        HandleLine();

        if (set)
            Active();

        
    }

    void GrappleInput() {
        
        if (Input.GetKeyDown(KeyCode.E)) {

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, grappleable) && Vector3.Distance(hit.transform.position, spawn.transform.position) < maxDistance ) {

                grappleHit = hit.point;
                grappleObjectHit = hit.transform.gameObject;
                set = true;


            }


        }

        if (Input.GetKeyUp(KeyCode.E)) {

            set = false;

        }
    }

    void HandleLine() {

        if (set) {

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spawn.transform.position);
            lineRenderer.SetPosition(1, grappleHit);

        }

        else {
            lineRenderer.enabled = false;
        }

    }

    void Active() {
        
        RaycastHit hit;
        if (Physics.Linecast(spawn.transform.position, grappleHit + ((spawn.transform.position - grappleHit).normalized * 0.5f), out hit) && hit.transform.gameObject != owner && hit.transform.gameObject != grappleGun) {
            set = false;
        }

        if (Vector3.Distance(spawn.transform.position, grappleHit) > maxDistance) {
            set = false;
        }

        rb.AddForce((grappleHit - spawn.transform.position).normalized * grappleForce, ForceMode.Force);
        rb.AddForce(Vector3.up * upwardForce, ForceMode.Force);


    

    }
}

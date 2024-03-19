using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    [Header("Wallrunning")]
    public LayerMask wall;
    public LayerMask ground;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float exitWallTime;
    private float exitWallTimer;
    private bool exiting;
    public bool useGravity;
    public float gravityCounterForce;
    public MouseLook ml;
    
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Refs")]
    public Transform orientation;
    private Movement mov;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        mov = GetComponent<Movement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        States();
    }

    private void CheckForWall() {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wall);
    }
    
    private bool AboveGround() {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    private void States() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exiting) {

            if (!mov.wallrunning)
                StartWallRun();

            if (Input.GetKeyDown(KeyCode.Space))
                WallJump();

        }
        else if (exiting) {
            if (mov.wallrunning)
                StopWallRun();
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0)
                exiting = false;
        }

        else {
            StopWallRun();
        }
    }

    void FixedUpdate() {
        if (mov.wallrunning)
            WallRunMovement();
    }

    private void StartWallRun() {

        mov.wallrunning = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (ml != null)
            ml.ToFov(70f);
        // if (wallLeft)
        //     ml.Tilt(-5f);
        // if (wallRight)
        //     ml.Tilt(5f);

    }

    private void WallRunMovement() {

        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;


        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);

        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);

    }

    private void StopWallRun() {
        mov.wallrunning = false;

        if (ml != null)
            ml.ToFov(60f);
        // ml.Tilt(0f);
    }

    private void WallJump() {

        exiting = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 force = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(force, ForceMode.Impulse);
    }
}

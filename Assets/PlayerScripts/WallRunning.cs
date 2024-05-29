using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the physics for this is really complicated, i code from a tutorial + edits to fix stuff and smooth everything
//https://www.youtube.com/watch?v=WfW0k5qENxM

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
        //raycast each way
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
            if (mov.wallrunning)
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

        if (ml != null) {
            if (wallLeft)
                ml.Tilt(-5f);
            if (wallRight)
                ml.Tilt(5f);
        }

    }

    private void WallRunMovement() {

        //gravity off so full control
        rb.useGravity = useGravity;

        //get where wall is facing
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //so you dont move backwards
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //keep you against wall
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);

        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);

    }

    private void StopWallRun() {
        mov.wallrunning = false;

        if (ml != null) {
            ml.ToFov(60f);
            ml.Tilt(0f);
        }
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

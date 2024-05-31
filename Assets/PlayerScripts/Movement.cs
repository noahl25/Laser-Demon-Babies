using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float wallRunSpeed;
    public float jetpackingAirMultiplier;
    
    bool readyToJump;

    public float walkSpeed;
    public float sprintSpeed;
    public float maxWalkSpeed = 12;
    public float maxSprintSpeed = 17;
    public float crouchSpeed;
    public float crouchYScale;
    public float jetpackingJumpMultiplier = 0.3f;
    private float startYScale;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public MouseLook ml;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    float maxSpeed;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Refs")]
    public Grapple grapple;
    public Vector3 startPos;
    public Camera cam;
    public Transform playerTransform;

    [Header("Animation")]
    public Animation handAnimation;
    public AnimationClip handWalkAnimation;
    public AnimationClip idleAnimation;
    public PlayerPhotonAnimationManager photonAnimationManager;

    [HideInInspector] public MovementState state;

    public enum MovementState {
        walking,
        sprinting,
        crouching,
        wallrunning,
        jetpacking,
        air
    }

    [HideInInspector] public bool wallrunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        handAnimation.AddClip(handWalkAnimation, "walk");
        handAnimation.AddClip(idleAnimation, "idle");
    }

    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        PlayerInput();
        SpeedControl();
        StateHandler();

        if (wallrunning)
            ml.ToFov(80f);
        else if (Input.GetKey(sprintKey))
            ml.ToFov(70f);
        else 
            ml.ToFov(60f);


        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(KeyCode.L)) {
            transform.position = startPos;
        }
    }

    private void StateHandler() {
        //handle all the possible states and set move speed/max speed
        if (wallrunning) {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey)) {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            maxSpeed = maxSprintSpeed;
        }
        else if (grounded) {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            maxSpeed = maxWalkSpeed;
        }
        else if (Input.GetKey(jumpKey)) {
            state = MovementState.jetpacking;
            moveSpeed = sprintSpeed;
            maxSpeed = maxSprintSpeed;
        }
        else {
            state = MovementState.air;
            moveSpeed = sprintSpeed;
            maxSpeed = maxSprintSpeed;
        }
        
    }

    private void MovePlayer()
    {

        Debug.Log(transform.rotation.y);
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z);

        moveDirection = forward * verticalInput + right * horizontalInput;

        Vector2 mag = new Vector2(horizontalInput, verticalInput);

        //animations
        if (!Weapon.doingAction && grounded) {
            if (mag.magnitude >= 0.5f) {
                handAnimation.CrossFade("walk", 0.5f);
            }
            else {
                handAnimation.CrossFade("idle", 0.5f);
            }

        }
        else {
            handAnimation.Stop();
        }

        if (mag.magnitude >= 0.5f) {
            photonAnimationManager.PlayWalkAnimationSynced();
        }
        else {
            photonAnimationManager.PlayIdleAnimationSynced();
        }

        //applying force
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * Time.fixedDeltaTime * (Jetpack.Instance.jetpackActive ? jetpackingAirMultiplier : airMultiplier), ForceMode.Force);


  
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        
    }

    private void Jump()
    {

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (state == MovementState.jetpacking || grapple.set) {
            rb.AddForce(transform.up * jumpForce * jetpackingJumpMultiplier, ForceMode.Impulse);
        }
        else {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
  
    }

}
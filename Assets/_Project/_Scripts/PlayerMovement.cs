using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Fix cancel jump early when gravity is flipped
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private PlayerAnimationHandler animationHandler;

    // Player Movement variables
    //--------------------------------------------------------
    // Horizontal Movement
    private float targetVelocityX; 
    private float currentVelocityX;

    [Header("Horizontal Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 50f;



    // Vertical Movement
    private float currentVelocityY;

    [Header("Gravity")]
    [SerializeField] private float minFallAcceleration = 1f;
    [SerializeField] private float maxFallAcceleration = 3f;
    [SerializeField] private float fallClamp = -40f;
    [SerializeField] private bool flipGravity = false;
    private float fallAcceleration;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 30;
    [SerializeField] private float jumpApexThreshold = 10f;
    [SerializeField] private float coyoteTimeThreshold = 0.1f;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float jumpEndEarlyGravityModifier = 3;

    private float timeLeftGround;
    [SerializeField] private bool coyoteUsable;
    private bool endedJumpEarly = true;
    private float apexPoint;
    private float lastJumpPressed;
    //private bool HasBufferedJump => IsGrounded && lastJumpPressed + jumpBuffer > Time.time;


    // Input variables
    private float XAxisInput;


    // Collision Checks
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;
    public bool IsGrounded { get; private set; }
    [Header("Head Check")]
    [SerializeField] private Transform headCheck;
    [SerializeField] private Vector2 headCheckSize;
    public bool hitHead { get; private set; }

    // States
    private States currentState = States.Moving;

    private enum States
    {
        Moving,
        Airborne
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void Start()
    {
        // When a jump is pressed, record the time (to be used for jump buffering)
        
    }

    private void Update()
    {
        

        
        GravityCheck();
        CollisionCheck();
        HandleStates();

        CalculateJumpApex();
        CalculateGravity();

        CalculateWalkSpeed();
        Move();
    }

    public void ReceiveHorizontalInput(float inputVector)
    {
        XAxisInput = inputVector;
    }

    private void Move()
    {
        rb.velocity = new Vector2(currentVelocityX, currentVelocityY);
    }

    private void GravityCheck()
    {
        if (!flipGravity)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, -1);
        }
    }

    private void CalculateWalkSpeed()
    {

        if (XAxisInput > 0)
        {
            targetVelocityX = XAxisInput * maxSpeed;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, acceleration * Time.deltaTime);

        }
        else if (XAxisInput < 0)
        {
            targetVelocityX = XAxisInput * maxSpeed;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, acceleration * Time.deltaTime);
        }
        else
        {
            targetVelocityX = 0f;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, deceleration * Time.deltaTime);

        }

        if(currentState == States.Moving && !InputHandler.Instance.isJumping)
        {
            if (XAxisInput != 0)
            {
                animationHandler.ChangeAnimationState("DuckWalk");
            }
            else
            {
                animationHandler.ChangeAnimationState("DuckIdle");
            }
        }


    }

    private void CollisionCheck()
    {
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        hitHead = Physics2D.OverlapBox(headCheck.position, headCheckSize, 0f, groundLayer);
    }

    private void HandleStates()
    {
        if (IsGrounded)
        {
            
        }

        // Player is "airborne" while not on the ground
        if (currentState == States.Airborne && IsGrounded)
        {
            currentState = States.Moving;
            currentVelocityY = 0;
            // Jump is buffered
            if (lastJumpPressed + jumpBuffer > Time.time)
            {
                Jump();
            }
            // Change animation

        }
        else if (currentState == States.Moving && !IsGrounded)
        {
            if (!InputHandler.Instance.isJumping)
            {
                StartCoroutine(StartCoyoteFrames());
            }
            currentState = States.Airborne;
        }
    }

    private void CalculateGravity()
    {
        float _gravityMultiplier = !flipGravity ? 1 : -1;

        if (IsGrounded)
        {
            // Move out of the ground
            //if (currentVelocityY != 0) currentVelocityY = 0;
        }
        else { 
            var _fallAcceleration = endedJumpEarly && currentVelocityY * _gravityMultiplier > 0 ? fallAcceleration * jumpEndEarlyGravityModifier : fallAcceleration;

            currentVelocityY -= _fallAcceleration * Time.deltaTime * _gravityMultiplier;

            // Clamp
            if (Mathf.Abs(currentVelocityY) > Mathf.Abs(fallClamp)) currentVelocityY = -fallClamp * _gravityMultiplier;
        }

        if (hitHead)
        {
            if (currentVelocityY * _gravityMultiplier > 0) currentVelocityY = 0;
        }

        /*// Gravity is normal
        else if(!flipGravity)
        {
            // Add downward force while ascending if we ended the jump early
            var _fallAcceleration = endedJumpEarly && currentVelocityY > 0 ? fallAcceleration * jumpEndEarlyGravityModifier : fallAcceleration;

            // Fall
            currentVelocityY -= _fallAcceleration * Time.deltaTime;

            // Clamp
            if (Mathf.Abs(currentVelocityY) > Mathf.Abs(fallClamp)) currentVelocityY = -fallClamp;
        }
        // Gravity is flipped
        else
        {
            // Add downward force while ascending if we ended the jump early
            var _fallAcceleration = endedJumpEarly && currentVelocityY < 0 ? fallAcceleration * jumpEndEarlyGravityModifier : fallAcceleration;

            // Fall
            currentVelocityY += _fallAcceleration * Time.deltaTime;

            // Clamp
            if (Mathf.Abs(currentVelocityY) > Mathf.Abs(fallClamp)) currentVelocityY = fallClamp;
        }*/
    }

    // Player has reduced gravity as they approach the apex of their jump
    private void CalculateJumpApex()
    {
        if (!IsGrounded)
        {
            apexPoint = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(rb.velocity.y));
            fallAcceleration = Mathf.Lerp(maxFallAcceleration, minFallAcceleration, apexPoint);
        }
        else
        {
            apexPoint = 0;
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed");
        lastJumpPressed = Time.time;
        if (IsGrounded || coyoteUsable)
        {
            Jump();
        }
    }

    public void JumpCanceled()
    {
        float _gravityMultiplier = !flipGravity ? 1 : -1;
        Debug.Log("Jump Canceled Early");
        // End the jump early if button released
        if (!IsGrounded && !endedJumpEarly && rb.velocity.y * _gravityMultiplier > 0)
        {
            Debug.Log("EndJump Early test");
            endedJumpEarly = true;
        }
    }

    private void Jump()
    {
        animationHandler.ChangeAnimationState("DuckJump");
        timeLeftGround = Time.time;
        endedJumpEarly = false;
        coyoteUsable = false;
        Debug.Log("JUMP!");
        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (!flipGravity)
        {
            currentVelocityY = jumpHeight;
        }
        else
        {
            currentVelocityY = -jumpHeight;
        }

    }

    private IEnumerator StartCoyoteFrames()
    {
        coyoteUsable = true;
        yield return new WaitForSeconds(coyoteTimeThreshold);
        coyoteUsable = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(headCheck.position, headCheckSize);
    }

    public void FlipGravity()
    {
        flipGravity = !flipGravity;
    }

}

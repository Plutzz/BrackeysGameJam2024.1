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
    private float gravityMultiplier;

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
        if(PlayerController.Instance.pushableBox != null)
        {
            PlayerController.Instance.pushableBox.SetSpeed(currentVelocityX);
        }
    }

    private void GravityCheck()
    {


        if (Input.GetKeyDown(KeyCode.G)) FlipGravity();

        gravityMultiplier = !flipGravity ? 1 : -1;

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
    }

    private void CollisionCheck()
    {
        var _groundCheck = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        // If it is a platform and player is moving up, ignore ground check
        if (_groundCheck != null)
        {
            // is a platform
            if (_groundCheck.GetComponent<PlatformEffector2D>() != null && currentVelocityY * gravityMultiplier > 0.1f)
            {
               
                IsGrounded = false;
            }
            else
            {
                IsGrounded = true;
            }
        }
        else
        {
            IsGrounded = false;
        }
        var _headCheck = Physics2D.OverlapBox(headCheck.position, headCheckSize, 0f, groundLayer);
        // If it is a platform, player does not hit head
        if (_headCheck != null && _headCheck.GetComponent<PlatformEffector2D>() == null)
        {
            hitHead = true;
        }
        else
        {
            hitHead = false;
        }
        
    }

    private void HandleStates()
    {
        // Player is "airborne" while not on the ground
        if (currentState == States.Airborne && IsGrounded)
        {
            currentState = States.Moving;
            
            currentVelocityY = 0;
            // Jump is buffered
            if (lastJumpPressed + jumpBuffer > Time.time)
            {
                animationHandler.ChangeAnimationState(animationHandler.DuckJumpState);
                currentState = States.Airborne;
                Jump();
            }

        }
        else if (currentState == States.Moving && !IsGrounded)
        {
            if (currentVelocityY < 0.1f * gravityMultiplier)
            {
                StartCoroutine(StartCoyoteFrames());
            }
            currentState = States.Airborne;
        }

        if (currentState == States.Moving && currentVelocityY < 0.1f)
        {
            if (XAxisInput != 0)
            {
                animationHandler.ChangeAnimationState(animationHandler.DuckWalkState);
            }
            else
            {
                animationHandler.ChangeAnimationState(animationHandler.DuckIdleState);
            }
        }
    }

    private void CalculateGravity()
    {

        if (IsGrounded)
        {
            // Move out of the ground
            //if (currentVelocityY != 0) currentVelocityY = 0;
        }
        else { 
            var _fallAcceleration = endedJumpEarly && currentVelocityY * gravityMultiplier > 0 ? fallAcceleration * jumpEndEarlyGravityModifier : fallAcceleration;

            currentVelocityY -= _fallAcceleration * Time.deltaTime * gravityMultiplier;

            // Clamp
            if (Mathf.Abs(currentVelocityY) > Mathf.Abs(fallClamp)) currentVelocityY = -fallClamp * gravityMultiplier;
        }

        if (hitHead)
        {
            if (currentVelocityY * gravityMultiplier > 0) currentVelocityY = 0;
        }

        
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
        lastJumpPressed = Time.time;
        if (IsGrounded || coyoteUsable)
        {
            Jump();
        }
    }

    public void JumpCanceled()
    {
        float _gravityMultiplier = !flipGravity ? 1 : -1;
        // End the jump early if button released
        if (!IsGrounded && !endedJumpEarly && rb.velocity.y * _gravityMultiplier > 0)
        {
            endedJumpEarly = true;
        }
    }

    public void TakeKnockBack(Vector2 _knockBackVector, float _knockUpAmount)
    {
        endedJumpEarly = true;

        currentVelocityX = _knockBackVector.x;
        currentVelocityY = _knockUpAmount;
    }

    private void Jump()
    {
        if (PlayerController.Instance.isPushingBox) return;

        animationHandler.ChangeAnimationState(animationHandler.DuckJumpState);
        timeLeftGround = Time.time;
        endedJumpEarly = false;
        coyoteUsable = false;
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

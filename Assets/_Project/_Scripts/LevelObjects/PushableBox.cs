using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Change raycast to player (idk why I added it here)
public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool gettingPushed;

    [Header("Player Check")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float playerRaycastOffset;
    public bool IsGrounded { get; private set; }

    [Header("Gravity Variables")]
    [SerializeField] private float fallAcceleration;
    [SerializeField] private float fallClamp;
    private float currentVelocityY;
    public bool flipGravity;
    private int gravityMultiplier;

    public float TimeScale = 1.0f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        GravityCheck();
        CalculateGravity();
        GroundCheck();

    }

    private void GroundCheck()
    {
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        if (PlayerController.Instance.isPushingBox)
        {
            if (!IsGrounded && PlayerController.Instance.pushableBox != null)
            {
                Debug.Log("Box Off The Ground");
                PlayerController.Instance.StopPushingBox();
            }
        }

        // When the box hits the ground, set it's body type to static
        if (!gettingPushed && rb.bodyType == RigidbodyType2D.Kinematic)
        {
            if (IsGrounded)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void GravityCheck()
    {
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

    public void FlipGravity()
    {
        flipGravity = !flipGravity;
    }

    private void CalculateGravity()
    {
        if(!IsGrounded && rb.bodyType == RigidbodyType2D.Kinematic)
        {
            currentVelocityY -= fallAcceleration * Time.deltaTime * gravityMultiplier;

            // Clamp
            if (Mathf.Abs(currentVelocityY) > Mathf.Abs(fallClamp))
            {
                currentVelocityY = -fallClamp * gravityMultiplier;
            }
            rb.velocity = new Vector2(rb.velocity.x, currentVelocityY);
        }
        else
        {
            currentVelocityY = 0f;
        }
    }

    //Velocity to be set by PlayerMovement Script (SetSpeed method)
    public void StartPush()
    {
        if (gettingPushed) return;

        gettingPushed = true;
        Debug.Log("Start Push");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void StopPush()
    {
        if (!gettingPushed) return;

        gettingPushed = false;
        Debug.Log("Stop Push");
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

    }

    public void SetSpeed(float _speed)
    {
        rb.velocity = new Vector2(_speed, rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

}

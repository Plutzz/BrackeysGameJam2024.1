using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public bool isFacingRight { get; private set; } = true;
    [Header("Directions")]
    [SerializeField] private CameraFollowObject cameraFollowObject;

    [Header("Gravity")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject fallingGameObject;

    private PlayerMovement playerMovement;
    private float XAxisInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        InputHandler.Instance.playerInputActions.Player.Jump.performed += playerMovement.JumpPressed;
    }

    private void Update()
    {
        ApplyInputs();
    }

    private void ApplyInputs() {
        playerMovement.ReceiveHorizontalInput(InputHandler.Instance.inputVector.x);
        if (InputHandler.Instance.playerInputActions.Player.Jump.WasReleasedThisFrame())
        {
            playerMovement.JumpCanceled();
        }
    }

    void FixedUpdate()
    {
        TurnCheck();
        DownwardCheck();
    }

    private void TurnCheck()
    {
        if (InputHandler.Instance.inputVector.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (InputHandler.Instance.inputVector.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, isFacingRight? 180 : 0, transform.rotation.z));
        isFacingRight = !isFacingRight;
        cameraFollowObject.CallFlip();
    }

    private void DownwardCheck() {
        fallingGameObject.SetActive(rb.velocity.y < 0);
    }

    
}

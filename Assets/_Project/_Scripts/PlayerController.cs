using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public bool isFacingRight { get; private set; } = true;
    [Header("Directions")]
    [SerializeField] private CameraFollowObject cameraFollowObject;

    [Header("Gravity")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject fallingGameObject;

    [Header ("Specials")]
    private PlayerSpecialHandler specialHandler;
    [SerializeField] private float specialMaxHoldTime = 1f;
    [SerializeField] private GameObject dummyAnimator;

    [Header("Ability Checks")]
    public bool hasDoor;

    [Header("Interactable")]
    [SerializeField] private float startOffset = -.5f;
    [SerializeField] private float rayLength = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        specialHandler = GetComponent<PlayerSpecialHandler>();
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
        if (InputHandler.Instance.playerInputActions.Player.Special.WasPerformedThisFrame()) {
            PausePlayerMovement();
        }
        if (InputHandler.Instance.playerInputActions.Player.Special.WasReleasedThisFrame() ) {
            if (InputHandler.Instance.chargePressTime + specialMaxHoldTime > Time.time) {
                specialHandler.UseSpecial();
                StartCoroutine(ReplaceAnimator(gameObject));
            }
            ResumePlayerMovement();
        }
        if (InputHandler.Instance.playerInputActions.Player.Interact.WasPerformedThisFrame()) {
            TryInteract();
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
        if (playerMovement.enabled == false) {
            return;
        }
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, isFacingRight? 180 : 0, transform.rotation.z));
        isFacingRight = !isFacingRight;
        cameraFollowObject.CallFlip();
    }

    private void DownwardCheck() {
        fallingGameObject.SetActive(rb.velocity.y < 0);
    }

    
    private void PausePlayerMovement() { 
        playerMovement.enabled = false;
        rb.velocity = Vector3.zero;
    }

    private void ResumePlayerMovement() {
        playerMovement.enabled = true;
    }

    IEnumerator ReplaceAnimator(GameObject user)
    {
        Debug.Log("Trying to replace animator");
        Animator animator = user.GetComponentInChildren<Animator>();

        RuntimeAnimatorController originalController = animator.runtimeAnimatorController;

        animator.runtimeAnimatorController = dummyAnimator.GetComponent<Animator>().runtimeAnimatorController;

        yield return new WaitForSeconds(3);

        animator.runtimeAnimatorController = originalController;

    }

    private void TryInteract() {
        //start ray behind to slightly ahead
        //checking for anything on interactable layer
        Debug.Log("Trying to interact");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * startOffset, transform.right, rayLength, interactableLayer);
        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) { interactable.Interact(); }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerMovement playerMovement;
    public bool isFacingRight { get; private set; } = true;
    [Header("Directions")]
    [SerializeField] private CameraFollowObject cameraFollowObject;

    [Header("Gravity")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject fallingGameObject;

    [Header("Ability Checks")]
    public bool hasDoor;

    [Header("Interactable")]
    [SerializeField] private float startOffset = -.5f;
    [SerializeField] private float rayLength = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Door")]
    [SerializeField] private LayerMask doorLayer;
    private Door door;
    public bool isPushingBox { get; private set; }
    public PushableBox pushableBox;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        InputHandler.Instance.playerInputActions.Player.Jump.performed += playerMovement.JumpPressed;
        InputHandler.Instance.playerInputActions.Player.Door.performed += DoorCheck;
        LevelManager.Instance.reloadingScene += UnsubscribeFromEvents;
    }

    private void UnsubscribeFromEvents()
    {
        InputHandler.Instance.playerInputActions.Player.Jump.performed -= playerMovement.JumpPressed;
        InputHandler.Instance.playerInputActions.Player.Door.performed -= DoorCheck;
        LevelManager.Instance.reloadingScene -= UnsubscribeFromEvents;
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
        if (InputHandler.Instance.playerInputActions.Player.Special.WasPerformedThisFrame()) 
        {
            PausePlayerMovement();
        }
        if (InputHandler.Instance.playerInputActions.Player.Interact.WasPerformedThisFrame()) 
        {
            TryInteract();
        }
        if(hasDoor && InputHandler.Instance.playerInputActions.Player.Door.WasReleasedThisFrame())
        {
            PutDownDoor();
        }
        if (isPushingBox && InputHandler.Instance.playerInputActions.Player.Push.WasReleasedThisFrame())
        {
            StopPushingBox();
        }
        if (Input.GetKeyDown(KeyCode.R)) { 
            LevelManager.Instance.ResetLevel();
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
        if (playerMovement.enabled == false || isPushingBox) {
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

    #region Door Methods
    private void DoorCheck(InputAction.CallbackContext context)
    {
        //start ray behind to slightly ahead
        //checking for anything on door layer
        //can be optimsed with layer mask
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * startOffset, transform.right, rayLength, interactableLayer);
        if (hit.collider != null)
        {
            door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                if(!isPushingBox)
                {
                    PickUpDoor();
                }
            }
        }
    }
    private void PickUpDoor()
    {
        door.PickUp();
        door.transform.position = transform.position + (Vector3.up * 0.5f);
        door.transform.parent = transform;
        hasDoor = true;
    }
    private void PutDownDoor()
    {
        hasDoor = false;
        door.PutDown();
        door.transform.parent = null;
        door = null;
    }

    public void PushBox()
    {
        isPushingBox = true;
    }
    public void StopPushingBox()
    {
        isPushingBox = false;
        if(pushableBox != null)
        {
            pushableBox.StopPush();
        }
        pushableBox = null;
    }

    #endregion
}

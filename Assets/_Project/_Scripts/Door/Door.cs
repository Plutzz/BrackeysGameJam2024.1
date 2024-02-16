using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private Collider2D col;
    private Rigidbody2D rb;
    public bool isOpen { get; private set; }
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject openDoorObj;
    [SerializeField] private GameObject closeDoorObj;
    [SerializeField] private GameObject platformCollider;
    [SerializeField] private GameObject groundCollider;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(PlayerController.Instance.isPushingDoor)
        {
            bool _groundCheck = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
            if(!_groundCheck)
            {
                PlayerController.Instance.StopPushingDoor();
            }
        }

    }

    public void PickUp()
    {
        if(isOpen)
        {
            CloseDoor();
        }
        col.enabled = false;
        platformCollider.SetActive(false);
        graphics.SetActive(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void PutDown()
    {
        col.enabled = true;
        platformCollider.SetActive(true);
        graphics.SetActive(true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OpenDoor()
    {
        isOpen = true;
        platformCollider.SetActive(false);
        openDoorObj.SetActive(true);
        closeDoorObj.SetActive(false);
    }

    public void CloseDoor()
    {
        isOpen = false;
        platformCollider.SetActive(true);
        openDoorObj.SetActive(false);
        closeDoorObj.SetActive(true);
    }

    public override void Interact()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void PushDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        PlayerController.Instance.PushDoor(this);
        platformCollider.SetActive(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void StopPushingDoor()
    {
        platformCollider.SetActive(true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(InputHandler.Instance.playerInputActions.Player.Push.IsPressed())
        {
            PushDoor();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}

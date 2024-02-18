using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private Collider2D col;
    private Rigidbody2D rb;
    public bool isOpen { get; private set; }
    public bool isPickedUp { get; private set; }
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject openDoorObj;
    [SerializeField] private GameObject closeDoorObj;
    [SerializeField] private GameObject platformCollider;

    private bool isGravityFlipped;
    private int gravityMultiplier;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GravityCheck();
    }

    public void PickUp()
    {
        if(isOpen)
        {
            CloseDoor();
        }
        isPickedUp = true;
        col.enabled = false;
        platformCollider.SetActive(false);
        graphics.SetActive(false);
        isGravityFlipped = PlayerController.Instance.GetComponent<PlayerMovement>().IsGravityFlipped;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void PutDown()
    {
        isPickedUp = false;
        col.enabled = true;
        platformCollider.SetActive(true);
        graphics.SetActive(true);
        ResetPosition();
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

    private void GravityCheck()
    {
        gravityMultiplier = !isGravityFlipped ? 1 : -1;

        if (!isGravityFlipped)
        {
            transform.localScale = new Vector2(1, 1);
            rb.gravityScale = 1;

        }
        else
        {
            transform.localScale = new Vector2(1, -1);
            rb.gravityScale = -1;
        }
    }

    public void FlipGravity()
    {
        isGravityFlipped = !isGravityFlipped;
    }

    public void ResetPosition()
    {
        Vector3 _position = PlayerController.Instance.transform.position;
        if(!isGravityFlipped)
        {
            _position.y += 0.5f;
        }
        else
        {
            _position.y -= 0.5f;
        }
        transform.position = _position;
    }
}

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


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private Rigidbody2D rb;
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
}

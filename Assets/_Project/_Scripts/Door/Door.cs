using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject openDoorObj;
    [SerializeField] private GameObject closeDoorObj;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void PickUp()
    {
        col.enabled = false;
        graphics.SetActive(false);
    }

    public void PutDown()
    {
        col.enabled = true;
        graphics.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLevelObject : MonoBehaviour
{
    public bool hidden;
    [SerializeField] protected GameObject hiddenObject;
    [SerializeField] private float startOffset = -0.5f;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask interactableLayer;
    private Door door;
    protected virtual void Reveal()
    {
        if (!hidden) return;
        hidden = false;
        if(hiddenObject != null) { hiddenObject.SetActive(true); }
    }

    protected virtual void Hide()
    {
        if (hidden) return;
        hidden = true;
        if (hiddenObject != null) { hiddenObject.SetActive(false); }
    }

    protected void Update()
    {
        if(hidden)
        {
            Debug.DrawRay(transform.position + transform.right * startOffset, transform.right * rayLength, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * startOffset, transform.right, rayLength, interactableLayer);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
                door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    Debug.Log("Hit Door");
                    if (door.isOpen)
                    {
                        Reveal();
                    }
                }
            }
        }
        else if(door.isPickedUp)
        {
            door = null;
            Hide();
        }
    }

}

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
    private SpriteRenderer riftSprite;
    private void Awake()
    {
        riftSprite = GetComponent<SpriteRenderer>();
    }
    protected virtual void Reveal()
    {
        if (!hidden) return;
        riftSprite.enabled = false;
        hidden = false;
        if(hiddenObject != null) { hiddenObject.SetActive(true); }
    }

    protected virtual void Hide()
    {
        if (hidden) return;
        riftSprite.enabled = true;
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
                door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
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

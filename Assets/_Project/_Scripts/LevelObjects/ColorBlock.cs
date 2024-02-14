using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : MonoBehaviour
{
    public ColorBlockManager.BlockColors blockColor;
    private Collider2D col;
    private SpriteRenderer ren;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        ren = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ColorBlockManager.Instance.blockList.Add(this);
    }

    public void ActivateBlock()
    {
        col.enabled = true;
        ren.enabled = true;
    }

    public void DeactivateBlock()
    {
        col.enabled = false;
        ren.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu]
public class ColorBlock : Tile
{
    public ColorBlockManager.BlockColors blockColor;
    private Collider2D col;

    private void Awake()
    {
        col = Instantiate(new BoxCollider2D());
        GetColor(blockColor);
    }

    private void Start()
    {
        ColorBlockManager.Instance.blockList.Add(this);
    }

    public void ActivateBlock()
    {
        col.enabled = true;
    }

    public void DeactivateBlock()
    {
        col.enabled = false;
    }

    private void GetColor(ColorBlockManager.BlockColors blockColor)
    {
        switch(blockColor)
        {
            case ColorBlockManager.BlockColors.Red:
                color = Color.red;
                break;
            case ColorBlockManager.BlockColors.Blue:
                color = Color.blue;
                break;
            case ColorBlockManager.BlockColors.Yellow:
                color = Color.yellow;
                break;
            case ColorBlockManager.BlockColors.Green:
                color = Color.green;
                break;
        }
    }
}

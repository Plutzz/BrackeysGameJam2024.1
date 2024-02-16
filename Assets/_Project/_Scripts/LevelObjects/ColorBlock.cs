using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu]
public class ColorBlock : MonoBehaviour
{
    public ColorBlockManager.BlockColors blockColor;
    private Color color;
    private Collider2D col;
    private SpriteRenderer ren;
    [SerializeField] private Sprite EnabledBlockGraphics;
    [SerializeField] private Sprite DisabledBlockGraphics;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        ren = GetComponent<SpriteRenderer>();
        GetColor();
        SetColor();
    }
    private void Start()
    {
        ColorBlockManager.Instance.blockList.Add(this);
    }

    public void ActivateBlock()
    {
        col.enabled = true;
        ren.sprite = EnabledBlockGraphics;
    }

    public void DeactivateBlock()
    {
        col.enabled = false;
        ren.sprite = DisabledBlockGraphics;
    }

    private void GetColor()
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
    private void SetColor()
    {
        if(color != null)
        {
            ren.color = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlockManager : Singleton<ColorBlockManager>
{
    public List<ColorBlock> blockList;
    public BlockColors currentColor = BlockColors.Blue;
    public int numberOfColors;

    public float timePerColorSwitch = 2f;
    private float timeOfLastSwap;

    private bool initialSwitch = false;

    public enum BlockColors
    {
        Blue,
        Red,
        Yellow,
        Green,
    }

    protected override void Awake()
    {
        base.Awake();
        blockList = new List<ColorBlock>();
    }

    private void Update()
    {
        if(!initialSwitch)
        {
            SwitchColors();
            initialSwitch = true;
        }

        if(Time.time > timeOfLastSwap + timePerColorSwitch)
        {
            timeOfLastSwap = Time.time;
            SwitchColors();
        }
    }
    private void SwitchColors()
    {
        currentColor = currentColor + 1;
        if((int)currentColor >= numberOfColors)
        {
            currentColor = 0;
        }


        // Activate all of the blocks that are the currentColor
        // Disable all others
        foreach(var block in blockList)
        {
            if(block.blockColor == currentColor)
            {
                block.ActivateBlock();
            }
            else
            {
                block.DeactivateBlock();
            }
        }

    }


}

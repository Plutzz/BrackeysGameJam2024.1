using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenEnemy : HiddenLevelObject
{
    protected override void Reveal()
    {
        if (!hidden) return;
        hidden = false;
        if (hiddenObject != null) { hiddenObject.SetActive(true); }
        hiddenObject = null;
    }
}

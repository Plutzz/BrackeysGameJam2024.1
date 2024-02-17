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
        StartCoroutine(gracePeriod(1f));
        hiddenObject = null;
    }

    private IEnumerator gracePeriod(float seconds)
    {
        GameObject _hiddenObject = hiddenObject;
        _hiddenObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(seconds);
        _hiddenObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}

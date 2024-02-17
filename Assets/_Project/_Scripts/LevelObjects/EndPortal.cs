using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : Interactable
{
    [SerializeField] private string sceneName;
    public override void Interact()
    {
        LevelManager.Instance.LoadScene(sceneName);
    }
}

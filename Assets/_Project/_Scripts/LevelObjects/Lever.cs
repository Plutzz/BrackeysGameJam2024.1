using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] private UnityEvent onEvent;
    [SerializeField] private UnityEvent offEvent;

    private bool leverOn;
    public override void Interact()
    {
        leverOn = !leverOn;
        if (leverOn)
        {
            onEvent?.Invoke();
        }
        else {
            offEvent?.Invoke();
        }
        
    }

}
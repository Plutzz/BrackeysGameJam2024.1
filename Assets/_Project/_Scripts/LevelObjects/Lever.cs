using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    //Animations
    private Animator anim;
    private string currentState;
    private bool isActivated;

    [SerializeField] private Activatable[] activatables;


    public override void Interact()
    {
        if(isActivated)
        {
            LeverOff();
        }
        else
        {
            LeverOn();
        }

        AudioManager.Instance.PlaySound(AudioManager.Sounds.Lever);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void LeverOn()
    {
        Debug.Log("Lever Down");
        ChangeAnimationState("LeverOn");
        isActivated = true;
        foreach(var activatable in activatables)
        {
            activatable.Activate();
        }

    }
    private void LeverOff()
    {
        ChangeAnimationState("LeverOff");
        isActivated = false;
        foreach (var activatable in activatables)
        {
            activatable.Deactivate();
        }
    }
    private void ChangeAnimationState(string newState)
    {

        // Ensures state change is only triggered once
        if (currentState == newState) return;

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

}
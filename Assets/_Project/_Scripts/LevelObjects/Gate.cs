using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Activatable
{
    private Animator anim;
    private string currentState;
    private Collider2D gateCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        gateCollider = GetComponent<Collider2D>();
    }
    private void OpenGate()
    {
        ChangeAnimationState("GateOpen");
        gateCollider.enabled = false;
    }

    private void CloseGate()
    {
        ChangeAnimationState("GateClose");
        gateCollider.enabled = true;
    }

    private void ChangeAnimationState(string newState)
    {

        // Ensures state change is only triggered once
        if (currentState == newState) return;

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    public override void Activate()
    {
        OpenGate();
    }

    public override void Deactivate()
    {
        CloseGate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private StageButton button;
    private Animator anim;
    private string currentState;
    private Collider2D gateCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        gateCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        button.onButtonDown += OpenGate;
        button.onButtonUp += CloseGate;
    }

    public void OpenGate()
    {
        ChangeAnimationState("GateOpen");
        gateCollider.enabled = false;
    }

    public void CloseGate()
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

}

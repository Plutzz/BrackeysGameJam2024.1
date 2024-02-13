using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator anim;
    private string currentState;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void ChangeAnimationState(string newState)
    {
        // Ensures state change is only triggered once
        if(currentState == newState) return;

        Debug.Log("CHANGE ANIM STATE " + newState);

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    public void RepeatAnimationState()
    {
        anim.Play(currentState);
    }

    public void PlayJumpAnimation()
    {
        anim.Play("DuckJump");
    }
}

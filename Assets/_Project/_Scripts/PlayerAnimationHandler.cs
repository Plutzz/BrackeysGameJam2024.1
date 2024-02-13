using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator anim;
    private PlayerController playerController;
    private string currentState;

    public string DuckWalkState { get; private set; }
    public string DuckIdleState { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponentInChildren<PlayerController>();
    }

    public void ChangeAnimationState(string newState)
    {
        if (playerController.hasDoor)
        {
            DuckWalkState = "DuckWalk";
            DuckIdleState = "DuckIdle";
        }
        else
        {
            DuckWalkState = "DuckWalk_NoDoor";
            DuckIdleState = "DuckIdle_NoDoor";
        }

        // Ensures state change is only triggered once
        if(currentState == newState) return;

        Debug.Log("CHANGE ANIM STATE " + newState);

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }
}

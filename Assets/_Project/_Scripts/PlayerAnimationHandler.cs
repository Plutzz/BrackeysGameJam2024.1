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
    public string DuckJumpState { get; private set; }

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
            DuckJumpState = "DuckJump";
        }
        else
        {
            DuckWalkState = "DuckWalk_NoDoor";
            DuckIdleState = "DuckIdle_NoDoor";
            DuckJumpState = "DuckJump_NoDoor";
            if (playerController.isPushingDoor)
            {
                DuckWalkState = "DuckWalk_Push";
                DuckIdleState = "DuckIdle_Push";
            }
        }

        // Ensures state change is only triggered once
        if(currentState == newState) return;

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }
}

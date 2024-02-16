using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //Animations
    private Animator anim;
    private string currentState;
    private bool buttonReady;

    [SerializeField] private Activatable[] activatables;
    [SerializeField] private Vector2 buttonHitboxSize;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var collision = Physics2D.Raycast(transform.position - new Vector3(transform.localScale.x * 0.5f, 0), transform.right, transform.localScale.x);
        if (collision && (collision.rigidbody.gameObject.CompareTag("Player") || collision.rigidbody.gameObject.CompareTag("ButtonPressable")))
        {
            ButtonDown();
        }
        else
        {
            ButtonUp();
        }
    }
    private void ButtonDown()
    {
        if (!buttonReady) return;
        buttonReady = false;
        Debug.Log("ButtonDown");
        ChangeAnimationState("ButtonDown");

        foreach(var activatable in activatables)
        {
            activatable.Activate();
        }

    }
    private void ButtonUp()
    {
        if (buttonReady) return;
        buttonReady = true;
        ChangeAnimationState("ButtonUp");
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

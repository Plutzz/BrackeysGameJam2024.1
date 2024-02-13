using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    //Animations
    private Animator anim;
    private string currentState;

    //Events
    public event Action onButtonDown;
    public event Action onButtonUp;


    private void ButtonDown()
    {
        Debug.Log("ButtonDown");
        ChangeAnimationState("ButtonDown");
        if (onButtonDown != null)
        {
            onButtonDown();
        }
    }
    private void ButtonUp()
    {
        ChangeAnimationState("ButtonUp");
        if (onButtonUp != null)
        {
            onButtonUp();
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void ChangeAnimationState(string newState)
    {

        // Ensures state change is only triggered once
        if (currentState == newState) return;

        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ButtonDown();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ButtonUp();
        }
    }


}

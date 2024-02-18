using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Button : MonoBehaviour
{
    //Animations
    private Animator anim;
    private string currentState;
    private bool buttonReady;
    private int numberOfCollisions;

    [SerializeField] private Activatable[] activatables;
    [SerializeField] private Vector2 buttonHitboxSize;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        buttonReady = true;
    }
    private void ButtonDown()
    {
        if (!buttonReady) return;

        AudioManager.Instance.PlaySound(AudioManager.Sounds.Lever);
        buttonReady = false;
        Debug.Log("ButtonDown");
        ChangeAnimationState("ButtonDown");
        if(activatables.Length > 0)
        {
            foreach (var activatable in activatables)
            {
                activatable.Activate();
            }
        }
    }
    protected virtual void ButtonUp()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ButtonPressable"))
        {
            numberOfCollisions++;
            ButtonDown();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("ButtonPressable"))
        {
            numberOfCollisions--;
            if(numberOfCollisions <= 0)
            {
                numberOfCollisions = 0;
                ButtonUp();
            }
        }
    }
}

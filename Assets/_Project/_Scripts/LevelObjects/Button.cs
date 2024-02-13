using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Interactable
{
    [SerializeField] private UnityEvent pressEvent;
    [SerializeField] private UnityEvent releaseEvent;

    [SerializeField] private float releaseTimer;
    private float timer;

    public override void Interact() { 
        pressEvent?.Invoke();
        timer = .01f;
    }

    private void Update()
    {
        if (timer > 0) {
            timer += Time.deltaTime;
            if (timer > releaseTimer) { 
                
                releaseEvent?.Invoke();

                timer = 0;
            }
        }
    }
}

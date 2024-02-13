using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent pressEvent;
    [SerializeField] private UnityEvent releaseEvent;

    [SerializeField] private float requiredWeight;
    private float currentWeight;
    private bool currentlyActive;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null) {
            currentWeight += rb.mass;
            CheckWeight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentWeight -= rb.mass;
            CheckWeight();
        }
    }

    private void CheckWeight() {
        if (currentWeight > requiredWeight == currentlyActive) {
            //Already correct state
            return;
        }

        currentlyActive = currentWeight > requiredWeight;

        if (currentlyActive)
        {
            pressEvent?.Invoke();
        }
        else { 
            releaseEvent?.Invoke();
        }
    }
}

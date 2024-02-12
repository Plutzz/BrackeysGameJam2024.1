using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Triggered");
        if (collision.gameObject != gameObject && collision.gameObject != transform.parent.gameObject)
        {
            UnitHealth health = collision.GetComponent<UnitHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
        }
    }
}
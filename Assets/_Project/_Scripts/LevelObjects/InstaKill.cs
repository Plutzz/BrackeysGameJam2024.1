using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitHealth health = collision.gameObject.GetComponent<UnitHealth>();
        if (health != null) { 
            health.Die();
        }
    }
}

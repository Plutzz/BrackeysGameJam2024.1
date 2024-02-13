using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitHealth health = collision.gameObject.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.TakeDamage(damage, transform.position);
        }
    }
}

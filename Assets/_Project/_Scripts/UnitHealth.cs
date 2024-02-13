using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private float health;
    private float maxHealth;
    [SerializeField] private float defaultKnockbackForce;
    private void Awake()
    {
        maxHealth = health;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) {
            Die();
        }
        health = Mathf.Min(health, maxHealth);
    }

    //Currently doesn't apply knockback due to scripts setting velocity directly
    public virtual void TakeDamage(float damage, Vector3 damageOrigin) { 
        TakeDamage(damage);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            
            Vector3 forceDirection = (transform.position - damageOrigin).normalized;
            rb.AddForce(forceDirection * defaultKnockbackForce, ForceMode2D.Impulse);

        }
    }

    public virtual void Die() { 
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] protected float health;
    protected float maxHealth;
    [SerializeField] protected float defaultKnockbackForce;
    [SerializeField] protected AudioManager.Sounds takeDamageSound;
    [SerializeField] protected AudioManager.Sounds deathSound;
    protected virtual void Awake()
    {
        maxHealth = health;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log("take " + damage + " damage");
        AudioManager.Instance.PlaySound(takeDamageSound);

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

            //Debug.Log("Take Knockback in this direction: " + forceDirection);

            rb.velocity = defaultKnockbackForce * forceDirection;

            //rb.AddForce(forceDirection * defaultKnockbackForce, ForceMode2D.Impulse);
        }
    }

    public virtual void Die() {
        AudioManager.Instance.PlaySound(deathSound);
        Destroy(gameObject);
    }
}

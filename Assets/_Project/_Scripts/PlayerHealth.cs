using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : UnitHealth
{
    [SerializeField] private float knockUpAmount;

    [SerializeField] private float invincibilityTime;
    [SerializeField] private int invincibilityBlinks;
    [SerializeField] private SpriteRenderer graphics;

    private bool canTakeDamage;

    protected override void Awake()
    {
        base.Awake();
        canTakeDamage = true;
    }

    public override void TakeDamage(float damage)
    {
        // if player can't take damage return
        if (!canTakeDamage) return;

        base.TakeDamage(damage);

        StartCoroutine(Invincibility());
    }

    public override void TakeDamage(float damage, Vector3 damageOrigin)
    {
        if (!canTakeDamage) return;

        TakeDamage(damage);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {

            Vector3 forceDirection = (transform.position - damageOrigin).normalized;

            Debug.Log("Take Knockback in this direction: " + forceDirection);

            GetComponent<PlayerMovement>().TakeKnockBack(defaultKnockbackForce * forceDirection, knockUpAmount);
            //rb.AddForce(forceDirection * defaultKnockbackForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Invincibility()
    {
        canTakeDamage = false;

        for (int i = 0; i < invincibilityBlinks; i++)
        {
            yield return new WaitForSeconds(invincibilityTime / (invincibilityBlinks * 2));
            graphics.enabled = false;
            yield return new WaitForSeconds(invincibilityTime / (invincibilityBlinks * 2));
            graphics.enabled = true;
        }

        canTakeDamage = true;
    }
}

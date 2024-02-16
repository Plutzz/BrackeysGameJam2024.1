using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : UnitHealth
{
    [SerializeField] private float knockUpAmount;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime;
    [SerializeField] private int invincibilityBlinks;
    [SerializeField] private SpriteRenderer graphics;
    private bool canTakeDamage;

    [Header("TimeStop")]
    [SerializeField] private float hitStopTimeScale = 0.05f;        //How slow game slows down to when hit
    [SerializeField] private float hitStopRestoreSpeed = 10f;       //Speed at which time scale is restored
    [SerializeField] private float hitStopDelay = 0.1f;             //How much time before time scales begins to restore
    private float timeRestoreSpeed;
    private bool restoreTime;



    protected override void Awake()
    {
        base.Awake();
        canTakeDamage = true;
    }

    private void Update()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * timeRestoreSpeed;
            }
            else
            {
                //playerMovement.enabled = true;
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        // if player can't take damage return
        if (!canTakeDamage) return;

        base.TakeDamage(damage);

        if(health > 0)
        {
            StopTime(hitStopTimeScale, hitStopRestoreSpeed, hitStopDelay);
        }

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

    public override void Die()
    {
        base.Die();

        LevelManager.Instance.ScenicResetLevel();
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

    private void StopTime(float _changeTimeScale, float _restoreSpeed, float _delay)
    {
        timeRestoreSpeed = _restoreSpeed;

        if (_delay > 0)
        {
            //playerMovement.enabled = false;
            StopCoroutine(StartTime(_delay));
            StartCoroutine(StartTime(_delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = _changeTimeScale;
    }

    private IEnumerator StartTime(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        restoreTime = true;
    }
}

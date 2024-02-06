using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    public GameObject user;
    public float damage;
    public float appearTime;
    public bool destroyOnHide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != user) { 
            UnitHealth collisionHealth = collision.GetComponent<UnitHealth>();
            if (collisionHealth != null)
            {
                collisionHealth.TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }   
    }

    private void OnEnable()
    {
        Invoke("DisableObject", appearTime);
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (destroyOnHide) { 
            Destroy(gameObject);
        }
    }

}

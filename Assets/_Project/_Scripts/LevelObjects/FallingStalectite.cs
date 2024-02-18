using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStalectite : MonoBehaviour
{
    [SerializeField] private float damage = 1;

    private void Awake()
    {
        Debug.Log("Dropping");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitHealth health = collision.gameObject.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.TakeDamage(damage, transform.position);
        }

        Destroy(gameObject);
        AudioManager.Instance?.PlaySound(AudioManager.Sounds.Stalactite);
    }
}

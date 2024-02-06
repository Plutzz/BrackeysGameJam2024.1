using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            PerformMeleeAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            PerformProjectileAttack();
        }
    }

    private void PerformMeleeAttack() { 
        meleeAttack.SetActive(true);
        meleeAttack.transform.position = transform.position + VectorToMousePosition().normalized;
    }

    private void PerformProjectileAttack()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position + VectorToMousePosition().normalized, Quaternion.identity);
        Rigidbody2D projectileRB = newProjectile.GetComponent<Rigidbody2D>();
        
        newProjectile.SetActive(true);
        projectileRB.velocity = VectorToMousePosition().normalized * projectileSpeed;
    }

    private Vector3 VectorToMousePosition() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return worldPosition - transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWanderingController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    [Header ("Physics checks")]
    [SerializeField] private Transform frontWallCheck;
    [SerializeField] private Transform frontFloorCheck;
    [SerializeField] private Transform normalFloorCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckFlip();
        rb.velocity = transform.right * moveSpeed;
    }

    private void CheckFlip()
    {
        RaycastHit2D hit = Physics2D.Raycast(frontWallCheck.position, transform.right, rayLength, groundLayer);
        if (hit.collider != null) {
            Flip();
        }
        RaycastHit2D frontFloorHit = Physics2D.Raycast(frontFloorCheck.position, Vector2.down, rayLength, groundLayer);
        hit = Physics2D.Raycast(normalFloorCheck.position, Vector2.down, rayLength, groundLayer);
        if (frontFloorHit.collider == null && hit.collider != null)
        {
            Flip();
        }

    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, facingRight ? 180 : 0, transform.rotation.z));
        facingRight = !facingRight;

        //rb.velocity = transform.right * moveSpeed;
    }


    void OnDrawGizmos()
    {
        // Visualize the raycast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(frontWallCheck.position, frontWallCheck.position + transform.right * rayLength);
        Gizmos.DrawLine(frontFloorCheck.position, frontFloorCheck.position + Vector3.down * rayLength);
        Gizmos.DrawLine(normalFloorCheck.position, normalFloorCheck.position + Vector3.down * rayLength);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { 

            UnitHealth health = collision.GetComponent<UnitHealth>();
            if (health != null) {
                health.TakeDamage(1);
            }
        }
    }

}

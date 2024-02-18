using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2DController))]
public class EnemyWanderingController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2DController rbController;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float acceleration;

    [Header ("Physics checks")]
    [SerializeField] protected Transform frontWallCheck;
    [SerializeField] protected Transform frontFloorCheck;
    [SerializeField] protected Transform normalFloorCheck;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected float rayLength;

    private bool facingRight = true;

    void Start()
    {
        rbController = GetComponent<Rigidbody2DController>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckFlip();
        rbController.SetTargetXVelocity(transform.right.x * moveSpeed, acceleration);
    }

    protected virtual void CheckFlip()
    {
        RaycastHit2D hit = Physics2D.Raycast(frontWallCheck.position, transform.right, rayLength, groundLayer);
        if (hit.collider != null) {
            Flip();
            return;
        }
        RaycastHit2D frontFloorHit = Physics2D.Raycast(frontFloorCheck.position, Vector2.down, rayLength, groundLayer);
        hit = Physics2D.Raycast(normalFloorCheck.position, Vector2.down, rayLength, groundLayer);
        if (frontFloorHit.collider == null && hit.collider != null)
        {
            Flip();
        }

    }

    protected virtual void Flip()
    {
        rbController.ForceSetVelocity(Vector3.zero);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, facingRight ? 180 : 0, transform.rotation.z));
        facingRight = !facingRight;

        //rb.velocity = transform.right * moveSpeed;
    }


    protected virtual void OnDrawGizmos()
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

            AudioManager.Instance.PlaySound(AudioManager.Sounds.Door_Gorg);
        }
    }

}

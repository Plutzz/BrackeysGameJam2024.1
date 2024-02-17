using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class JumpingEnemyBehavior : EnemyWanderingController
{

    [SerializeField] protected Transform jumpCheck;
    [SerializeField] protected float jumpRayDistance;

    [SerializeField] protected float jumpVelocity;

    [SerializeField] protected Transform edgeCheck;
    [SerializeField] protected float edgeRayDistance;
    [SerializeField] protected Vector2 edgeHopVelocity;

    private bool canJump = true;


    protected override void CheckFlip()
    {
        RaycastHit2D floorHit = Physics2D.Raycast(normalFloorCheck.position, Vector2.down, rayLength, groundLayer);
        if (floorHit.collider != null) { canJump = true; }

        if (!canJump)
        {
            return;
        }
        RaycastHit2D wallHit = Physics2D.Raycast(frontWallCheck.position, transform.right, rayLength, groundLayer | obstacleLayer);
        if (wallHit.collider != null)
        {
            Debug.Log("Hit a wall");
            RaycastHit2D hit = Physics2D.Raycast(frontWallCheck.position, jumpCheck.position - frontWallCheck.position, (jumpCheck.position - frontWallCheck.position).magnitude, groundLayer);
            if (hit.collider == null)
            {
                Debug.Log("Nothing above");
                hit = Physics2D.Raycast(jumpCheck.position, transform.right, jumpRayDistance, groundLayer);
                if (hit.collider == null)
                {
                    Debug.Log("Can Jump up");
                    rbController.AddVelocity(transform.up * jumpVelocity);
                    canJump = false;
                    return;
                }
            }

            Flip();
            return;
        }
        RaycastHit2D frontFloorHit = Physics2D.Raycast(frontFloorCheck.position, Vector2.down, rayLength, groundLayer);

        if (frontFloorHit.collider == null && floorHit.collider != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(frontWallCheck.position, edgeCheck.position - frontWallCheck.position, (edgeCheck.position - frontWallCheck.position).magnitude, groundLayer);
            if (hit.collider == null)
            {
                Debug.Log("Nothing infront");
                hit = Physics2D.Raycast(edgeCheck.position, transform.up * edgeRayDistance * -1, edgeRayDistance, groundLayer);
                RaycastHit2D obstacleHit = Physics2D.Raycast(edgeCheck.position, transform.up * edgeRayDistance * -1, edgeRayDistance, obstacleLayer);
                if (hit.collider != null && obstacleHit.collider == null)
                {
                    Debug.Log("Can Jump down");
                    rbController.AddVelocity(new Vector2(transform.right.x * edgeHopVelocity.x, edgeHopVelocity.y));
                    canJump = false;
                    return;
                }
            }


            Flip();
        }

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(frontWallCheck.position, jumpCheck.position);
        Gizmos.DrawLine(jumpCheck.position, jumpCheck.position + transform.right * jumpRayDistance);

        Gizmos.DrawLine(frontFloorCheck.position, edgeCheck.position);
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position - transform.up * edgeRayDistance);

    }
}
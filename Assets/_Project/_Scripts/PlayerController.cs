using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isFacingRight { get; private set; } = true;
    [SerializeField] private CameraFollowObject cameraFollowObject;

    private Rigidbody2D rb;
    [SerializeField] private Collider2D footCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        TurnCheck();
        DownwardCheck();
    }

    private void TurnCheck()
    {
        if (InputHandler.Instance.inputVector.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (InputHandler.Instance.inputVector.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, isFacingRight? 180 : 0, transform.rotation.z));
        isFacingRight = !isFacingRight;
        cameraFollowObject.CallFlip();
    }

    private void DownwardCheck() { 
        footCollider.enabled = (rb.velocity.y < 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != gameObject)
        {
            UnitHealth health = collision.GetComponent<UnitHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
        }
    }
}

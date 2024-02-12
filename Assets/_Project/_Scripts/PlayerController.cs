using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isFacingRight { get; private set; } = true;
    [SerializeField] private CameraFollowObject cameraFollowObject;
    void Start()
    {
    }

    void FixedUpdate()
    {
        TurnCheck();
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
}

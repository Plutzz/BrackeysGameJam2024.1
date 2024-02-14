using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float targetXVelocity;
    private float targetYVelocity;
    private Vector2 targetVelocity;

    private float xAcceleration = 0;
    private float yAcceleration = 0;
    private float acceleration;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.x != targetXVelocity || rb.velocity.y != targetYVelocity) { 
        
            //Debug.Log($"Current: {rb.velocity.x} Target: {targetXVelocity} Update: {UpdateXVelocity()}");
            AddVelocity(new Vector2(UpdateXVelocity(), UpdateYVelocity()));
        }

        
    }

    private float UpdateXVelocity()
    {
        float changeInXVelocity = targetXVelocity - rb.velocity.x;
        //Close enough to hard set
        if (Mathf.Abs(changeInXVelocity) < Mathf.Abs(xAcceleration) * Time.fixedDeltaTime)
        {
            return changeInXVelocity;
        }

        return Mathf.Sign(changeInXVelocity) * xAcceleration * Time.fixedDeltaTime; 
        
    }
    private float UpdateYVelocity()
    {
        float changeInYVelocity = targetYVelocity - rb.velocity.y;
        //Close enough to hard set
        if (Mathf.Abs(changeInYVelocity) < Mathf.Abs(yAcceleration) * Time.fixedDeltaTime)
        {
            return changeInYVelocity;
        }

        return Mathf.Sign(changeInYVelocity) * yAcceleration * Time.fixedDeltaTime;

    }

    public void ForceSetVelocity(Vector3 velocity) {
        rb.velocity = velocity;
    }

    public void AddVelocity(Vector2 addedVelocity) {
        rb.velocity += addedVelocity;
    }


    public void SetTargetVelocity(Vector2 velocity, float acceleration) {
        SetTargetXVelocity(velocity.x, acceleration);
        SetTargetYVelocity(velocity.y, acceleration);
    }

    public void SetTargetXVelocity(float xVelocity, float acceleration) { 
        targetXVelocity = xVelocity;
        xAcceleration = acceleration;
    }

    public void SetTargetYVelocity(float yVelocity, float acceleration)
    {
        targetYVelocity = yVelocity;
        yAcceleration = acceleration;
    }

    public void ClearTargetVelocity() { 
        acceleration = 0f;
    }


}

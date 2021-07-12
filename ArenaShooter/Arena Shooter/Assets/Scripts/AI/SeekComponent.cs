using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekComponent : MonoBehaviour
{
    
    public float targetRadius;
    public float slowRadius;
    public float speed;
    public float maxAcceleration;
    public float timeToTarget;

    float oneOverTimeToTarget;
    private void Start()
    {
        oneOverTimeToTarget = 1 / timeToTarget;
    }
    public Vector3 Seek(Rigidbody rb, Vector3 targetPosition)
    {
        Vector3 diff = targetPosition - transform.position;
        if (Mathf.Abs(diff.magnitude) <= targetRadius)
        {
            rb.velocity = Vector2.zero;
            //return;
            return Vector3.zero;
        }

        float targetSpeed = 0;
        if (diff.magnitude > slowRadius)
        {
            targetSpeed = speed;
        }
        else
        {
            targetSpeed = speed * diff.magnitude * oneOverTimeToTarget;
            //targetSpeed = speed * diff.magnitude /timeToTarget;
        }

        Vector3 targetVelocity = diff;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        Vector3 acceleration = targetVelocity - rb.velocity;
        acceleration *= oneOverTimeToTarget;

        //rb.AddForce(acceleration);
        return acceleration;
    }
}

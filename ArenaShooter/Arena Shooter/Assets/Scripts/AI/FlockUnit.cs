using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    // Start is called before the first frame update
    FlockUnit[] units;

    Vector3 randomPoint;

    Rigidbody rb;
    SeekComponent mSeek;

    public Transform point;

    public float pointRadius;
    public float pointDistance;
    public float rotationSpeed;
    public float moveSpeed;
    public float maxAcceleration;

    List<Transform> unitsInSight;
    List<Rigidbody> unitsInSightRB;

    float oneOverNumUnitsInSight = 0;
    public float WanderRadius = 0;

    public float AlignWeight = 0;
    public float CohesionWeight = 0;
    public float SeparationWeight = 0;
    void Start()
    {
        units = FindObjectsOfType<FlockUnit>();
        rb = GetComponent<Rigidbody>();
        mSeek = GetComponent<SeekComponent>();

        unitsInSight = new List<Transform>();
        unitsInSightRB = new List<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        randomPoint.x = Random.Range(-pointRadius, pointRadius);
        randomPoint.y = Random.Range(-pointRadius, pointRadius);
        randomPoint.z = Random.Range(-pointRadius, pointRadius);

        randomPoint = transform.position + transform.right * randomPoint.x + transform.up * randomPoint.y 
                                        + transform.forward * randomPoint.z + transform.forward * pointDistance;

        if(randomPoint.sqrMagnitude > WanderRadius*WanderRadius)
        {
            randomPoint.x = Random.Range(-WanderRadius, WanderRadius);
            randomPoint.y = Random.Range(-WanderRadius, WanderRadius);
            randomPoint.z = Random.Range(-WanderRadius, WanderRadius);
        }
        //point.position = randomPoint;

        Quaternion toRot;// = Quaternion.LookRotation((randomPoint - transform.position));
        toRot = Quaternion.LookRotation(rb.velocity);

        transform.rotation = Quaternion.Slerp(transform.rotation, toRot, Time.deltaTime * rotationSpeed);

        //rb.velocity = transform.forward * moveSpeed;

        
    }

    private void FixedUpdate()
    {
        Vector3 acceleration = Vector3.zero;
        if (unitsInSight.Count != 0)
        {
            
            Vector3 averageSeekPosition = Vector3.zero;
            Vector3 averageHeading = Vector3.zero;
            for (int i = 0; i < unitsInSight.Count; i++)
            {
                averageSeekPosition += unitsInSight[i].position;
                averageHeading += unitsInSightRB[i].velocity;
            }
            averageSeekPosition *= oneOverNumUnitsInSight;
            averageHeading *= oneOverNumUnitsInSight;

            Vector3 alignmentForce = mSeek.Seek(rb, transform.position + averageHeading) * AlignWeight;
            Vector3 cohesionForce = mSeek.Seek(rb, averageSeekPosition) * CohesionWeight;
            Vector3 separationForce = mSeek.Seek(rb, transform.position - averageSeekPosition + transform.position) * SeparationWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += separationForce;

            if (acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
            {
                acceleration.Normalize();
                acceleration *= maxAcceleration;
            }

            rb.AddForce(acceleration);
        }
        else
        {
            acceleration = mSeek.Seek(rb, randomPoint);

            if (acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
            {
                acceleration.Normalize();
                acceleration *= maxAcceleration;
            }

            rb.AddForce(acceleration);
        }
        
    }

    public void AddVisibleUnit(Transform newUnit)
    {
        if(!unitsInSight.Contains(newUnit))
        {
            unitsInSight.Add(newUnit);
            unitsInSightRB.Add(newUnit.GetComponent<Rigidbody>());
            oneOverNumUnitsInSight = 1 / unitsInSight.Count;
        }
    }

    public void RemoveVisibleUnit(Transform unitToRemove)
    {
        if (unitsInSight.Contains(unitToRemove))
        {
            unitsInSight.Remove(unitToRemove);
            unitsInSightRB.Remove(unitToRemove.GetComponent<Rigidbody>());
            if(unitsInSight.Count != 0)
            {
                oneOverNumUnitsInSight = 1 / unitsInSight.Count;
            }
            
        }
    }
}


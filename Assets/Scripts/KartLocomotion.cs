using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartLocomotion : MonoBehaviour
{
    [SerializeField] private List<Transform> wheels;
    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float suspensionMaxDist;
    [SerializeField] private float springStrength;
    [SerializeField] private float springDamping;
    [SerializeField] private float suspensionRestDist;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Physics implementation by Toyful Games
    //https://www.youtube.com/watch?v=CdPYlj5uZeI&t=1168s
    void FixedUpdate()
    {
        foreach (Transform wheel in wheels)
        {
            RaycastHit hit;
            Ray ray = new(wheel.transform.position, -Vector3.up);
            if (Physics.Raycast(ray, out hit, suspensionMaxDist))
            {
                CalculateSuspension(wheel.transform, hit);
                CalculateSteering(wheel.transform);
                CalculateAcceleration(wheel.transform);
            }
        }
    }

    //Calculates a dampened spring force.
    private void CalculateSuspension(Transform wheelTransform, RaycastHit wheelRay)
    {
        Vector3 springDir = wheelTransform.up;
        Vector3 wheelWorldVel = rb.GetPointVelocity(wheelTransform.position);

        float offset = suspensionRestDist - wheelRay.distance;
        float vel = Vector3.Dot(springDir, wheelWorldVel);
        
        float force = (offset * springStrength) - (vel * springDamping);

        rb.AddForceAtPosition(springDir * force, wheelTransform.position);
        
    }

    private void CalculateSteering(Transform wheelTransform)
    {
        Vector3 steeringDir = wheelTransform.right;
        Vector3 wheelWorldVel = rb.GetPointVelocity(wheelTransform.position);

        float steeringVel = Vector3.Dot(steeringDir, wheelWorldVel);
        float desiredVelChange = -steeringVel * 1f; //1f = should be a value between 0 to 1. 0 = no grip, 1 = full grip
        
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        rb.AddForceAtPosition(steeringDir * 1 * desiredAccel, wheelTransform.position); //1 = tire mass
    }

    private void CalculateAcceleration(Transform wheelTransform)
    {
        Vector3 accelDir = wheelTransform.forward;

        if (input.accelerating)
        {
            float kartSpeed = 10;
            rb.AddForceAtPosition(accelDir * kartSpeed, wheelTransform.position);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Transform wheel in wheels)
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + (-Vector3.up * suspensionMaxDist));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=CdPYlj5uZeI&t=1168s - Toyful Games explaination of raycast vehicles
//https://www.youtube.com/watch?v=LG1CtlFRmpU&t=283s - SpaceDust Studios explaination of raycast vehicles

[RequireComponent(typeof(Rigidbody))]
public class KartLocomotion : MonoBehaviour
{
    [SerializeField] private List<Transform> wheels;
    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody rb;
    [Header("Suspension")]
    [SerializeField] [Tooltip("The fully extended length of the suspension springs.")]
        private float suspensionLength = 0.5f;
    [SerializeField] [Tooltip("The force of the spring to be applied where the wheel is.")]
        private float springStrength = 600f;
    [SerializeField] [Tooltip("How much force will be resisted by the spring when returning to the rest position.")]
        private float springDamping = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Raycast-based kart implementation
   
    void FixedUpdate()
    {
        foreach (Transform wheel in wheels)
        {
            RaycastHit hit;
            Ray ray = new(wheel.transform.position, -Vector3.up);
            if (Physics.Raycast(ray, out hit, suspensionLength))
            {
                CalculateSuspension(wheel.transform, hit);
            }
        }
    }

    //Calculates a dampened spring force.
    private void CalculateSuspension(Transform wheelTransform, RaycastHit wheelRay)
    {
        Vector3 springDirection = wheelTransform.up;
        Vector3 wheelWorldVel = rb.GetPointVelocity(wheelTransform.position);

        float offset = suspensionLength - wheelRay.distance; //This measures how much the spring is being compressed.
        float vel = Vector3.Dot(springDirection, wheelWorldVel);
        
        float force = (offset * springStrength) - (vel * springDamping);

        rb.AddForceAtPosition(springDirection * force, wheelTransform.position);
        
    }

    private void OnDrawGizmos()
    {
        foreach (Transform wheel in wheels)
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + (-Vector3.up * suspensionLength));
    }
}

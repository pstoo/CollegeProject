using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=CdPYlj5uZeI&t=1168s - Toyful Games explaination of raycast vehicles
//https://www.youtube.com/watch?v=LG1CtlFRmpU&t=283s - SpaceDust Studios explaination of raycast vehicles
//https://digitalrune.github.io/DigitalRune-Documentation/html/143af493-329d-408f-975d-e63625646f2f.htm - Digital Rune documentation

[RequireComponent(typeof(Rigidbody))]
public class KartLocomotion : MonoBehaviour
{
    [SerializeField] private List<Transform> tires;
    [SerializeField] private List<Transform> frontTires;
    [SerializeField] private ScriptableKart kart;

    [SerializeField] private List<Transform> rearTires;

    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody rb;

    private float lerpTimer = 0.0f; //Value used to lerp between the current and desired tire angle.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Raycast-based kart implementation
    void FixedUpdate()
    {
        if (input.steering != 0)
            lerpTimer += Time.fixedDeltaTime;
        else
            lerpTimer -= Time.fixedDeltaTime;

        foreach (Transform tire in tires)
        {
            RaycastHit hit;
            Ray ray = new(tire.transform.position, -Vector3.up);
            if (Physics.Raycast(ray, out hit, kart.SuspensionLength))
            {
                CalculateSuspension(tire, hit);
                CancelSidewaysForce(tire);
                Accelerate(tire);
            }
        }
        foreach (Transform tire in frontTires)
            AdjustSteeringAngle(tire);
    }

    //Calculates a dampened spring force.
    private void CalculateSuspension(Transform tire, RaycastHit ray)
    {
        Vector3 tireWorldVel = rb.GetPointVelocity(tire.position);

        float offset = kart.SuspensionLength - ray.distance; //This measures how much the spring is being compressed.
        float vel = Vector3.Dot(tire.up, tireWorldVel);

        float force = (offset * kart.SpringStrength) - (vel * kart.SpringDamping);

        rb.AddForceAtPosition(tire.up * force, tire.position);
    }

    //Prevents the kart from sliding
    private void CancelSidewaysForce(Transform tire)
    {
        Vector3 tireWorldVel = rb.GetPointVelocity(tire.position);

        float steeringVel = Vector3.Dot(tire.right, tireWorldVel);
        float desiredVelChange = -steeringVel * kart.TireGrip;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        rb.AddForceAtPosition(tire.right * 5f * desiredAccel, tire.position); //5f = Mass of the tires
    }


    private void AdjustSteeringAngle(Transform tire)
    {
        Vector3 currentAngle = tire.localEulerAngles;

        float inputAngle = input.steering * kart.SteeringAngleLimit;
        inputAngle = Mathf.Clamp(inputAngle, -kart.SteeringAngleLimit, kart.SteeringAngleLimit);

        float targetAngle = input.steering != 0 ? inputAngle : currentAngle.y;

        lerpTimer = Mathf.Clamp(lerpTimer, 0, kart.TimeToRotate);
        float t = lerpTimer / kart.TimeToRotate;

        float rotation = Mathf.LerpAngle(0, targetAngle, t);
        tire.localEulerAngles = new Vector3(currentAngle.x, rotation, currentAngle.z);
    }

    private void Accelerate(Transform tire)
    {
        if (input.accelerating)
        {
            float carSpeed = Vector3.Dot(rb.transform.forward, rb.velocity);
            rb.AddForceAtPosition(tire.forward * 125f, tire.position);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Transform wheel in tires)
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + (-Vector3.up * kart.SuspensionLength));
    }
}

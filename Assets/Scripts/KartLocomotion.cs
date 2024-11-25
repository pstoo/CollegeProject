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
    [SerializeField] private List<Transform> tireGFX;
    [SerializeField] private ScriptableKart kart;

    [SerializeField] private List<Transform> frontTires;
    [SerializeField] private List<Transform> rearTires;

    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private KartAnimation animator;

    private float lerpTimer = 0.0f; //Value used to lerp between the current and desired tire angle.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    //Raycast-based kart implementation
    void FixedUpdate()
    {
        // float speed = Vector3.Dot(transform.forward, rb.velocity);
        // float downforce = speed * 20f;
        // rb.AddForce(-transform.up * downforce);
        if (input.steering != 0)
            lerpTimer += Time.fixedDeltaTime;
        else
            lerpTimer -= Time.fixedDeltaTime;

        foreach (Transform tire in tires)
        {
            RaycastHit hit;
            Ray ray = new(tire.position, -Vector3.up);
            if (Physics.Raycast(ray, out hit, kart.SuspensionLength))
            {
                CalculateSuspension(tire, hit);
                if (frontTires.Contains(tire))
                {
                    CancelSlippingForces(tire, kart.FrontWheelGrip);
                    AdjustSteeringAngle(tire);
                }
                else if (rearTires.Contains(tire))
                    CancelSlippingForces(tire, kart.RearWheelGrip);
                Accelerate(tire);
                ApplyHandbrake(tire);
            }
            animator.UpdateSuspensionPoint(tires.IndexOf(tire), hit);
        }
        StabilizeRollForces(frontTires);

        foreach (Transform tire in frontTires)

            for (int i = 0; i < tireGFX.Count; i++)
            {
                if (i < frontTires.Count)
                    animator.UpdateTireRotation(i, frontTires[i].localEulerAngles.y);

                animator.UpdateTirePosition(i, tires[i].localPosition.x, tires[i].localPosition.z);
            }
    }

    private void StabilizeRollForces(List<Transform> tireArray)
    {
        //Code from here: https://gamedev.stackexchange.com/questions/118388/how-to-do-an-anti-sway-bar-for-a-car-in-unity-5
        if (!kart.DoAntiRoll)
            return; //TODO: Debug

        if (tireArray.Count < 2)
            return; //Don't calculate if we don't have don't have enough tires to work with.
        Transform tireA = frontTires[0];
        Transform tireB = frontTires[1];
        float travelA = 1.0f;
        float travelB = 1.0f;
        RaycastHit hit;

        Physics.Raycast(tireA.position, -Vector3.up, out hit, kart.SuspensionLength);
        bool groundedA = hit.collider != null ? true : false;
        if (groundedA)
            travelA = (-tireA.InverseTransformPoint(hit.point).y - kart.TireRadius) / kart.SuspensionLength;

        Physics.Raycast(tireB.position, -Vector3.up, out hit, kart.SuspensionLength);
        bool groundedB = hit.collider != null ? true : false;

        if (groundedB)
            travelB = (-tireB.InverseTransformPoint(hit.point).y - kart.TireRadius) / kart.SuspensionLength;

        float antiRollForce = (travelA - travelB) * kart.AntiRoll;
        if (groundedA)
            rb.AddForceAtPosition(tireA.up * -antiRollForce, tireA.position);
        if (groundedB)
            rb.AddForceAtPosition(tireB.up * antiRollForce, tireB.position);
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
    private void CancelSlippingForces(Transform tire, AnimationCurve gripFactor)
    {
        Vector3 tireWorldVel = rb.GetPointVelocity(tire.position);
        float steeringVel = Vector3.Dot(tire.right, tireWorldVel);
        float slipPercentage = Mathf.Abs(steeringVel) / tireWorldVel.magnitude;

        float tireGripFactor = !float.IsNaN(slipPercentage) ? kart.FrontWheelGrip.Evaluate(slipPercentage) : 0;

        if (kart.UseGripCurve == false)
            tireGripFactor = kart.TireGrip;

        float desiredVelChange = -steeringVel * tireGripFactor;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        rb.AddForceAtPosition(tire.right * kart.TireMass * desiredAccel, tire.position); //5f = Mass of the tires
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
        float coeffient = input.accelerating > 0.0f ? kart.AccelCoeffient : kart.BrakingCoeffient;
        float kartSpeed = Vector3.Dot(transform.forward, rb.velocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(kartSpeed) / kart.TopSpeed);

        float motorForce = (kart.PowerCurve.Evaluate(normalizedSpeed) * input.accelerating) * (rb.mass * coeffient);

        //Debug.Log($"speed:{kartSpeed},speed%:{normalizedSpeed},motorForce:{motorForce}");

        if (kart.DoPowerCurve)
            rb.AddForceAtPosition(tire.forward * motorForce, tire.position);
        else
            rb.AddForceAtPosition(tire.forward * kart.TopSpeed, tire.position);
    }

    private float CalculateBrakeForces(Transform tire)
    {
        Vector3 tireWorldVel = rb.GetPointVelocity(tire.position);
        float forwardVel = Vector3.Dot(tire.forward, tireWorldVel);
        float accelPercentage = Mathf.Abs(forwardVel) / tireWorldVel.magnitude;

        float desiredVelChange = -forwardVel * kart.BrakingCoeffient;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        return desiredAccel;
    }

    private void ApplyHandbrake(Transform tire)
    {
        if (input.brake)
        {
            Debug.Log("found");
            float brakeForce = CalculateBrakeForces(tire);
            rb.AddForceAtPosition(tire.forward * brakeForce, tire.position);
        }
    }

    public InputManager Input { set { input = value; } }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(rb.centerOfMass + transform.position, 0.5f);
        //Debug.Log(rb.centerOfMass);

        foreach (Transform wheel in tires)
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + (-Vector3.up * kart.SuspensionLength));
    }
}

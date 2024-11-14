using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kart", menuName = "ScriptableObjects/Kart")]
public class ScriptableKart : ScriptableObject
{
    //At some point, should probably add mass, center of gravity, and maybe a prefab or model to this

    [Header("Suspension")]
    [SerializeField]
    [Tooltip("The fully extended length of the suspension springs.")]
    private float suspensionLength = 0.5f;
    [SerializeField]
    [Tooltip("The force of the spring to be applied where the wheel is.")]
    private float springStrength = 600.0f;
    [SerializeField]
    [Tooltip("How much force will be resisted by the spring when returning to the rest position.")]
    private float springDamping = 15.0f;

    [Header("Steering")]
    [SerializeField]
    [Tooltip("The maximum and minimum angle of the tires when steered.")]
    private float steeringAngleLimit = 30.0f;
    [SerializeField]
    [Tooltip("How long it takes for the tires to reach the maximum steering angle in seconds.")]
    private float timeToRotate = 0.5f;

    [Header("Anti-Slipping")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("A value between 0..1 that determines how much the tires will resist sliding.")]
    private float tireGrip = 1.0f;
    [SerializeField]
    [Tooltip("tbi")]
    private AnimationCurve frontWheelGrip;
    [SerializeField]
    [Tooltip("tbi")]
    private AnimationCurve rearWheelGrip;
    [SerializeField]
    [Tooltip("How")]
    private float antiRollForce = 100.0f;
    [SerializeField] private bool useGripCurve = true;
    [SerializeField] private bool doAntiRoll = true;

    [Header("Acceleration")]
    [SerializeField] private float speed = 50f;

    [SerializeField] private float tireRadius = 0.425f;


    //Properties
    public float SuspensionLength { get { return suspensionLength; } }
    public float SpringStrength { get { return springStrength; } }
    public float SpringDamping { get { return springDamping; } }

    public float SteeringAngleLimit { get { return steeringAngleLimit; } }
    public float TimeToRotate { get { return timeToRotate; } }

    public float TireGrip { get { return tireGrip; } }
    public AnimationCurve FrontWheelGrip { get { return frontWheelGrip; } }
    public AnimationCurve RearWheelGrip { get { return rearWheelGrip; } }
    public bool UseGripCurve { get { return useGripCurve;} }
    public float AntiRoll { get { return antiRollForce; } }
    public bool DoAntiRoll { get { return doAntiRoll;} }

    public float Speed { get { return speed; } }

    public float TireRadius { get { return tireRadius; } }
}

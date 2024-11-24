using System.Collections;
using System.Collections.Generic;
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

    [Header("Speed")]
    [SerializeField] private float topSpeed = 50f;
    [SerializeField]
    [Tooltip("tbi")]
    private AnimationCurve powerCurve;

    [SerializeField] private float tireRadius = 0.425f;
    
    [SerializeField] private bool useGripCurve = true;
    [SerializeField] private bool doAntiRoll = true;
    [SerializeField] private bool doPowerCurve = true;


    //Properties
    public float SuspensionLength { get { return suspensionLength; } }
    public float SpringStrength { get { return springStrength; } }
    public float SpringDamping { get { return springDamping; } }

    public float SteeringAngleLimit { get { return steeringAngleLimit; } }
    public float TimeToRotate { get { return timeToRotate; } }

    public float TireGrip { get { return tireGrip; } }
    public AnimationCurve FrontWheelGrip { get { return frontWheelGrip; } }
    public AnimationCurve RearWheelGrip { get { return rearWheelGrip; } }
    public float AntiRoll { get { return antiRollForce; } }

    public float TopSpeed { get { return topSpeed; } }
    public AnimationCurve PowerCurve { get { return powerCurve;} }

    public float TireRadius { get { return tireRadius; } }

    public bool UseGripCurve { get { return useGripCurve;} }
    public bool DoAntiRoll { get { return doAntiRoll;} }
    public bool DoPowerCurve { get { return doPowerCurve; } }
}

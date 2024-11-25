using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float accelerating { get; private set; }
    public float steering { get; private set; }
    public bool brake { get; private set; }
    //public float steeringAngle { get; private set; } = 30.0f;
    public void Accelerate(InputAction.CallbackContext context)
    {
        accelerating = context.ReadValue<float>();
    }

    public void Steer(InputAction.CallbackContext context)
    {
        steering = context.ReadValue<float>();
    }

    public void Brake(InputAction.CallbackContext context)
    {
        brake = context.performed;
    }
}

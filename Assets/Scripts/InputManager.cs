using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool accelerating { get; private set; }
    public void Accelerate(InputAction.CallbackContext context)
    {
        accelerating = context.performed;
    }
}

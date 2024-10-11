using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void Accelerate(InputAction.CallbackContext context)
    {
        Debug.Log("Let's goooo " + context.ReadValue<float>());
    }
}

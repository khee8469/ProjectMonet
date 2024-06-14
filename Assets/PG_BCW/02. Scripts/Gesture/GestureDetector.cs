using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestureDetector : MonoBehaviour
{
    public InputActionProperty pinchAction;

    private void OnEnable()
    {
        pinchAction.action.performed += OnPinchPerformed;
        pinchAction.action.canceled += OnPinchCanceled;
        pinchAction.action.Enable();
    }

    private void OnDisable()
    {
        pinchAction.action.performed -= OnPinchPerformed;
        pinchAction.action.canceled -= OnPinchCanceled;
        pinchAction.action.Disable();
    }

    private void OnPinchPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Pinch gesture performed");
    }

    private void OnPinchCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Pinch gesture canceled");
    }
}

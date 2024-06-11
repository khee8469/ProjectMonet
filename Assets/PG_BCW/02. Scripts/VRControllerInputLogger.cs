using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRControllerInputLogger : MonoBehaviour
{
    public InputActionReference triggerActionReference;
    public InputActionReference gripActionReference;
    public InputActionReference primaryButtonActionReference;

    private void OnEnable()
    {
        // InputActionReference에서 InputAction 가져오기 및 이벤트 핸들러 등록
        triggerActionReference.action.performed += OnTriggerPerformed;
        triggerActionReference.action.canceled += OnTriggerCanceled;

        gripActionReference.action.performed += OnGripPerformed;
        gripActionReference.action.canceled += OnGripCanceled;

        primaryButtonActionReference.action.performed += OnPrimaryButtonPerformed;
        primaryButtonActionReference.action.canceled += OnPrimaryButtonCanceled;

        // 액션 활성화
        triggerActionReference.action.Enable();
        gripActionReference.action.Enable();
        primaryButtonActionReference.action.Enable();
    }

    private void OnDisable()
    {
        // 이벤트 핸들러 해제 및 액션 비활성화
        triggerActionReference.action.performed -= OnTriggerPerformed;
        triggerActionReference.action.canceled -= OnTriggerCanceled;

        gripActionReference.action.performed -= OnGripPerformed;
        gripActionReference.action.canceled -= OnGripCanceled;

        primaryButtonActionReference.action.performed -= OnPrimaryButtonPerformed;
        primaryButtonActionReference.action.canceled -= OnPrimaryButtonCanceled;

        triggerActionReference.action.Disable();
        gripActionReference.action.Disable();
        primaryButtonActionReference.action.Disable();
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger action performed!");
    }

    private void OnTriggerCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger action canceled!");
    }

    private void OnGripPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Grip action performed!");
    }

    private void OnGripCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Grip action canceled!");
    }

    private void OnPrimaryButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Primary button action performed!");
    }

    private void OnPrimaryButtonCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Primary button action canceled!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ModalityManager : UnityEngine.XR.Interaction.Toolkit.XRController
{
    [Header("현재 스크립트")]

    [Tooltip("XRInputModalityManager를 할당하시오")]
    [SerializeField]
    XRInputModalityManager xRInputModalityManager;

    //true일때 컨트롤러 추적을 하지않아 컨트롤러를 흔들어도 핸드트래킹을 유지
    private bool isHandTracking = false;

    protected override void Awake()
    {
        base.Awake();

        if (xRInputModalityManager == null) Debug.LogError("ModalityManager클래스에 XRInputModalityManager를 할당하시오");
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (xRInputModalityManager != null)
        {
            xRInputModalityManager.trackedHandModeStarted.AddListener(SetHandTrackingOn);
            xRInputModalityManager.trackedHandModeEnded.AddListener(SetHandTrackingOff);
        }

    }

    public void SetHandTrackingOn()
    {
        isHandTracking = true;
    }

    public void SetHandTrackingOff()
    {
        isHandTracking = false;
    }

    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        if (controllerState != null && isHandTracking)
        {
            //currentControllerState.position = Vector3.zero;
            //currentControllerState.rotation = Quaternion.identity;
            currentControllerState.isTracked = false;
            currentControllerState.inputTrackingState = InputTrackingState.None;
            return;
        }
    }





    /*protected override void UpdateInput(XRControllerState controllerState)
    {
        if (controllerState == null)
            return;

        // 특정 버튼 입력이 있을 때만 컨트롤러 입력을 업데이트
        if (!isHandTrackingActive)
        {
            base.UpdateInput(controllerState);
        }
        else
        {
            controllerState.ResetFrameDependentStates();

            // 예: 트리거 버튼이 눌렸는지 확인
            bool triggerPressed = inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue) && triggerValue;

            if (triggerPressed)
            {
                // 핸드 트래킹을 비활성화하고 컨트롤러 입력을 업데이트
                isHandTrackingActive = false;
                base.UpdateInput(controllerState);
            }
        }
    }*/



    /*void Start()
    {
        InitializeDevices();
    }

    void Update()
    {
        // 손 트래킹 모드가 활성화된 동안에는 컨트롤러의 위치와 회전을 추적하지 않음
        if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
        {
            return;
        }

        // 컨트롤러의 위치와 회전 추적
        UpdateControllerData();
    }

    void InitializeDevices()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();

        // 왼쪽 컨트롤러 초기화
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftController = inputDevices[0];
            Debug.Log("Left controller initialized.");
        }
        else
        {
            Debug.LogWarning("No left controller found.");
        }

        // 리스트 비우기
        inputDevices.Clear();

        // 오른쪽 컨트롤러 초기화
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);
        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
            Debug.Log("Right controller initialized.");
        }
        else
        {
            Debug.LogWarning("No right controller found.");
        }

        // 모든 장치 출력 (디버깅용)
        InputDevices.GetDevices(inputDevices);
        Debug.Log("All detected devices:");
        foreach (var device in inputDevices)
        {
            Debug.Log($"Device found: {device.name} Characteristics: {device.characteristics}");
        }
    }

    void UpdateControllerData()
    {
        if (leftController.isValid)
        {
            if (leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 leftPosition))
            {
                Debug.Log($"Left Controller Position: {leftPosition}");
            }

            if (leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion leftRotation))
            {
                Debug.Log($"Left Controller Rotation: {leftRotation}");
            }

            // 버튼 입력 감지
            if (leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool leftPrimaryButton) && leftPrimaryButton)
            {
                Debug.Log($"Left Controller leftPrimaryButton: {leftPrimaryButton}");
            }
        }
        else
        {
            Debug.LogWarning("Left controller is not valid.");
        }

        if (rightController.isValid)
        {
            if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 rightPosition))
            {
                Debug.Log($"Right Controller Position: {rightPosition}");
            }

//            if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion rightRotation))
            {
                Debug.Log($"Right Controller Rotation: {rightRotation}");
            }

            // 버튼 입력 감지
            if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool rightPrimaryButton) && rightPrimaryButton)
            {
                Debug.Log($"Left Controller rightPrimaryButton: {rightPrimaryButton}");
            }
        }
        else
        {
            Debug.LogWarning("Right controller is not valid.");
        }
    }*/
}

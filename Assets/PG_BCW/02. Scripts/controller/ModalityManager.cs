using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ModalityManager : MonoBehaviour
{
    [SerializeField]
    XRInputModalityManager xrInputModalityManager;


    private void Awake()
    {
        xrInputModalityManager.trackedHandModeStarted.AddListener(ControllerInputOff);
    }

    private void ControllerInputOff()
    {

    }
    void Update()
    {
        if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
        {
            // 손 트래킹 모드가 활성화된 동안 컨트롤러의 위치와 회전을 추적하지 않습니다.
            Debug.Log(123);
            return;
        }
    }

    /*private InputDevice leftController;
    private InputDevice rightController;

    void OnEnable()
    {
        // Initialize the devices
        InitializeDevices();

        // Subscribe to XRInputModalityManager events
        xrInputModalityManager.trackedHandModeStarted.AddListener(OnTrackedHandModeStarted);
        xrInputModalityManager.trackedHandModeEnded.AddListener(OnTrackedHandModeEnded);
    }

    void OnDisable()
    {
        // Unsubscribe from XRInputModalityManager events
        xrInputModalityManager.trackedHandModeStarted.RemoveListener(OnTrackedHandModeStarted);
        xrInputModalityManager.trackedHandModeEnded.RemoveListener(OnTrackedHandModeEnded);
    }

    void InitializeDevices()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDevices);
        if (inputDevices.Count > 0)
            leftController = inputDevices[0];

        inputDevices.Clear();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);
        if (inputDevices.Count > 0)
            rightController = inputDevices[0];
    }

    void OnTrackedHandModeStarted()
    {
        // 손 트래킹 모드가 시작되면 컨트롤러의 위치와 회전 추적을 중지합니다.
        Debug.Log("Hand tracking mode started. Stopping controller tracking.");
    }

    void OnTrackedHandModeEnded()
    {
        // 손 트래킹 모드가 종료되면 컨트롤러의 버튼 입력을 다시 추적하도록 합니다.
        Debug.Log("Hand tracking mode ended. Resuming controller button tracking.");
    }*/
}

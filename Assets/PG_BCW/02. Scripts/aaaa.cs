using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class aaaa : MonoBehaviour
{
    /*public XRNode handNode; // LeftHand 또는 RightHand 선택
    private InputDevice handDevice;
    public XRDirectInteractor interactor;

    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(handNode, devices);
        if (devices.Count > 0)
        {
            handDevice = devices[0];
        }
    }

    private void Update()
    {
        if (handDevice.isValid && handDevice.TryGetFeatureValue(CommonUsages.isTracked, out bool isTracked) && isTracked)
        {
            if (handDevice.TryGetFeatureValue(CommonUsages.handData, out Hand hand))
            {
                if (CheckFistGesture(hand))
                {
                    if (interactor.selectTarget == null)
                    {
                        GrabObject();
                    }
                }
                else
                {
                    if (interactor.selectTarget != null)
                    {
                        ReleaseObject();
                    }
                }
            }
        }
    }

    private bool CheckFistGesture(Hand hand)
    {
        // 모든 손가락이 구부러져 있는지를 확인하는 로직
        float totalFlex = 0;
        foreach (var finger in hand.fingers)
        {
            totalFlex += finger.bone3.rotation.eulerAngles.magnitude; // 손가락의 마지막 마디 굴곡 값 사용
        }
        return (totalFlex / hand.fingers.Count) > 45; // 적절한 기준 값 사용
    }

    private void GrabObject()
    {
        // 물체를 잡는 로직
        interactor.StartManualInteraction(interactor.firstInteractableSelected);
        Debug.Log("Object grabbed!");
    }

    private void ReleaseObject()
    {
        // 물체를 놓는 로직
        interactor.EndManualInteraction();
        Debug.Log("Object released!");
    }*/
}

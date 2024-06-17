using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Samples.GestureSample;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureGesture : MonoBehaviour
{
    public XRBaseInteractor leftHandInteractor;
    public StaticHandGesture leftGrabGesture;

    void Update()
    {


        if (CheckHandGesture(leftHandInteractor, leftGrabGesture))
        {
            GrabObject(leftHandInteractor);
        }

    }

    bool CheckHandGesture(XRBaseInteractor interactor, StaticHandGesture gesture)
    {
        Debug.Log(gesture.m_PerformedTriggered);
        return gesture.m_PerformedTriggered;
    }

    void GrabObject(XRBaseInteractor interactor)
    {
        var interactable = interactor.selectTarget;
        if (interactable != null && interactable.GetComponent<XRGrabInteractable>() != null)
        {
            Debug.Log(1111111111);

           interactor.interactionManager.SelectEnter(interactor, interactable);
        }
    }
}

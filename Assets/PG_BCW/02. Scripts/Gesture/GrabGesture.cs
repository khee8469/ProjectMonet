using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //호버중인 오브젝트가잇는지 확인
        var hoveredInteractable = interactor.interactablesHovered.FirstOrDefault();
        var interactable = hoveredInteractable as IXRSelectInteractable;

        if (interactable != null && interactable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            Debug.Log(interactor.transform.name);
            Debug.Log(interactable.transform.name);
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            interactor.interactionManager.SelectEnter(interactor, interactable);
        }
        else
        {
            Debug.Log("No valid interactable found to grab.");
        }
    }
}

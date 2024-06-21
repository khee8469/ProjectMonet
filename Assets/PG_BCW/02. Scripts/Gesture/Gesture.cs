using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;
using UnityEngine.XR.Interaction.Toolkit;

public class Gesture : MonoBehaviour
{
    [Tooltip("interactionManager에 상호작용 신청할 leftInteractor")]
    public XRBaseInteractor leftHandInteractor;
    [Tooltip("interactionManager에 상호작용 신청할 rightInteractor")]
    public XRBaseInteractor rightHandInteractor;

    [Tooltip("제스처가 저장된 클래스, 제스처 실행여부 판단")]
    public StaticHandGesture HandGesture;

    [Tooltip("잡은 오브젝트 정보 임시 저장용")]
    protected IXRSelectInteractable leftSelectedInteractable;
    protected IXRSelectInteractable rightSelectedInteractable;


    void Update()
    {
        //Debug.Log(HandGesture.leftPerformedTriggered);
        if (HandGesture.leftPerformedTriggered && !leftHandInteractor.hasSelection)
        {
            GestureEnter(leftHandInteractor);

        }

        if (!HandGesture.leftPerformedTriggered && leftHandInteractor.hasSelection)
        {
            GestureExit(leftHandInteractor);

        }

        if (HandGesture.rightPerformedTriggered && !rightHandInteractor.hasSelection)
        {
            GestureEnter(rightHandInteractor);

        }

        if (!HandGesture.rightPerformedTriggered && rightHandInteractor.hasSelection)
        {
            GestureExit(rightHandInteractor);

        }
    }

    public virtual void GestureEnter(XRBaseInteractor interactor)
    {

    }

    public virtual void GestureExit(XRBaseInteractor interactor)
    {

    }
}

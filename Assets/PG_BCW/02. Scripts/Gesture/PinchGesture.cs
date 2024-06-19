using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

public class PinchGesture : Gesture
{
    /*PaintBucket paintBucket;


    bool isLeftPinch = false;
    bool isRightPinch = false;

    void Update()
    {
        //조건 다시 봐야함 막썻음
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


    //기능 1. 물감을 짜낸다.
    public override void GestureEnter(XRBaseInteractor interactor)
    {
        Debug.Log("핀치 제스처");
        paintBucket = null;
        //호버중인 오브젝트중 물감통 찾기
        if (interactor.interactablesHovered.Count > 0)
        {
            foreach (var a in interactor.interactablesHovered)
            {
                paintBucket = a as PaintBucket;
                if (paintBucket != null)
                {
                    //물감나오는거 함수 호출
                    paintBucket.PaintPlay();
                    Debug.Log("물감호출");
                    break;
                }
            }
        }
    }

    public override void GestureExit(XRBaseInteractor interactor)
    {       
        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }*/
}

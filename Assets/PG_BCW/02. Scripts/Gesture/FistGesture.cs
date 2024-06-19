using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FistGesture : Gesture
{
    PaintBucket paintBucket;

    bool isLeftFist = false;
    bool isRightFist = false;

    void Update()
    {
        //조건 다시 봐야함 막썻음
        //Debug.Log(HandGesture.leftPerformedTriggered);
        if (HandGesture.leftPerformedTriggered && !leftHandInteractor.hasSelection && !isLeftFist)
        {
            GestureEnter(leftHandInteractor);
        }

        if (!HandGesture.leftPerformedTriggered && leftHandInteractor.hasSelection && isLeftFist)
        {
            GestureExit(leftHandInteractor);
        }

        if (HandGesture.rightPerformedTriggered && !rightHandInteractor.hasSelection && !isRightFist)
        {
            GestureEnter(rightHandInteractor);
        }

        if (!HandGesture.rightPerformedTriggered && rightHandInteractor.hasSelection && isRightFist)
        {
            GestureExit(rightHandInteractor);
        }
    }

    //기능 1. 물감을 짜낸다.
    public override void GestureEnter(XRBaseInteractor interactor)
    {
        Debug.Log("피스트 제스처");

        IXRSelectInteractable hoveredInteractable = null;
        foreach (var interactable in interactor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
            {
                Debug.Log(hoveredInteractable.transform.name);
                break;
            }

        }
        //Debug.Log(hoveredInteractable);
        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            interactor.interactionManager.SelectEnter(interactor, hoveredInteractable);

            // 왼손 오른손 잡은 오브젝트 정보 저장
            if (interactor == leftHandInteractor)
            {
                leftSelectedInteractable = hoveredInteractable;

                isLeftFist = true;
            }
            else
            {
                rightSelectedInteractable = hoveredInteractable;

                isRightFist = true;
            }
        }


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
        // 저장된 손 오브젝트 놓기
        if (interactor == leftHandInteractor)
        {
            if (leftSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
                interactor.interactionManager.SelectExit(interactor, leftSelectedInteractable);
                leftSelectedInteractable = null;

                isLeftFist = false;
            }
        }
        else
        {
            if (rightSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
                interactor.interactionManager.SelectExit(interactor, rightSelectedInteractable);
                rightSelectedInteractable = null;

                isRightFist = false;
            }
        }


        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }
}

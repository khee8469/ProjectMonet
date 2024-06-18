using System.Linq;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabGesture : MonoBehaviour
{
    [Tooltip("interactionManager에 상호작용 신청할 leftInteractor")]
    public XRBaseInteractor leftHandInteractor;
    [Tooltip("interactionManager에 상호작용 신청할 rightInteractor")]
    public XRBaseInteractor rightHandInteractor;

    [Tooltip("제스처가 저장된 클래스, 제스처 실행여부 판단")]
    public StaticHandGesture GrabHandGesture;
    [Tooltip("잡은 오브젝트 정보 임시 저장용")]
    private IXRSelectInteractable leftSelectedInteractable;
    private IXRSelectInteractable rightSelectedInteractable;

    private bool isLeftGestureActive = false;
    private bool isRightGestureActive = false;

    void Update()
    {
        Debug.Log(GrabHandGesture.leftPerformedTriggered);
        if (GrabHandGesture.leftPerformedTriggered && !leftHandInteractor.hasSelection)
        {
            GrabEnter(leftHandInteractor);

        }

        if (!GrabHandGesture.leftPerformedTriggered && leftHandInteractor.hasSelection)
        {
            GrabExit(leftHandInteractor);

        }



        if (GrabHandGesture.rightPerformedTriggered && !rightHandInteractor.hasSelection)
        {
            GrabEnter(rightHandInteractor);

        }

        if (!GrabHandGesture.rightPerformedTriggered && rightHandInteractor.hasSelection)
        {
            GrabExit(rightHandInteractor);

        }

    }

    // 물건잡기
    private void GrabEnter(XRBaseInteractor interactor)
    {
        // 호버중인 오브젝트가 있는지 확인
        IXRSelectInteractable hoveredInteractable = interactor.interactablesHovered.FirstOrDefault() as IXRSelectInteractable;
        

        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            Debug.Log(hoveredInteractable.transform.name);
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            interactor.interactionManager.SelectEnter(interactor, hoveredInteractable);
            
            // 왼손 오른손 잡은 오브젝트 정보 저장
            if (interactor == leftHandInteractor)
            {
                Debug.Log(22222222222222);
                leftSelectedInteractable = hoveredInteractable;
            }
            else
            {
                rightSelectedInteractable = hoveredInteractable;
            }
        }
    }

    // 물건놓기
    private void GrabExit(XRBaseInteractor interactor)
    {
        // 저장된 손 오브젝트 놓기
        if (interactor == leftHandInteractor)
        {
            if (leftSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
                interactor.interactionManager.SelectExit(interactor, leftSelectedInteractable);

                leftSelectedInteractable = null;
            }
        }
        else
        {
            if (rightSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
                interactor.interactionManager.SelectExit(interactor, rightSelectedInteractable);

                rightSelectedInteractable = null;
            }
        }
    }



    /*void Update()
    {

        if (GrabHandGesture.leftPerformedTriggered && !leftHandInteractor.selectTarget)
        {
            GrabEnter(leftHandInteractor);
            leftHandInteractor.allowSelect = false;
        }
        else if(!GrabHandGesture.leftPerformedTriggered && leftHandInteractor.selectTarget)
        {
            GrabExit(leftHandInteractor);
        }

        if (GrabHandGesture.rightPerformedTriggered && !rightHandInteractor.selectTarget)
        {
            GrabEnter(rightHandInteractor);
        }
        else if (!GrabHandGesture.rightPerformedTriggered && rightHandInteractor.selectTarget)
        {
            GrabExit(rightHandInteractor);
        }
    }

    //물건잡기
    private void GrabEnter(XRBaseInteractor interactor)
    {
        //호버중인 오브젝트가잇는지 확인
        IXRSelectInteractable hoveredInteractable = interactor.interactablesHovered.FirstOrDefault() as IXRSelectInteractable;

        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            interactor.interactionManager.SelectEnter(interactor, hoveredInteractable);

            //왼손 오른손 잡은 오브젝트 정보 저장
            if(interactor == rightHandInteractor)
            {
                leftSelectedInteractable = hoveredInteractable;
            }
            else
            {
                rightSelectedInteractable = hoveredInteractable;
            }
        }
    }

    //물건놓기
    private void GrabExit(XRBaseInteractor interactor)
    {
        //저장된 손 오브젝트 놓기
        if(interactor == leftHandInteractor)
        {
            if (leftSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
                interactor.interactionManager.SelectExit(interactor, leftSelectedInteractable);

                leftSelectedInteractable = null;
            }
        }
        else
        {
            if (rightSelectedInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
                interactor.interactionManager.SelectExit(interactor, rightSelectedInteractable);

                rightSelectedInteractable = null;
            }
        }
    }*/
}

using System.Linq;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabGesture : Gesture
{
    // 물건잡기
    public override  void GestureEnter(XRBaseInteractor interactor)
    {
        Debug.Log("그랩 제스처");
        // 호버중인 오브젝트가 있는지 확인
        //IXRSelectInteractable hoveredInteractable = interactor.interactablesHovered.FirstOrDefault() as IXRSelectInteractable;
        IXRSelectInteractable hoveredInteractable = null;
        foreach (var interactable in interactor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
                break;
        }
        Debug.Log(hoveredInteractable);
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
}

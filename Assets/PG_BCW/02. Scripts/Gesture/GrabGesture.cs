using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class GrabGesture : Gesture
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    // 왼손 물건잡기
    public override void LeftGestureEnter()
    {
        //Debug.Log("그랩 제스처");

        //호버된 오브젝트 중에서 잡을수있는 오브젝트 찾기
        IXRSelectInteractable hoveredInteractable = null;

        foreach (var interactable in LeftHandInteractor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
            {
                //Debug.Log(hoveredInteractable.transform.name);
                break;
            }

        }
        //Debug.Log(hoveredInteractable);
        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            LeftHandInteractor.interactionManager.SelectEnter(LeftHandInteractor, hoveredInteractable);

            // 왼손 오브젝트 정보 저장
            LeftSelectedInteractable = hoveredInteractable;
        }
    }

    // 왼손 물건놓기
    public override void LeftGestureExit()
    {
        /*Debug.Log("그랩 제스처 끝");

        // 저장된 손 오브젝트 놓기
        if (LeftSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            LeftHandInteractor.interactionManager.SelectExit(LeftHandInteractor, LeftSelectedInteractable);
            LeftSelectedInteractable = null;
        }*/
    }




    // 오른손 물건잡기
    public override void RightGestureEnter()
    {
        // 호버중인 오브젝트가 있는지 확인
        //Debug.Log("그랩 제스처");

        IXRSelectInteractable hoveredInteractable = null;

        foreach (var interactable in RightHandInteractor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
            {
                //Debug.Log(hoveredInteractable.transform.name);
                break;
            }

        }
        //Debug.Log(hoveredInteractable);
        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            RightHandInteractor.interactionManager.SelectEnter(RightHandInteractor, hoveredInteractable);

            // 왼손 오브젝트 정보 저장
            RightSelectedInteractable = hoveredInteractable;
        }
    }

    // 오른손 물건놓기
    public override void RightGestureExit()
    {
        /*Debug.Log("그랩 제스처 끝");

        // 저장된 손 오브젝트 놓기
        if (RightSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            RightHandInteractor.interactionManager.SelectExit(RightHandInteractor, RightSelectedInteractable);
            RightSelectedInteractable = null;
        }*/
    }
}

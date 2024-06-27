using JJH;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class GrabGesture : Gesture
{
    //물건 잡기용 제스처

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

        //호버된거도 없으면 끝
        if (LeftHandInteractor.interactablesHovered.Count == 0)
            return;

        //호버된 오브젝트 중에서 잡을수있는 오브젝트 찾기
        IXRSelectInteractable hoveredInteractable = null;

        foreach (var interactable in LeftHandInteractor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
                LeftHandInteractor.interactionManager.SelectEnter(LeftHandInteractor, hoveredInteractable);

                // 아이템이 select enter 되어서 손에 붙어 있는 경우고 이 상황에서만 pen은 line Renderer 를 생성해줘야한다.
                if(hoveredInteractable is Pen)
                {
                    Pen pen = hoveredInteractable as Pen;
                    Debug.Log("Pen의 start 그랩");
                    pen.StartDrawing();
                }
                break;
            }
        }
    }

    // 왼손 물건놓기
    public override void LeftGestureExit()
    {

    }




    // 오른손 물건잡기
    public override void RightGestureEnter()
    {
        
        Debug.Log("그랩 제스처");

        //호버된거도 없으면 끝
        if (RightHandInteractor.interactablesHovered.Count == 0)
            return;
        // 호버중인 오브젝트가 있는지 확인
        IXRSelectInteractable hoveredInteractable = null;

        foreach (var interactable in RightHandInteractor.interactablesHovered)
        {
            hoveredInteractable = interactable as IXRSelectInteractable;

            if (hoveredInteractable != null)
            {
                // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
                RightHandInteractor.interactionManager.SelectEnter(RightHandInteractor, hoveredInteractable);

                if(hoveredInteractable is Pen)
                {
                    Pen pen =  hoveredInteractable as Pen;
                    pen.StartDrawing();
                    Debug.Log("Pen hovered");

                }

                break;
            }
        }
    }

    // 오른손 물건놓기
    public override void RightGestureExit()
    {

    }
}

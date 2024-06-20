using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FistGesture : Gesture
{
    PaintBucket paintBucket;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    //기능 물건 잡기, 물감짜기
    public override void LeftGestureEnter()
    {
        Debug.Log("피스트 제스처");

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

        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            LeftHandInteractor.interactionManager.SelectEnter(LeftHandInteractor, hoveredInteractable);

            // 왼손 오브젝트 정보 저장
            LeftSelectedInteractable = hoveredInteractable;
        }


        paintBucket = null;
        //호버중인 오브젝트중 물감통 찾기
        if (LeftHandInteractor.interactablesHovered.Count > 0)
        {
            foreach (var a in LeftHandInteractor.interactablesHovered)
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

    public override void LeftGestureExit()
    {
        /*// 저장된 손 오브젝트 놓기
        if (LeftSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            LeftHandInteractor.interactionManager.SelectExit(LeftHandInteractor, LeftSelectedInteractable);
            LeftSelectedInteractable = null;
        }*/

        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }






    //기능 물건 잡기, 물감짜기
    public override void RightGestureEnter()
    {
        Debug.Log("피스트 제스처");

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

        if (hoveredInteractable != null && hoveredInteractable.transform.GetComponent<XRGrabInteractable>() != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택하도록 요청
            RightHandInteractor.interactionManager.SelectEnter(RightHandInteractor, hoveredInteractable);

            // 왼손 오브젝트 정보 저장
            RightSelectedInteractable = hoveredInteractable;
        }


        paintBucket = null;
        //호버중인 오브젝트중 물감통 찾기
        if (RightHandInteractor.interactablesHovered.Count > 0)
        {
            foreach (var a in RightHandInteractor.interactablesHovered)
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

    public override void RightGestureExit()
    {
        /*// 저장된 손 오브젝트 놓기
        if (RightSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            RightHandInteractor.interactionManager.SelectExit(RightHandInteractor, LeftSelectedInteractable);
            RightSelectedInteractable = null;
        }*/

        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }
}

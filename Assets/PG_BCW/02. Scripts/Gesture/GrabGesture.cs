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
        /*//Debug.Log("그랩 제스처");

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
                    Debug.Log("Pen 형 오부젝트를 selectEnter 하였다.");
                }
                break;
            }
        }*/

        PaintBucket paintBucket = null;
        Pen pen = null;
        //호버중인 오브젝트중 물감통 찾기
        if (LeftHandInteractor.hasSelection)
        {
            Debug.Log("레프트 제스쳐 피스트 제스쳐");
            //잡고잇는 오브젝트 첫번쨰
            paintBucket = LeftHandInteractor.interactablesSelected[0] as PaintBucket;
            if (paintBucket != null)
            {
                //물감나오는거 함수 호출
                paintBucket.PaintPlay();
                Debug.Log("물감호출");
            }

            pen = LeftHandInteractor.interactablesSelected[0] as Pen;
            if (pen != null)
            {
                // Pen의 그리기 가능 함수 호출 
                pen.StartDrawing();
                Debug.Log("Pen is Not NULL");
            }
        }
        else if (!LeftHandInteractor.hasSelection)
        {
            Debug.Log("레프트 제스쳐 피스트 제스쳐22");
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
                    break;
                }
            }

            pen = LeftHandInteractor.interactablesSelected[0] as Pen;
            if (pen != null)
            {
                // Pen의 그리기 가능 함수 호출 
                pen.StartDrawing();
                Debug.Log("Pen is Not NULL");
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

        /*Debug.Log("그랩 제스처");

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

                break;
            }
        }*/

        PaintBucket paintBucket = null;
        Pen pen = null;
        //호버중인 오브젝트중 물감통 찾기
        if (RightHandInteractor.hasSelection)
        {
            Debug.Log("라이트 제스쳐 피스트 제스쳐33");
            //잡고잇는 오브젝트 첫번쨰
            paintBucket = RightHandInteractor.interactablesSelected[0] as PaintBucket;
            if (paintBucket != null)
            {
                //물감나오는거 함수 호출
                paintBucket.PaintPlay();
                Debug.Log("물감호출");
            }
            Debug.Log(RightHandInteractor.interactablesSelected.Count + "숫자");
            pen = RightHandInteractor.interactablesSelected[0] as Pen;
            if (pen != null)
            {
                // Pen의 그리기 가능 함수 호출 
                pen.StartDrawing();
                Debug.Log("Pen is Not NULL");
            }

        }

        else if (!RightHandInteractor.hasSelection)
        {
            Debug.Log("레프트 제스쳐 피스트 제스쳐44");

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
                    break;
                }
            }

            pen = RightHandInteractor.interactablesSelected[0] as Pen;
            if (pen != null)
            {
                // Pen의 그리기 가능 함수 호출 
                pen.StartDrawing();
                Debug.Log("Pen is Not NULL");
            }

        }
    }

    // 오른손 물건놓기
    public override void RightGestureExit()
    {

    }
}

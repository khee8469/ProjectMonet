using JJH;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FistGesture : Gesture
{

    
    // 인스펙터 확인용 
    [SerializeField]Pen pen;
    

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    //기능 물감짜기
    public override void LeftGestureEnter()
    {
        PaintBucket paintBucket = null;
        pen = null;

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

                    pen = LeftHandInteractor.interactablesSelected[0] as Pen;
                    if (pen != null)
                    {
                        // Pen의 그리기 가능 함수 호출 
                        pen.StartDrawing();
                        Debug.Log("Pen is Not NULL");
                    }
                    break;
                }
            }
        }
    }

    public override void LeftGestureExit()
    {

    }

    //기능 물건 잡기, 물감짜기
    public override void RightGestureEnter()
    {
        PaintBucket paintBucket = null;
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
            Debug.Log(RightHandInteractor.interactablesSelected.Count+"숫자");
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

                    pen = RightHandInteractor.interactablesSelected[0] as Pen;
                    if (pen != null)
                    {
                        // Pen의 그리기 가능 함수 호출 
                        pen.StartDrawing();
                    }
                    break;
                }
            }
        }
    }

    // 내 손에서 붓이 빠져 나갔을 때 stopdrawing 함수 호출 해줘야함. 
    public override void RightGestureExit()
    {
        
    }
}

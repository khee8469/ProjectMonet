using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;


public class PinchGesture : Gesture
{


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
        Pen pen = null;
        //호버중인 오브젝트중 물감통 찾기
        if (LeftHandInteractor.hasSelection)
        {
            //Debug.Log("레프트 제스쳐 피스트 제스쳐");
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
            //Debug.Log("레프트 제스쳐 피스트 제스쳐22");
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

    public override void LeftGestureExit()
    {

    }



    //기능 물건 잡기, 물감짜기
    public override void RightGestureEnter()
    {
        PaintBucket paintBucket = null;
        Pen pen = null;
        //호버중인 오브젝트중 물감통 찾기
        if (RightHandInteractor.hasSelection)
        {
            //Debug.Log("라이트 제스쳐 피스트 제스쳐33");
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
            //Debug.Log("레프트 제스쳐 피스트 제스쳐44");

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

    public override void RightGestureExit()
    {

    }
}

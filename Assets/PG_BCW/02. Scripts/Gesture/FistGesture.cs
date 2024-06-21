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

    //기능 물감짜기
    public override void LeftGestureEnter()
    {
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
        
    }



    //기능 물건 잡기, 물감짜기
    public override void RightGestureEnter()
    {
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
        
    }
}

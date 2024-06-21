using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGesture : Gesture
{

    //잡은 오브젝트를 놓기용 제스처

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }


    public override void LeftGestureEnter()
    {

    }

    public override void LeftGestureExit()
    {
        Debug.Log("왼손손펴기");
        // 저장된 손 오브젝트 놓기
        if (LeftSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            LeftHandInteractor.interactionManager.SelectExit(LeftHandInteractor, LeftSelectedInteractable);
            LeftSelectedInteractable = null;
        }
    }


    public override void RightGestureEnter()
    {

    }

    public override void RightGestureExit()
    {
        Debug.Log("오른손손펴기");
        // 저장된 손 오브젝트 놓기
        if (RightSelectedInteractable != null)
        {
            // 인터랙션 매니저에 인터랙터가 인터랙터블을 선택 해제하도록 요청
            RightHandInteractor.interactionManager.SelectExit(RightHandInteractor, RightSelectedInteractable);
            RightSelectedInteractable = null;
        }
    }
}

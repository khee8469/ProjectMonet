using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenGesture : Gesture
{
    // Interaction Manager 에 상호작용을 신청하는듯 하다. 

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }


    // Hand tracking 이 진입되고 나가는 부분 인듯하다. 
    public override void LeftGestureEnter()
    {

    }

    public override void LeftGestureExit()
    {

    }




    // hand tracking 이 진입 되고 나가는 부분인듯 하다. 
    public override void RightGestureEnter()
    {

    }

    public override void RightGestureExit()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

public class PinchGesture : Gesture
{
    private PaintBucket paintBucket;

    public XRNode handType;
    private XRHandSubsystem handSubsystem;

    void Start()
    {
        List<XRHandSubsystem> handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetInstances(handSubsystems);
        if (handSubsystems.Count > 0)
        {
            handSubsystem = handSubsystems[0];
        }
    }
    // 위치 변화 감지 필요

    void Update()
    {
        /*if (handSubsystem != null)
        {
            Hand hand;
            if (handSubsystem.TryGetHand(handType, out hand))
            {
                HandJointLocation jointLocation;
                if (hand.TryGetJoint(HandJoint.IndexTip, out jointLocation))
                {
                    Vector3 indexTipPosition = jointLocation.position;
                    Quaternion indexTipRotation = jointLocation.rotation;

                    // 이제 indexTipPosition과 indexTipRotation에 검지손가락 끝의 위치와 회전 정보가 있습니다.
                    Debug.Log("Index Finger Tip Position: " + indexTipPosition);
                    Debug.Log("Index Finger Tip Rotation: " + indexTipRotation);
                }
            }
        }*/
    }



    public override void GestureEnter(XRBaseInteractor interactor)
    {
        //Debug.Log("핀치 제스처");
        paintBucket = interactor.interactablesHovered.FirstOrDefault() as PaintBucket;

        coroutine = StartCoroutine(HandPosition());


        //물감나오는거 함수 호출
        if( paintBucket != null )
        {
            paintBucket.PaintPlay();
        }
    }

    public override void GestureExit(XRBaseInteractor interactor)
    {
        StopCoroutine(coroutine);

        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }


    Coroutine coroutine;
    IEnumerator HandPosition()
    {
        

        yield return new WaitForSeconds(0.1f);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

public class NewBehaviourScript : XRDirectInteractor
{
    XRHandSubsystem handSubsystem;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        if (handSubsystems.Count > 0)
        {
            handSubsystem = handSubsystems[0];
            handSubsystem.updatedHands += OnUpdatedHands;
        }
    }

    //손의 위치가 변할때마다 실행되는 함수
    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints))
        {
            XRHand leftHand = subsystem.leftHand;
            GrabGesture(leftHand);
        }
    }

    void GrabGesture(XRHand hand)
    {
        //손가락 관절을 참조
        XRHandJoint palm = hand.GetJoint(XRHandJointID.Palm);
        XRHandJoint indexTip = hand.GetJoint(XRHandJointID.IndexTip);
        XRHandJoint middleTip = hand.GetJoint(XRHandJointID.MiddleTip);
        XRHandJoint ringTip = hand.GetJoint(XRHandJointID.RingTip);
        XRHandJoint littleTip = hand.GetJoint(XRHandJointID.LittleTip);

        //손가락 관절의 포즈를 가져오면
        if (palm.TryGetPose(out Pose palmPose) && indexTip.TryGetPose(out Pose indexPose) &&
            middleTip.TryGetPose(out Pose middlePose) && ringTip.TryGetPose(out Pose ringPose) &&
            littleTip.TryGetPose(out Pose littlePose))
        {
            //손바닥과 손가락들과의 거리를 비교
            float distanceThreshold = 0.05f; // Define your own threshold for the gesture
            if (Vector3.Distance(palmPose.position, indexPose.position) < distanceThreshold &&
                Vector3.Distance(palmPose.position, middlePose.position) < distanceThreshold &&
                Vector3.Distance(palmPose.position, ringPose.position) < distanceThreshold &&
                Vector3.Distance(palmPose.position, littlePose.position) < distanceThreshold)
            {
                GrabEnterAction();
            }
            else
            {
                GrabExitAction();
            }
        }
    }

    void GrabEnterAction()
    {
        //물체를 잡는다
        SendSelectEnterEvent();
    }

    void GrabExitAction()
    {
        //물체를 잡는다
        SendSelectExitEvent();
    }

    private void SendSelectEnterEvent()
    {
        // 현재 호버링 중인 객체 가져오기
        XRBaseInteractable interactable = GetHoveredInteractable();

        if (interactable != null)
        {
            /*SelectEnterEventArgs args = new SelectEnterEventArgs
            {
                interactableObject = interactable,
                interactorObject = this
            };*/

            interactionManager.SelectEnter(this , interactable);
        }
    }

    private void SendSelectExitEvent()
    {
        // 현재 호버링 중인 객체 가져오기
        XRBaseInteractable interactable = GetHoveredInteractable();

        if (interactable != null)
        {
            /*var args = new SelectEnterEventArgs
            {
                interactableObject = interactable,
                interactorObject = this
            };*/

            interactionManager.SelectExit(this , interactable);
        }
    }

    private XRBaseInteractable GetHoveredInteractable()
    {
        // 현재 호버링 중인 객체를 반환
        // interactablesHovered는 호버링 중인 객체 목록을 반환하는 프로퍼티입니다.
        if (interactablesHovered.Count > 0)
        {
            /*for(int i = 0;  i < interactablesHovered.Count; i++)
            {
                interactablesHovered[i].transform.GetComponent<>
            }*/
            return interactablesHovered[0] as XRBaseInteractable;
        }

        return null;
    }
}

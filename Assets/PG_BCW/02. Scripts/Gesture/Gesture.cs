using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Gesture : MonoBehaviour
{
    [Tooltip("interactionManager에 상호작용 신청할 leftInteractor")]// direct interactor
    [SerializeField]
    private XRBaseInteractor leftHandInteractor;
    public XRBaseInteractor LeftHandInteractor { get { return leftHandInteractor; } }

    [Tooltip("interactionManager에 상호작용 신청할 rightInteractor")]// direct interactor
    [SerializeField]
    private XRBaseInteractor rightHandInteractor;
    public XRBaseInteractor RightHandInteractor { get { return rightHandInteractor; } }

    [Tooltip("왼손 오른손 모션 이벤트를 각각 넣어줘야함")]//각 제스처 오브젝트에
    [SerializeField]
    private StaticGesture staticHandGesture;
    public StaticGesture StaticHandGesture { get { return staticHandGesture; } }

    [Tooltip("왼손 충돌체 끄기")]//Hand Interaction Visual오브젝트에있음
    [SerializeField]
    private HandColliderController lefthandColliderController;
    public HandColliderController LeftHandColliderController { get {  return lefthandColliderController; } }

    [Tooltip("오른손 충돌체 끄기")]//Hand Interaction Visual오브젝트에있음
    [SerializeField]
    private HandColliderController righthandColliderController;
    public HandColliderController RightHandColliderController { get { return righthandColliderController; } }

    public virtual void Awake()
    {
        if (staticHandGesture != null)
            return;
        staticHandGesture = GetComponent<StaticGesture>();
    }

    public virtual void Start()
    {
        //이벤트에 기능 넣기
        staticHandGesture.leftGesturePerformed.AddListener(LeftGestureEnter);
        staticHandGesture.leftGestureEnded.AddListener(LeftGestureExit);
        staticHandGesture.rightGesturePerformed.AddListener(RightGestureEnter);
        staticHandGesture.rightGestureEnded.AddListener(RightGestureExit);
    }


    public abstract void LeftGestureEnter();

    public abstract void LeftGestureExit();

    public abstract void RightGestureEnter();

    public abstract void RightGestureExit();
}

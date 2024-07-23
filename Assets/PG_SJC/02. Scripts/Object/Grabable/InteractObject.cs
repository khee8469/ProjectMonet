using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;
using EPOOutline;

// 상호작용할 오브젝트의 타입
public enum GrabType
{
    None = -1,          // 단순 상호작용 오브젝트 
    Direct,
    DirectRay,
    Ray
}


namespace Jc
{
    /*[RequireComponent(typeof(Outlinable))]*/  // RequireMent로 넣은 스크립트는 이거 꺼주면 알아서 나가는 듯?
    public class InteractObject : XRGrabInteractable, IInteractable
    {
        [Space(5)]
        [Header("---- 컴포넌트 커스텀 ----")]
        [Space(5)]
        [Header("오브젝트의 그랩 타입 (상호작용)")]
        [SerializeField]
        protected GrabType grabType;
        public GrabType GrabType {get { return grabType; } }

        [Header("기본값 true : 반댓손 오브젝트를 SelectExit")]
        [SerializeField]
        protected bool isSingleGrab = true;

        [Header("양손 그랩 오브젝트인지?")]
        [SerializeField]
        protected bool isTwoHanded = false;

        protected bool isGrabbed = false;   // 오브젝트의 그랩 여부

        [SerializeField]
        protected float grabDistanceThreshold;  // 그랩 최대거리
        public float GrabDistanceThreshold  {get { return grabDistanceThreshold; } }

        [SerializeField]
        protected float grabDistance;       // 그랩 시 오브젝트가 위치할 거리
        public float GrabDistance {get { return grabDistance; } }

        [SerializeField]
        protected List<XRBaseInteractor> interactors;
        public List<XRBaseInteractor> Interactors {get { return interactors; } }

        protected bool CheckTwoHanded()
        {
            //return interactors
            return interactors.Count >= 2;
        }

        protected override void Awake()
        {
            base.Awake();
            /*Outlinable outlinable = GetComponent<Outlinable>();
            outlinable.enabled = false; // 일단 꺼주기 --> Hover 되거나 Select 되었을 때만 아웃라인이 발생해야 한다. */
        }


        // 상속하는 자식에서 다양화

        




        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if(isTwoHanded)
            {
                interactors.Add(args.interactableObject as XRBaseInteractor);

                if(CheckTwoHanded())
                {
                    trackPosition = true;
                }
            }



        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (isTwoHanded)
            {
                interactors.Remove(args.interactableObject as XRBaseInteractor);

                if (!CheckTwoHanded())
                {
                    trackPosition = false;
                }
            }
        }

        public float GetDistanceThreshold()
        {
            return grabDistanceThreshold;
        }

        public float GetInteractDistance()
        {
            return grabDistance;
        }

        public bool GetSingleGrab()
        {
            return isSingleGrab;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}

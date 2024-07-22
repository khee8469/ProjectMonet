using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 씬 이동 용 그림에 붙을 Door Script --> DoorID 번호 저장 잘 해주고. 

    public class StageDoor : XRSimpleInteractable, IComparable<StageDoor>, IActivatable, IInteractable
    {
        [Header("Door 관리 ID")]
        [Tooltip("id에 따라 on /off 결정")]
        [SerializeField] private int doorID;

        public int DoorID { get; private set; }

        [SerializeField]
        UnLockController unLockController;  // 각 door 들이 참조할 스테이지 관리 매니저

        [Header("넘어갈 씬 이름")]
        [Tooltip("string으로 관리할 씬 이름")]
        [SerializeField] string SceneName;

        new Rigidbody rigidbody;

        [Tooltip(" stage를 넘어갈 수 있도록 풀어줘야 하는 bool 변수")]
        [SerializeField] private bool isOn = false;

        [SerializeField] private float distance = 5f;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

        }

        // playerPositionTrigger에서 참조해서 door의 layer를 nothing everything 으로 전환해주는 함수
        public void stageOn(bool boolean)
        {
            if(boolean==true)  // true 면 진입한 것 -> 누르지 못하도록 door 닫아버리기.
            {
                this.interactionLayers = 0;
            }
            else
            {
                this.interactionLayers = -1; 
            }
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }

        public int CompareTo(StageDoor other)
        {
            if (other == null) return 1;
            return doorID.CompareTo(other.doorID);
        }

        public void Activate() // 맵이 열려 있으면 TRUE 리턴 / 안 열려 있으면 FLASE 리턴. 
        {
            if (Manager.PlayableData.CanvasData.stageUnlockStatus[doorID])  // doorID가 true 라면 --> 해금되어 있는 상태 
            {
                Manager.Scene.LoadScene(SceneName);
            }
            else
            {
                
            }

        }

        public float GetInteractDistance()
        {
            return 10f;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetDistanceThreshold()
        {
            return distance;
        }

        public bool GetSingleGrab()
        {
            return false;
        }
    }
}



using Jc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class StageDoor : InteractObject , IComparable<StageDoor> , IActivatable
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

        private void Start()
        {
            unLockController = FindObjectOfType<UnLockController>();
            rigidbody =GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            throwOnDetach = false;
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
            //Activate();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            return;
            
        }
        // 할당된 ID 순서대로 정렬 . 
        public int CompareTo(StageDoor other)
        {
            if (other == null) return 1;
            return doorID.CompareTo(other.doorID);
        }

        public void Activate() // 맵이 열려 있으면 TRUE 리턴 / 안 열려 있으면 FLASE 리턴. 
        {
            Debug.Log("Activate 발동");
            // 맵이 열려 있으면 해당 씬 로딩 가능. 
            if (Manager.Chapter.runtimeStageData.stageUnlockStatus.Count > doorID &&
                Manager.Chapter.runtimeStageData.stageUnlockStatus[doorID])
            {
                Manager.Scene.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("해당 스테이지는 잠겨 있습니다.");
            }
        }






        // Static object의 끌려 들어온 coliider를 원 상태로 복구 시켜준다. 
        public void ResetColliderPosition()
        {

        }
    }
}



using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class StageDoor : XRSimpleInteractable , IComparable<StageDoor> , IActivatable
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


        private void Start()
        {
            unLockController = FindObjectOfType<UnLockController>();
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {

            base.OnHoverEntered(args);
            Debug.Log(" 온 호버 엔티드");
        }
        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);

            Debug.Log("온 액티베이트 진입");

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

        public int CompareTo(StageDoor other)
        {
            if (other == null) return 1;
            return doorID.CompareTo(other.doorID);
        }

        public bool Activate() // 맵이 열려 있으면 TRUE 리턴 / 안 열려 있으면 FLASE 리턴. 
        {
            // 맵이 열려 있으면 해당 씬 로딩 가능. 
            if (Manager.Chapter.runtimeStageData.stageUnlockStatus.Count > doorID &&
                Manager.Chapter.runtimeStageData.stageUnlockStatus[doorID])
            {
                Manager.Scene.LoadScene(SceneName);
                return true;
            }
            else
            {
                Debug.Log("해당 스테이지는 잠겨 있습니다.");
                return false;
            }
        }
    }
}



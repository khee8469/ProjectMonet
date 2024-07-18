using Jc;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 씬 이동 용 그림에 붙을 Door Script --> DoorID 번호 저장 잘 해주고. 

    public class StageDoor : XRSimpleInteractable , IComparable<StageDoor> , IActivatable , IInteractable
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

        /*[Tooltip("자신을 숨겨줄 이젤 커버")]
        [SerializeField] GameObject cover;*/

        private void Start()
        {
            
            rigidbody =GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            
        }

        /*public void CoverOff()
        {
            if(cover!=null)
            {
                Debug.Log("커버드 발동");
                cover.gameObject.SetActive(false);
            }
            
        }*/

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

  
        public int CompareTo(StageDoor other)
        {
            if (other == null) return 1;
            return doorID.CompareTo(other.doorID);
        }

        public void Activate() // 맵이 열려 있으면 TRUE 리턴 / 안 열려 있으면 FLASE 리턴. 
        {
            
           /* // 맵이 열려 있으면 해당 씬 로딩 가능. 
            if (Manager.Chapter.runtimeStageData.stageUnlockStatus.Count > doorID &&
                Manager.Chapter.runtimeStageData.stageUnlockStatus[doorID])
            {
                Manager.Scene.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("해당 스테이지는 잠겨 있습니다.");
            }*/
            if (Manager.PlayableData.CanvasData.stageUnlockStatus[doorID])  // doorID가 true 라면 --> 해금되어 있는 상태 
            {
                Manager.Scene.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("해당 스테이지는 잠겨 있습니다.");
            }

        }

        public float GetInteractDistance()
        {
            return 10f;
        }

        public Transform GetTransform()
        {
            throw new NotImplementedException();
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jc
{
    public class Picture : InteractObject
    {
        [Header("에디터 세팅")]
        [Header("전용 소켓")]
        [SerializeField]
        private PictureSocket targetSocket;

        private Vector3 originPos;      // 초기 위치
        private Quaternion originRot;   // 초기 회전 값

        private Coroutine resetRoutine;

        [Header("밸런싱")]
        [SerializeField]
        private bool isEnable = false;  // 활성화 여부 체크

        protected override void Awake()
        {
            base.Awake();
            originPos = transform.position;
            originRot = transform.rotation;
        }

        private void ResetPosition()
        {
            
            transform.position = originPos; 
            transform.rotation = originRot; 
        }

        // Select된 소켓 체크
        private bool CheckSocket(SelectEnterEventArgs args)
        {
            PictureSocket pictureSocket = args.interactorObject as PictureSocket;

            if (pictureSocket == null || pictureSocket == targetSocket)
                return false;

            return true;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            
            // 지정된 소켓이 아닐경우
            if(!CheckSocket(args))
            {
                // Select 취소
                interactionManager.SelectExit(args as IXRSelectInteractor, this);
                // 위칫 값 초기화
                ResetPosition();
            }
            // 지정된 소켓에 들어간 경우
            else
            {
                isEnable = true;
                // 그랩 오브젝트 비활성화
                enabled = false;
            }
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            resetRoutine = StartCoroutine(ResetRoutine());
        }
        
        IEnumerator ResetRoutine()
        {
            // 놓여진 뒤 0.5초 뒤 현재 상태를 체크
            // 지정된 소켓에 위치하지 않은 경우 
            yield return new WaitForSeconds(0.5f);
            if (!isEnable)
            { 
                ResetPosition();
            }
        }
        
    }
}

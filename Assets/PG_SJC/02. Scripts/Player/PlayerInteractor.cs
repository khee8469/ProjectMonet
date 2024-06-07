using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("디버깅 용")]
        [SerializeField]
        private CinemachineVirtualCamera playerVC;

        [Header("VR 핸들러")]
        [SerializeField]
        private XRBaseInteractor leftITR;   // 왼쪽 스틱 인터렉터

        [SerializeField]
        private XRBaseInteractor rightITR;  // 오른쪽 스틱 인터렉터

        private GameObject leftSeletOB;     // 왼손 그랩 오브젝트
        public GameObject LeftSeletOB
        {
            get
            {
                if (leftSeletOB == null)
                    Debug.Log("왼손으로 잡은 오브젝트가 존재하지 않습니다.");
                return leftSeletOB;
            }
            set { leftSeletOB = value; }
        }

        private GameObject rightSeletOB;     // 오른손 그랩 오브젝트
        public GameObject RightSeletOB
        {
            get
            {
                if (rightSeletOB == null)
                    Debug.Log("왼손으로 잡은 오브젝트가 존재하지 않습니다.");
                return rightSeletOB;
            }
            set { rightSeletOB = value; }
        }

        [Header("플레이어 아이템 컨트롤러 (그랩할 수 있는 오브젝트 관리자)")]
        [SerializeField]
        private PlayerItemController itemController;

        private void OnEnable()
        {
            //Manager.Camera.PlayerCameraSetUp(playerVC);

            leftITR.selectEntered.AddListener(OnLeftHandSelectEnter);
            leftITR.selectExited.AddListener(OnLeftHandSelectExit);
            rightITR.selectEntered.AddListener(OnRightHandSelectEnter);
            rightITR.selectExited.AddListener(OnRightHandSelectExit);
        }
        private void OnDisable()
        {
            leftITR.selectEntered.RemoveListener(OnLeftHandSelectEnter);
            leftITR.selectExited.RemoveListener(OnLeftHandSelectExit);
            rightITR.selectEntered.RemoveListener(OnRightHandSelectEnter);
            rightITR.selectExited.RemoveListener(OnRightHandSelectExit);
        }

        #region VR 스틱 상호작용 콜백
        private void OnLeftHandSelectEnter(SelectEnterEventArgs args)
        {
            XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            LeftSeletOB = grabbedObject.gameObject;
            Debug.Log($"왼손 그랩 : {LeftSeletOB}");
        }
        private void OnLeftHandSelectExit(SelectExitEventArgs args)
        {
            XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            LeftSeletOB = null;
        }
        private void OnRightHandSelectEnter(SelectEnterEventArgs args)
        {
            XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            RightSeletOB = grabbedObject.gameObject;
            Debug.Log($"오른손 그랩 : {LeftSeletOB}");
        }
        private void OnRightHandSelectExit(SelectExitEventArgs args)
        {
            XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            RightSeletOB = null;
        }
        #endregion
    }
}

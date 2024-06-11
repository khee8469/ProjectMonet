using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("VR 핸들러")]
        [SerializeField]
        private XRBaseInteractor leftCTR;   // 왼쪽 스틱 컨트롤러

        [SerializeField]
        private XRBaseInteractor rightCTR;  // 오른쪽 스틱 컨트롤러

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

        private void Awake()
        {
        }
        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
        }

        #region VR 스틱 상호작용 콜백

        #region 왼손 그립
        private void OnLeftHandGripEnter(InputValue value)
        {
            //XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            //if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            //LeftSeletOB = grabbedObject.gameObject;
            //Debug.Log($"왼손 그랩 : {LeftSeletOB}");
        }
        private void OnLeftHandGripExit(InputValue value)
        {
            //XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            //if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            //LeftSeletOB = null;
        }
        #endregion

        #region 오른손 그립
        private void OnRightHandGripEnter(InputValue value)
        {
            //XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            //if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            //RightSeletOB = grabbedObject.gameObject;
            //Debug.Log($"오른손 그랩 : {LeftSeletOB}");
        }
        private void OnRightHandGripExit(InputValue value)
        {
            //XRGrabInteractable grabbedObject = args.interactableObject as XRGrabInteractable;
            //if (grabbedObject == null) return;  // 그랩 오브젝트 예외처리
            //RightSeletOB = null;
        }
        #endregion

        #endregion
        // 퀘스트 추가 후 수정 예정
    }
}

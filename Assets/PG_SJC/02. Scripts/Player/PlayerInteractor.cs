using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private PlayerTrigger trigger;

        [SerializeField]
        private PlayerControllerCallback controllerCallback;

        [SerializeField]
        private PlayerQuestController questController;

        [SerializeField]
        private GameObject popUpCanvas; // 인벤토리 / 퀘스트

        private Transform mainCam; // 메인 카메라

        [Space(5)]
        [Header("밸런싱")]
        [Tooltip("퀘스트 NPC")]
        [SerializeField]
        private NPC nearNPC;
        [Tooltip("상호작용만하는 NPC")]
        [SerializeField]
        private InteractorNPC nearInNPC;


        private bool isEnabledPopup = false;   // 팝업 활성화
        private Transform camTr;               // 메인 카메라 트랜스폼

        public UnityAction OnEndInteract;   // NPC와 상호작용 해제


        private void Awake()
        {
            mainCam = Camera.main.transform;
        }

        private void OnEnable()
        {
            // 퀘스트 npc 지정
            trigger.OnNPCEnter += OnEnterNPC;
            trigger.OnNPCExit += OnExitNPC;
            // 상호작용 npc 지정
            trigger.OnInNPCEnter += OnEnterInteractorNPC;
            trigger.OnInNPCExit += OnExitInteractorNPC;

            camTr = Camera.main.transform;

            controllerCallback.leftMenuBTNRef.action.performed += OnPopUpCanvas;    // 인벤토리/퀘스트 버튼 등록
            //controllerCallback.debugMenuBTNRef.action.performed += OnPopUpCanvas;   // 디버그 인벤토리/퀘스트 버튼 등록

            controllerCallback.leftTriggerRef.action.performed += OnInteractNPC;    // NPC 상호작용 등록
        }
        private void OnDisable()
        {
            // 퀘스트 npc 지정 해제
            trigger.OnNPCEnter -= OnEnterNPC;
            trigger.OnNPCExit -= OnExitNPC;
            // 상호작용 npc 지정 해제
            trigger.OnInNPCEnter -= OnEnterInteractorNPC;
            trigger.OnInNPCExit -= OnExitInteractorNPC;

            controllerCallback.leftMenuBTNRef.action.performed -= OnPopUpCanvas;
            controllerCallback.leftTriggerRef.action.performed -= OnInteractNPC;
        }

        // 퀘스트 NPC Trigger Enter 콜백
        private void OnEnterNPC(NPC target)
        {
            // 기존에 충돌한 NPC 할당해제
            if (nearNPC != null)
                nearNPC = null;

            // 가장 가까운 NPC 재할당 
            nearNPC = target;
        }
        // 퀘스트 NPC Trigger Exit 콜백
        private void OnExitNPC(NPC target)
        {
            if (target == nearNPC)
            {
                OnEndInteract?.Invoke();
                OnEndInteract -= target.OnExitInteract;
                nearNPC = null;
            }
        }


        // 상호작용 NPC Trigger Enter 콜백
        private void OnEnterInteractorNPC(InteractorNPC target)
        {
            // 기존에 충돌한 NPC 할당해제
            if (nearInNPC != null)
                nearInNPC = null;

            // 가장 가까운 NPC 재할당 
            nearInNPC = target;
        }
        // 상호작용 NPC Trigger Exit 콜백
        private void OnExitInteractorNPC(InteractorNPC target)
        {
            if (target == nearInNPC)
            {
                OnEndInteract?.Invoke();
                OnEndInteract -= target.OnExitInteract;
                nearInNPC = null;
            }
        }




        // NPC 상호작용 콜백
        private void OnInteractNPC(InputAction.CallbackContext context)
        {
            if (Manager.Inventory.isEnable) return;   // 팝업이 열려있는 경우 
            if (nearNPC == null && nearInNPC == null) return; // 근처 NPC가 없는 경우
            //퀘스트 npc면
            if (nearNPC != null)
            {
                nearNPC.OnInteract(questController);
                OnEndInteract += nearNPC.OnExitInteract;  // 상호작용 해제 등록
            }
            //상호작용 npc면
            else if(nearInNPC != null)
            {
                nearInNPC.OnTalkInteractor();
                OnEndInteract += nearInNPC.OnExitInteract; 
                //nearInNPC.transform.rotation = nearInNPC.transform.LookAt(transform.position);
            }
        }



        private void OnPopUpCanvas(InputAction.CallbackContext context)
        {
            Debug.Log("메뉴 버튼 클릭");
            isEnabledPopup = !isEnabledPopup;
            OnPopUp(isEnabledPopup);
        }

        private void OnPopUp(bool isEnable)
        {

            // 활성화 시 메인 카메라 트랜스폼을 추적
            if (isEnable)
                Manager.UI.OpenInfoGroup();
            else
                Manager.UI.CloseInfoGroup();
        }
    }
}

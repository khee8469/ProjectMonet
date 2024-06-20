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
        [Header("에디터 세팅")]
        [SerializeField]
        private PlayerTrigger trigger;

        [SerializeField]
        private PlayerControllerCallback controllerCallback;

        [SerializeField]
        private PlayerQuestController questController;

        [SerializeField]
        private GameObject popUpCanvas; // 인벤토리 / 퀘스트

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private NPC nearNPC;

        private bool isEnabledPopup = false;   // 팝업 활성화
        private Transform camTr;               // 메인 카메라 트랜스폼

        private void OnEnable()
        {
            trigger.OnNPCEnter += OnEnterNPC;
            trigger.OnNPCExit += OnExitNPC;

            camTr = Camera.main.transform;

            controllerCallback.leftMenuBTNRef.action.performed += OnPopUpCanvas;    // 인벤토리/퀘스트 버튼 등록
            //controllerCallback.debugMenuBTNRef.action.performed += OnPopUpCanvas;   // 디버그 인벤토리/퀘스트 버튼 등록

            controllerCallback.leftTriggerRef.action.performed += OnInteractNPC;    // NPC 상호작용 등록
        }
        private void OnDisable()
        {
            trigger.OnNPCEnter -= OnEnterNPC;
            trigger.OnNPCExit -= OnExitNPC;

            controllerCallback.leftMenuBTNRef.action.performed -= OnPopUpCanvas;
            controllerCallback.leftTriggerRef.action.performed -= OnInteractNPC;
        }

        // NPC Trigger Enter 콜백
        private void OnEnterNPC(NPC target)
        {
            // 기존에 충돌한 NPC 할당해제
            if (nearNPC != null)
                nearNPC = null;

            // 가장 가까운 NPC 재할당 
            nearNPC = target;
        }
        // NPC Trigger Exit 콜백
        private void OnExitNPC(NPC target)
        {
            if (target == nearNPC)
                nearNPC = null;
        }

        // NPC 상호작용 콜백
        private void OnInteractNPC(InputAction.CallbackContext context)
        {
            // 추후 조건추가 (메뉴버튼이 열려있을 경우 우선순위에서 제외됨.)
            if (nearNPC == null) return;

            nearNPC.OnInteract(questController);
        }

        private void OnPopUpCanvas(InputAction.CallbackContext context)
        {
            Debug.Log("메뉴 버튼 클릭");
            isEnabledPopup = !isEnabledPopup;
            OnPopUp(isEnabledPopup);
        }

        private void OnPopUp(bool isEnable)
        {            
            JJH.Manager.Inventory.isEnable = isEnable;
            popUpCanvas.SetActive(isEnable);
        }
    }
}

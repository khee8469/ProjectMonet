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

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private NPC nearNPC;

        private void OnEnable()
        {
            trigger.OnNPCEnter += OnEnterNPC;
            trigger.OnNPCExit += OnExitNPC;

            controllerCallback.leftTriggerRef.action.performed += OnInteractNPC;    // NPC 상호작용 등록
        }
        private void OnDisable()
        {
            trigger.OnNPCEnter -= OnEnterNPC;
            trigger.OnNPCExit -= OnExitNPC;

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

            nearNPC.OnInteract(transform.position);
        }
    }
}

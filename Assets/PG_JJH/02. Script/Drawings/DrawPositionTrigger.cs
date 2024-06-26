using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 플레이어의 이동을 방지 + 고개는 돌릴 수 있음. 
    // 이 상태에서만 그림을 그릴 수 있음. 
    public class DrawPositionTrigger : InteractObject , IActivatable
    {
        [SerializeField] private Transform playerMovePos;
        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private bool drawOn = false;


        // 여기도 결국 트리거 키로 진입을 해야 하기 때문에... 트리거 되서 진입해야함.

        [SerializeField] GameObject player;
        private CharacterController characterController; // 플레이어의 캐릭터 컨트롤러. 

        public void Activate()
        {
            if(player != null)
            {

                Debug.Log("Activate 발동");
                // 캐릭터 컨트롤러를 잠시 끄고 위치를 이동시킨다.
                characterController = player.GetComponent<CharacterController>();
                characterController.enabled = false;
                player.transform.position = playerMovePos.position; // 정해진 위치로 플레이어 이동
                characterController.enabled = true;

            }


            // 추가로 bool 변수에 따라 그림 그리기 진입 한 상태 / 진입 안 한 상태 구분해서 조작 중지를 나눠준다. 
        }

        public void ResetColliderPosition()
        {

        }

        // 플레이어가 들어오면 --> pos 로 이동시키고 강제 고정 
        private void OnTriggerEnter(Collider other)
        {
            if(Extension.Contain(playerLayer , other.gameObject.layer))
            {
                player = other.gameObject; // player 참조 시작. 
            }

            if(other.gameObject.CompareTag("Player"))
            {
                player =other.gameObject;
            }

        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            return;
            
        }



    }

}


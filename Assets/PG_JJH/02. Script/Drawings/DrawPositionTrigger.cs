using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using JJH;

namespace JJH
{
    // 플레이어의 이동을 방지 + 고개는 돌릴 수 있음. 
    // 이 상태에서만 그림을 그릴 수 있음. 
    public class DrawPositionTrigger : XRSimpleInteractable, IActivatable , IInteractable
    {
        [SerializeField] private Transform playerMovePos;
        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private GameObject player;

        [SerializeField]private CharacterController characterController; // 플레이어의 캐릭터 컨트롤러. 

        private bool isTrigger = false;

        [Tooltip("플레이어의 이동을 방지해줄 move provider")]
        [SerializeField] private DynamicMoveProvider move;

        [Tooltip("player의 moveSpeed를 저장해줄 변수")]
        private float originalSpeed;

        [Tooltip("플레이어를 돌려 줄 위치 ")]
        [SerializeField] private Transform returnPos;

        [Tooltip("해당 트리거가 진입되는 그림 door ")]
        [SerializeField] private StageDoor door;


        public void Activate()
        {

            if (player != null)
            {
                // 캐릭터 컨트롤러를 잠시 끄고 위치를 이동시킨다.
                //characterController = player.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    characterController.enabled = false;
                }
                //move = player.GetComponentInChildren<DynamicMoveProvider>();

                if (move != null)
                {
                    if (isTrigger == false)
                    {
                        
                        isTrigger = true;
                        originalSpeed = move.moveSpeed;
                        player.transform.rotation = playerMovePos.rotation;
                        player.transform.position = playerMovePos.position; // 정해진 위치로 플레이어 이동
                        move.moveSpeed = 0;
                        //characterController.radius = 0.1f;

                        if(door!=null)
                        {
                            Debug.Log($"도어 TRIGGER -> 진입 Door.false");
                            door.stageOn(false); // 트리거 진입하면 activate 발동하지 않는다.
                        }                   
                    }
                    else
                    {
                        isTrigger = false;
                        move.moveSpeed = originalSpeed;
                        player.transform.position = returnPos.position; // 정해진 위치로 플레이어 이동
                        //characterController.radius = 0.2f;
                        if (door!=null)
                        {
                            Debug.Log($"도어 TRIGGER -> 진입 Door.true");
                            door.stageOn(true); // 트리거 벗어나면 다시 activate가 발동된다. 
                        }
                        
                    }
                }
                if (characterController != null)
                {                    
                    characterController.enabled = true;
                }

                move.enabled = false;
                move.enabled = true; // 이동 제공자 초기화

            }
            // 추가로 bool 변수에 따라 그림 그리기 진입 한 상태 / 진입 안 한 상태 구분해서 조작 중지를 나눠준다. 
            
        }      
        // 플레이어가 들어오면 --> pos 로 이동시키고 강제 고정 

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);       
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {           
            base.OnSelectEntered(args);           
        }

        public float GetInteractDistance()
        {
            return 10f;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetDistanceThreshold()
        {
            return 0; 
        }
    }

}


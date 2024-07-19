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

        [SerializeField] private bool drawOn = false;
        // 여기도 결국 트리거 키로 진입을 해야 하기 때문에... 트리거 되서 진입해야함.

        [SerializeField] private GameObject player;
        private CharacterController characterController; // 플레이어의 캐릭터 컨트롤러. 

        private bool isTrigger = false;

        [Tooltip("플레이어의 이동을 방지해줄 move provider")]
        [SerializeField] private DynamicMoveProvider move;

        [Tooltip("player의 moveSpeed를 저장해줄 변수")]
        private float originalSpeed;

        /*[Tooltip("플레이어의 진입 시 위치를 저장해 둘 변수")]
        private Vector3 playerOiriginPos;*/

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
                    Debug.Log("캐컨 널 아님");
                    characterController.enabled = false;

                }
                //move = player.GetComponentInChildren<DynamicMoveProvider>();

                if (move != null)
                {
                    if (isTrigger == false)
                    {
                        Debug.Log("진입");
                        isTrigger = true;
                        originalSpeed = move.moveSpeed;
                        player.transform.rotation = playerMovePos.rotation;
                        player.transform.position = playerMovePos.position; // 정해진 위치로 플레이어 이동
                        move.moveSpeed = 0;
                        characterController.radius = 0.1f;
                        if(door!=null)
                        {
                            door.stageOn(false); // 트리거 진입하면 activate 발동하지 않는다.
                        }
                        
                    }
                    else
                    {
                        Debug.Log("탈출");
                        isTrigger = false;
                        move.moveSpeed = originalSpeed;
                        player.transform.position = returnPos.position; // 정해진 위치로 플레이어 이동
                        characterController.radius = 0.2f;
                        if (door!=null)
                        {
                            door.stageOn(true); // 트리거 벗어나면 다시 activate가 발동된다. 
                        }
                        
                    }
                }
                if (characterController != null)
                {
                    Debug.Log("캐컨 널 아님2");
                    characterController.enabled = true;
                }

                move.enabled = false;
                move.enabled = true; // 이동 제공자 초기화

            }
            // 추가로 bool 변수에 따라 그림 그리기 진입 한 상태 / 진입 안 한 상태 구분해서 조작 중지를 나눠준다. 
            
        }
        public void ResetColliderPosition()
        {

        }

        // 플레이어가 들어오면 --> pos 로 이동시키고 강제 고정 
        private void OnTriggerEnter(Collider other)
        {
            /*if (Extension.Contain(playerLayer, other.gameObject.layer))
            {
                player = other.gameObject; // player 참조 시작. 
            }*/
            /*if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject;
            }*/
            // 이거 그냥 태그로 하자. 

        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
            Debug.Log("포지션 액티베이트");
            //Activate();
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
            throw new System.NotImplementedException();
        }
    }

}


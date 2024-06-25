using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    // 플레이어의 이동을 방지 + 고개는 돌릴 수 있음. 
    // 이 상태에서만 그림을 그릴 수 있음. 
    public class DrawPositionTrigger : MonoBehaviour , IActivatable
    {
        [SerializeField] private Transform playerMovePos;
        [SerializeField] private LayerMask playerLayer; 

        // 여기도 결국 트리거 키로 진입을 해야 하기 때문에... 트리거 되서 진입해야함.

        GameObject player;


        private void Start()
        {
            
        }



        // 플레이어가 들어오면 --> pos 로 이동시키고 강제 고정 
        private void OnTriggerEnter(Collider other)
        {
            if(Extension.Contain(playerLayer , other.gameObject.layer))
            {
                player = other.gameObject; // player 참조 시작. 
            }

        }

        // exit 했을 때 굳이 참조를 잃어버릴 필요는 없을 것 같은데... 

        public bool Activate()
        {
            return false;
        }

        // 여기서 상호작용한 플레이어의 트리거 키를 
        private void EnterDrawing()
        {            
            //  player가 참조 되면 이제 바로 앞으로 이동 시키고 고정 시키기. 
            if(player != null)
            {

            }
        }


        



    }

}


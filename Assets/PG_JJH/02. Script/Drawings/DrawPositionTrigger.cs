using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    // 플레이어의 이동을 방지 + 고개는 돌릴 수 있음. 
    // 이 상태에서만 그림을 그릴 수 있음. 
    public class DrawPositionTrigger : MonoBehaviour
    {
        [SerializeField] private Transform playerMovePos;


        private void Start()
        {
            
        }



        // 플레이어가 들어오면 --> pos 로 이동시키고 강제 고정 
        private void OnTriggerEnter(Collider other)
        {
            
        }
    }

}


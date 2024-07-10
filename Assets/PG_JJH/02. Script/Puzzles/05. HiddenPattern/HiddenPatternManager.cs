using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;

namespace JJH
{
    public class HiddenPatternManager : PaintRewardPuzzle
    {
        // 페인트 5번 퍼즐 

        [Header("원반 오브젝트 모음")]
        [Tooltip("원반 오브젝트들 ")]
        [SerializeField]
        private List<HiddenPatternObject> objects;

        [Tooltip("랜턴 참조 필요하다.")]
        [SerializeField] private HiddenPatternflashlight flashLight;

        [Tooltip("자신의 콜라이더 --> 나가면 랜턴 원 위치 복귀 한다.")]
        [SerializeField] private new Collider collider;

        private void Start()
        {
            
        }


        // 등대 밖으로 랜턴이 나갔을 시 원위치 복귀
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("FlashLight"))
            {
                flashLight.FlashLightReturn(); // 랜턴 원 위치 
            }
        }

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();

            for(int i=0;i<objects.Count;i++)
            {
                objects[i].collider.enabled = false; // 모든 오브젝트의 콜라이더 꺼주기.
            }

            // 톱니바퀴가 인벤토리로 자동 지급 되어야 한다. 
            Debug.Log("일단 문양 퍼즐 완료함!");


        }




    }

}


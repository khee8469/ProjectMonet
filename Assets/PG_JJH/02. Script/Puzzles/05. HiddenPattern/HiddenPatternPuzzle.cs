using Jc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 퍼즐 매니저
    public class HiddenPatternPuzzle : PaintRewardPuzzle
    {
        // 페인트 5번 퍼즐 

        [Header("원반 오브젝트 모음")]
        [Tooltip("원반 오브젝트들 ")]
        [SerializeField]
        private List<HiddenPatternObject> objects;

        [Tooltip("랜턴 참조 필요하다.")]
        [SerializeField] private HiddenPatternflashlight flashLight;

        [Tooltip("플레이어 레이어")]
        [SerializeField] private LayerMask layerMask;

        [Tooltip("trigger 되면 무조건 꺼줄 moon panel")]
        [SerializeField] private GameObject moonPanel; 
        
        private void Start()
        {

        }

        // 등대 밖으로 랜턴이 나갔을 시 원위치 복귀
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("FlashLight"))
            {
                Debug.Log("충돌");

                // 그냥 멀리 날아가면 돌아오게?? 잡고 있는 중 체크 ㄴㄴ

                if(flashLight.firstInteractorSelecting != null) // NULL 이 아닐 때만 하는게 맞나? 
                {
                    Debug.Log("퍼스트 셀렉팅이 널이 아님");
                    flashLight.interactionManager.SelectExit(flashLight.firstInteractorSelecting as IXRSelectInteractor, flashLight as IXRSelectInteractable);
                }

                flashLight.FlashLightReturn(); // 랜턴 원 위치 
            }


            if(other.gameObject.CompareTag("Player"))
            {
                moonPanel.gameObject.SetActive(false);
            }



        }

        public override void OnClearPuzzle() // 온 클리어 퍼즐 --> 인스펙터 에서 할당해도 된다! 
        {
            base.OnClearPuzzle();
        }




    }

}


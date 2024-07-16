using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
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

        [Tooltip("자신의 콜라이더 --> 나가면 랜턴 원 위치 복귀 한다.")]
        [SerializeField] private new Collider collider;

        private void Start()
        {
            
        }


        // 등대 밖으로 랜턴이 나갔을 시 원위치 복귀
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("FlashLight"))
            {
                flashLight.interactionManager.SelectExit(flashLight.firstInteractorSelecting as IXRSelectInteractor, flashLight as IXRSelectInteractable);
                flashLight.FlashLightReturn(); // 랜턴 원 위치 
            }
        }

        public override void OnClearPuzzle() // 온 클리어 퍼즐 --> 인스펙터 에서 할당해도 된다! 
        {
            base.OnClearPuzzle();
        }




    }

}


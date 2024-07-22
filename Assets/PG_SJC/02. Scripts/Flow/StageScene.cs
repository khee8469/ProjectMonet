using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Jc;

namespace JJH
{
    public class StageScene : BaseScene
    {
        private ColorAdjustments colorAdjustments;

        // SceneID 맞춰서 포스트 프로세싱 적용 
        [SerializeField] private int SceneID;
        [SerializeField] Volume globalVolume;
        [Tooltip("맵이 컬러로 변했으면 더 이상 로컬 카메라가 의미가 없어지므로 로컬 카메라 꺼주기.")]
        [SerializeField] Camera localCamera;

        // 로딩 루틴 별로 카메라 찾아서 포스트 프로세싱 조절 해줄 것 

        // 임시 체크
    
        public override IEnumerator LoadingRoutine()
        {
            Debug.Log("챕터1 씬 로딩루틴 진행");

            // Volume 하나에 뭉쳐놓는게 낫지 어차피 여러 기능 쓸 거니까 그냥 volume을 찾자.

            if (ChapterManager.is_Colored[SceneID] == true) // true 라면 흑백효과 풀기. --> 챕터1 이 0 번 ? 
            {
                if (globalVolume != null)
                {
                    if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                    {
                        colorAdjustments.saturation.value = 0f; // 0 이 흑백 해제.
                        if(localCamera != null)
                        {
                            localCamera.gameObject.SetActive(false); // 로컬 카메라 꺼주기. --> 최적화 
                        }
                    }
                }
            }
            else // true가 되지 않은 상태라면 흑백효과 그대로 적용 
            {
                if (globalVolume != null)
                {
                    if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                    {
                        colorAdjustments.saturation.value = -100f; // -100 이 흑백효과 
                    }
                }
            }

            yield return null;
        }
    }
}

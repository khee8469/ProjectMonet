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

        [Tooltip("모네풍 셰이더 적용을 위한 forward renderer data")]
        [SerializeField] private UniversalRendererData data;

        // 모네풍 셰이더 Featrue 이름 
        private const string renderFeatureName = "OilPaint";


        // 로딩 루틴 별로 카메라 찾아서 포스트 프로세싱 조절 해줄 것 

        // 임시 체크

        private void Awake()
        {
            ChangeFeature(false); // 일단 시작할 때 꺼주기. 
            Debug.Log("씬 전환 Awake");
        }

        public override IEnumerator LoadingRoutine()
        {
            Debug.Log("씬 전환 로딩 루틴");

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

                // 그림이 완성된 상태로 들어오게 되면 모네풍 셰이더 On 해줄것.

                ChangeFeature(true);

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

        private void ChangeFeature(bool isEnabled)
        {
            if (data != null) // 포워드 렌더러가 Null이 아니라면 모네풍 적용 
            {
                foreach (var feature in data.rendererFeatures)
                {
                    if (feature.name == renderFeatureName)
                    {
                        feature.SetActive(isEnabled);
                    }
                }
            }
        }


    }
}

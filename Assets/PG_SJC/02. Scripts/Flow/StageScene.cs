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
        [Tooltip("맵이 컬러로 변했으면 더 이상 로컬 카메라가 의미가 없어지므로 로컬 카메라 꺼주기???.")]
        [SerializeField] Camera localCamera;

        [Tooltip("모네풍 셰이더 적용을 위한 forward renderer data")]
        [SerializeField] private UniversalRendererData data;
        // 모네풍 셰이더 Featrue 이름 
        private const string renderFeatureName = "OilPaint";

        /*[Tooltip("그림 완성 시에 켜 줄 ui 셰이더 방지용 카메라")]
        [SerializeField]
        private Camera uiCamera;*/


        // 로딩 루틴 별로 카메라 찾아서 포스트 프로세싱 조절 해줄 것 

        // 임시 체크

        private void Awake()
        {
            //uiCamera.gameObject.SetActive(false); // 기본적으로는 꺼준다. 
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
                            // local 카메라를 꺼주는 대신에 포스트프로세싱을 안받게 해서 ui / slot 등은 제대로 나오도록 해야함.
                            // 어쨋든 지금 글로벌 블룸도 everything을 전부 culling 해주고 있기 때문에 
                            // 아니면 ui 전용 not postprocessing 용도를 만들어서
                            // local 꺼주고 ui용 카메라 켜주고 하는 식도 나쁘지 않음
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

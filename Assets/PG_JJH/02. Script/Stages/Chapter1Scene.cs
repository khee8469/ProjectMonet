using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace JJH
{
    public class Chapter1Scene : BaseScene
    {
        private ColorAdjustments colorAdjustments;

        [SerializeField] private int SceneID = 0;

        // 로딩 루틴 별로 카메라 찾아서 포스트 프로세싱 조절 해줄 것 
        public override IEnumerator LoadingRoutine()
        {
            Debug.Log("챕터1 씬 로딩루틴 진행");


            // Volume 하나에 뭉쳐놓는게 낫지 어차피 여러 기능 쓸 거니까 그냥 volume을 찾자.
            Volume globalVolume = GameObject.FindObjectOfType<Volume>()?.GetComponent<Volume>();

            if (ChapterManager.is_Colored[SceneID] == true) // true 라면 흑백효과 풀기. --> 챕터1 이 0 번 ? 
            {
                if (globalVolume != null)
                {
                    if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                    {
                        colorAdjustments.saturation.value = 0f; // 0 이 흑백 해제.
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



            //Manager.Game.ChangeScene(); // yield return 크게 잡아서 start 보다 늦는데 어째서 slot이 null인지?? 
            yield return null;
        }

    }

}


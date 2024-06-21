using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace JJH
{
    public class Chapter2Scene : BaseScene
    {
        private ColorAdjustments colorAdjustments;
        [SerializeField] private int SceneID = 1; // 나중에 요거로 수정하는것도 나쁘지않어


        public override IEnumerator LoadingRoutine()
        {
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

            yield return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Manager.Scene.LoadScene("LobbyScene");

            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChapterManager.Instance.UnlockStage(2, true);

            }
        }
    }

}


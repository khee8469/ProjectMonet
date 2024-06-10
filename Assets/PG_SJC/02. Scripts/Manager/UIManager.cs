using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("유저 메인 캔버스")]
        [SerializeField]
        private Canvas mainCanavas;

        [Header("페이드 이미지")]
        [SerializeField]
        private Image fadeImage;

        private Color32 fadeInColor = new Color32(0, 0, 0, 1);
        private Color32 fadeOutColor = new Color32(0, 0, 0, 0);

        private void OnEnable()
        {
            Camera renderCamera = Camera.main;
            if (renderCamera == null) return;

            mainCanavas.worldCamera = renderCamera;
        }

        public IEnumerator FadeInRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color32.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }

            fadeImage.color = fadeInColor;
            yield return null;
        }
        public IEnumerator FadeOutRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color32.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }

            fadeImage.color = fadeOutColor;
            yield return null;
        }
    }
}
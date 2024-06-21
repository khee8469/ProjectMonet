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

        private void OnEnable()
        {
            Debug.Log("UIManager Enable");

            Camera renderCamera = Camera.main;
            if (renderCamera == null) return;

            mainCanavas.worldCamera = renderCamera;
        }

        // 페이드 인
        public IEnumerator FadeInRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            Color fadeInColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
            Color fadeOutColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
            
            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }
        }
        
        // 페이드 아웃
        public IEnumerator FadeOutRoutine(float fadeTime = 0f)
        {
            float rate = 0f;
            Color fadeInColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
            Color fadeOutColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);

            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }
        }
    }
}
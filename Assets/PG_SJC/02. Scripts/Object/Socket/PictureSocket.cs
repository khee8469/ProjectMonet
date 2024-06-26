using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 커스텀 소켓 (소켓 인터렉터 x)
    public class PictureSocket : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spr;

        private Color startColor;
        private Color endColor;

        private bool isHighlight = false;

        private bool isFadeIn = true;
        private float rate = 0f;

        private void Awake()
        {
            startColor = new Color(0, 0, 0, 0);
            endColor = new Color(0, 0, 0, 0.5f);
        }

        public void OnHighlight()
        {
            isHighlight = true;
        }
        public void OffHighlight()
        {
            isHighlight = false;
        }

        private void Update()
        {
            if(isHighlight)
            {
                rate += Time.deltaTime;
                if (rate > 1f)
                { 
                    isFadeIn = !isFadeIn;
                    rate = 0f;
                }

                spr.color = Color.Lerp(isFadeIn ? startColor : endColor,
                    isFadeIn ? endColor : startColor,
                    rate);
            }
        }
    }
}
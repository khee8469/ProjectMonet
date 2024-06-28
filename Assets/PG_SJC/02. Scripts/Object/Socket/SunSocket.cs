using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class SunSocket : CustomSocket
    {
        [Header("태양 오브젝트")]
        [SerializeField]
        private SunObject targetSun;

        [SerializeField]
        private float targetScale;      // 타깃 스케일

        [SerializeField]
        private float scaleThreshold;   // 스케일 임계치

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            if (interactable is not SunObject)
                return false;

            if (interactable.transform.localScale.x + scaleThreshold < targetScale 
                || interactable.transform.localScale.x - scaleThreshold > targetScale)
                return false;

            return base.CanHover(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if()
        }
    }
}

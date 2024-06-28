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
        private PuzzleManager puzzleManger;

        [SerializeField]
        private float targetScale;      // 타깃 스케일

        [SerializeField]
        private float scaleThreshold;   // 스케일 임계치

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            if (interactable is not SunObject)
                return false;

            if (targetScale + scaleThreshold < interactable.transform.localScale.x
                || targetScale - scaleThreshold > interactable.transform.localScale.x)
                return false;

            return base.CanHover(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            SunObject obj = args.interactableObject as SunObject;
            if (obj == null) return;

            obj.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            obj.transform.position = attachTransform.position;
            obj.ActiveObject();
            // 퍼즐 클리어
        }
    }
}

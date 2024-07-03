using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class SunSocket : CustomSocket, IPuzzleable
    {
        [Header("태양 오브젝트")]
        [SerializeField]
        private SunObject targetSun;
        [SerializeField]
        private SphereCollider col;
        [SerializeField]
        private PuzzleManager puzzle;

        [SerializeField]
        private float targetScale;      // 타깃 스케일

        [SerializeField]
        private float scaleThreshold;   // 스케일 임계치

        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }
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
            if (obj.transform.localScale.x < targetScale - scaleThreshold
                || obj.transform.localScale.x > targetScale + scaleThreshold)
                return;

            obj.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            obj.transform.position = attachTransform.position;
            obj.ActiveObject();
            // 퍼즐 클리어
        }

        public void ActiveSetting()
        {
            col.enabled = true;
        }

        public void CompleteSetting()
        {
            Destroy(targetSun.gameObject);
            // 0 글자 활성화
        }

        public void DisActiveSetting()
        {
            col.enabled = false;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

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
        private Image oImage;   // O 이미지 (M'o'net)


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
            return base.CanHover(interactable);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            SunObject obj = args.interactableObject as SunObject;
            if (obj == null)
                return;
            if (obj.transform.localScale.x < targetScale - scaleThreshold
                || obj.transform.localScale.x > targetScale + scaleThreshold)
                return;

            obj.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            obj.transform.position = attachTransform.position;
            obj.ActiveObject();
            // 퍼즐 클리어
            puzzle.OnClearPuzzle();
            StartCoroutine(ImageFadeRoutine());
        }
        public void ActiveSetting()
        {
            if (col != null)
                col.enabled = true;
        }

        public void CompleteSetting()
        {
            targetSun.IsLoaded = true;
            Destroy(targetSun.gameObject);
            puzzle.OnClearPuzzle();
            // 0 글자 활성화
            oImage.color = new Color(1, 1, 1, 1);
        }

        public void DisActiveSetting()
        {
            if (col != null)
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

        IEnumerator ImageFadeRoutine()
        {
            float rate = 0f;
            Color startColor = new Color(1f, 1f, 1f, 0f);
            Color endColor = new Color(1f, 1f, 1f, 1f);

            while (rate < 1)
            {
                rate += Time.deltaTime / 2f;
                oImage.color = Color.Lerp(startColor, endColor, rate);
                yield return null;
            }

            oImage.color = endColor;
        }
    }
}

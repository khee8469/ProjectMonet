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

        [Header("모네 팜플렛 메시렌더러")]
        [SerializeField]
        private MeshRenderer proceedRenderer;
        [SerializeField]
        private MeshRenderer clearRenderer;

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
            col.enabled = true;
        }

        public void CompleteSetting()
        {
            targetSun.IsLoaded = true;
            Destroy(targetSun.gameObject);
            puzzle.OnClearPuzzle();

            proceedRenderer.gameObject.SetActive(false);
            clearRenderer.gameObject.SetActive(true);
            clearRenderer.sharedMaterial.color = Color.white;
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

        IEnumerator ImageFadeRoutine()
        {
            float rate = 0f;
            Color startColor = new Color(1f, 1f, 1f, 0f);
            Color endColor = new Color(1f, 1f, 1f, 1f);
            clearRenderer.sharedMaterial.color = startColor;
            clearRenderer.enabled = true;

            while (rate < 1)
            {
                rate += Time.deltaTime / 2f;
                proceedRenderer.sharedMaterial.color = Color.Lerp(endColor, startColor, rate);
                clearRenderer.sharedMaterial.color = Color.Lerp(startColor, endColor, rate);
                yield return null;
            }

            proceedRenderer.gameObject.SetActive(false);
            proceedRenderer.sharedMaterial.color = endColor;
            yield return null;
        }
    }
}

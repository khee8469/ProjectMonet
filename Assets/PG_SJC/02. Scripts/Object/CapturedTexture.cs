using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class CapturedTexture : InteractObject, IInteractable
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private GameObject grabbedFrame;    // 그랩 시 프레임
        [SerializeField]
        private GameObject originFrame;     // 기존 프레임
        [SerializeField]
        private MeshRenderer meshRenderer;  // 액자의 배경으로 사용될 쿼드 메시
        [SerializeField]
        private Animator anim;

        [Tooltip("활성화 데이터")]
        [SerializeField]
        private PosRotPair activeData;

        [Tooltip("시네마틱 이벤트 재생시간")]
        [SerializeField]
        private float cinematicTime;

        [Tooltip("하이라이팅 할 머터리얼 ")]
        [SerializeField]
        private Material highlightMT;       // 액자를 하이라이팅 할 머터리얼
        private Material originMT;          // 액자의 기본 머터리얼 (하이라이팅 - 기본 - 하이라이팅)

        [Tooltip("하이라이팅 할 색상")]
        [SerializeField]
        private Color highlightingColor;    // 하이라이팅 머터리얼 색상
        private Color originColor;          // 기존 머터리얼 색상

        [Header("착시 적용 시 활성화 할 오브젝트 리스트")]
        [SerializeField]
        public List<GameObject> activableList;

        private bool isActive = false;
        public bool IsActive 
        {
            get  { return isActive; } 
            set
            {
                // 그랩된 상태에서만 활성화 여부 체크
                if (value)
                    isActive = isGrabbed;
                else
                    isActive = value;
            }
        }   

        public bool IsGrabbed   // isGrabbed 프로퍼티
        {
            get { return isGrabbed; }
            set { isGrabbed = value; }
        }

        private Coroutine highligtingRoutine;

        protected override void Awake()
        {
            base.Awake();
            objectType = ObjectType.CapturedScreen;

            // 원본 머터리얼을 캐싱
            originMT = meshRenderer.sharedMaterial;
            originColor = originMT.color;
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            meshRenderer.sharedMaterial = highlightMT;

            grabbedFrame.SetActive(!grabbedFrame.activeSelf);
            originFrame.SetActive(!originFrame.activeSelf);

            IsGrabbed = true;
            // 테스트용
            //OnHighlighting();
        }
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            meshRenderer.sharedMaterial = originMT;
            highlightMT.color = originColor;    // 머터리얼 색상 원복

            grabbedFrame.SetActive(!grabbedFrame.activeSelf);
            originFrame.SetActive(!originFrame.activeSelf);

            IsGrabbed = false;
            // 테스트용
            OffHighlighting();
        }

        // 트리거에 진입한 경우 메서드 호출
        public void OnHighlighting()
        {
            if (highligtingRoutine != null)
            {
                StopCoroutine(highligtingRoutine);
                highligtingRoutine = null;
            }

            highligtingRoutine = StartCoroutine(HighlightingRoutine());
        }
        // 트리거에서 벗어난 경우 메서드 호출
        public void OffHighlighting()
        {
            if (highligtingRoutine == null)
                return;

            StopCoroutine(highligtingRoutine);
            highligtingRoutine = null;
        }

        // 프레임 오브젝트 하이라이트 루틴
        IEnumerator HighlightingRoutine()
        {
            bool isHighlight = true;
            float rate = 0f;
            while (true)
            {
                if (rate >= 1f)
                {
                    isHighlight = !isHighlight;
                    rate = 0f;
                }

                rate += Time.deltaTime;

                if(isHighlight)
                    highlightMT.color = Color.Lerp(originColor, highlightingColor, rate);
                else
                    highlightMT.color = Color.Lerp(highlightingColor, originColor, rate);

                yield return null;
            }
        }

        // 아이템 상호작용
        public void Interact()
        {
            // 상태체크 (트리거에 진입한 경우)
            if(IsActive)
            {
                StartCoroutine(CinematicRoutine());
            }
        }

        // 시네머신 활성화 (트리거 상태에서 버튼 클릭 시)
        public void ActiveCinemachine()
        {

        }

        // 액자 시네마틱 루틴
        IEnumerator CinematicRoutine()
        {
            yield return Manager.UI.FadeInRoutine(1.5f);
            // 오브젝트 페이드 아웃 연출
            anim.SetTrigger("FadeOut");
            foreach (GameObject go in activableList)
                go.SetActive(true);
            yield return Manager.UI.FadeOutRoutine(1.5f);
            // 오브젝트 삭제
            Destroy(gameObject);
        }
    }
}

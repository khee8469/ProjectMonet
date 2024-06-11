using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class PhotoFrame : InteractObject
    {
        // 애니메이터 파라미터 ID
        enum AnimationParamID {FadeOut = 0}

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

        [Tooltip("하이라이팅 할 색상")]
        [SerializeField]
        private Color highlightingColor;    // 하이라이팅 머터리얼 색상
        private Color originColor;          // 기존 머터리얼 색상

        [Tooltip("페이드 아웃 머터리얼")]
        [SerializeField]
        private Material fadeOutMT;
        private Color fadeOutColor = new Color(0, 0, 0, 0);

        private Material originMT;          // 액자의 기본 머터리얼 (하이라이팅 - 기본 - 하이라이팅)

        [Header("착시 적용 시 활성화 할 오브젝트 리스트")]
        [SerializeField]
        public List<GameObject> activableList;

        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
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

            // 프레임 오브젝트를 변경 (XR Visual Feedback을 사용하지 않는 오브젝트로 변경)
            grabbedFrame.SetActive(true);
            originFrame.SetActive(false);

            meshRenderer.sharedMaterial = highlightMT;

            IsGrabbed = true;
            // 테스트용
            //OnHighlighting();
        }
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);

            meshRenderer.sharedMaterial = originMT;
            highlightMT.color = originColor;    // 머터리얼 색상 원복

            grabbedFrame.SetActive(false);
            originFrame.SetActive(true);


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

                if (isHighlight)
                    highlightMT.color = Color.Lerp(originColor, highlightingColor, rate);
                else
                    highlightMT.color = Color.Lerp(highlightingColor, originColor, rate);

                yield return null;
            }
        }

        // 액자 활성화 액션 실행
        public IEnumerator ActivePhotoFrame()
        {
            // 이미지 애니메이션 출력
            anim.SetTrigger("FadeOut");

            // 그림과 치환된 오브젝트 활성화
            foreach (GameObject go in activableList)
                go.SetActive(true);

            // 페이드아웃용 머터리얼로 변경
            meshRenderer.sharedMaterial = fadeOutMT;
            float rate = 0f;
            while(rate < 1f)
            {
                rate += Time.deltaTime / 0.9f;
                fadeOutMT.color = Color.Lerp(originColor, fadeOutColor, rate);
                yield return null;
            }
        }
    }
}

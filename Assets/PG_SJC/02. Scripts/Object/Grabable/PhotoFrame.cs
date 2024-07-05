using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;
using UnityEngine.UI;

namespace Jc
{
    public class PhotoFrame : InteractObject
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
        [SerializeField]
        private Rigidbody rigid;

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
        private Color originMTColor;          // 기존 머터리얼 색상

        [SerializeField]
        private Image screenImage;         // 스크린 이미지 
        [SerializeField]
        private Color originIMGColor;       // 스크린 이미지 기본색상
        [SerializeField]
        private Color fadeOutIMGColor = new Color(1,1,1,0);

        [Tooltip("페이드 아웃 머터리얼")]
        [SerializeField]
        private Material fadeOutMT;
        private Color fadeOutColor = new Color(0, 0, 0, 0);

        private Material originMT;          // 액자의 기본 머터리얼 (하이라이팅 - 기본 - 하이라이팅)

        [Header("착시 적용 시 활성화 할 오브젝트 리스트")]
        [SerializeField]
        public List<GameObject> activableList;

        [Header("착시 적용 시 비활성화 할 오브젝트 리스트")]
        [SerializeField]
        public List<GameObject> disActivableList;

        private bool isActive = false;
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        public bool IsGrabbed   // isGrabbed 프로퍼티
        {
            get { return isGrabbed; }
            set { isGrabbed = value; }
        }

        private Coroutine highligtingRoutine;
        private Transform mainCamTr;

        [Header("밸런싱")]
        [SerializeField]
        private Vector3 originPos;
        [SerializeField]
        private Quaternion originRot;

        protected override void Awake()
        {
            base.Awake();
            grabType = GrabType.Ray;

            // 원본 머터리얼을 캐싱
            originMT = meshRenderer.sharedMaterial;
            originMTColor = originMT.color;
            mainCamTr = Camera.main.transform;
            // 원본 이미지 색상 캐싱
            originIMGColor = screenImage.color;

            // 최초 위치 설정
            originPos = transform.position;
            originRot = transform.rotation;
        }

        private void Update()
        {
            if (isGrabbed && !isActive)
            {
                // 오브젝트를 메인 카메라 시점에 고정
                transform.position = mainCamTr.position + mainCamTr.forward;
                transform.forward = mainCamTr.forward;
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            rigid.isKinematic = false;
            base.OnSelectEntered(args);

            // 프레임 오브젝트를 변경 (XR Visual Feedback을 사용하지 않는 오브젝트로 변경)
            grabbedFrame.SetActive(true);
            originFrame.SetActive(false);

            meshRenderer.sharedMaterial = highlightMT;

            IsGrabbed = true;
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            meshRenderer.sharedMaterial = originMT;
            highlightMT.color = originMTColor;    // 머터리얼 색상 원복

            grabbedFrame.SetActive(false);
            originFrame.SetActive(true);

            IsGrabbed = false;

            StartCoroutine(CheckSocketRoutine());
        }

        // 놓여진 후 일정시간 뒤 활성화 되어있는지를 체크
        IEnumerator CheckSocketRoutine()
        {
            yield return new WaitForSeconds(0.05f);
            // 활성화되어있지 않고 공중에 떠 있는 경우
            if(!isActive && !isGrabbed)
            {
                StartCoroutine(DisActivePhotoFrame());
            }
        }
        IEnumerator DisActivePhotoFrame()
        {
            // 이미지 애니메이션 출력
            //anim.SetTrigger("FadeOut");

            grabbedFrame.SetActive(true);
            originFrame.SetActive(false);

            // 페이드아웃용 머터리얼로 변경
            meshRenderer.sharedMaterial = fadeOutMT;
            float rate = 0f;
            while (rate < 1f)
            {
                rate += Time.deltaTime * 1.5f;
                fadeOutMT.color = Color.Lerp(originMTColor, fadeOutColor, rate);
                screenImage.color = Color.Lerp(originIMGColor, fadeOutIMGColor, rate);
                yield return null;
            }

            //grabbedFrame.SetActive(false);
            //originFrame.SetActive(true);
            screenImage.color = originIMGColor;
            meshRenderer.sharedMaterial = originMT;
            transform.position = originPos;
            transform.rotation = originRot;
            rigid.isKinematic = true;
            yield return null;
        }

        // 액자 활성화 액션 실행
        public IEnumerator ActivePhotoFrame()
        {
            // 이미지 애니메이션 출력
            anim.SetTrigger("FadeOut");

            if (activableList != null && activableList.Count > 0)
            {
                // 그림과 치환된 오브젝트 활성화
                foreach (GameObject go in activableList)
                    go.SetActive(true);
            }

            if (disActivableList != null && disActivableList.Count > 0)
            {
                // 충돌체 등 비활성화 할 오브젝트를 비활성화
                foreach (GameObject go in disActivableList)
                    go.SetActive(false);
            }

            // 페이드아웃용 머터리얼로 변경
            meshRenderer.sharedMaterial = fadeOutMT;
            float rate = 0f;
            while (rate < 1f)
            {
                rate += Time.deltaTime / 0.9f;
                fadeOutMT.color = Color.Lerp(originMTColor, fadeOutColor, rate);
                screenImage.color = Color.Lerp(originIMGColor, fadeOutIMGColor, rate);
                yield return null;
            }
        }
    }
}

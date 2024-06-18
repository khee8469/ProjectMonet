using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    /// <summary>
    /// Custom Ray Inetractor
    /// </summary>
    public class RayInteractor : XRRayInteractor
    {
        [Header("커스텀 세팅")]
        [SerializeField]
        private GameObject controller;

        [SerializeField]
        private RectTransform canvasRect;
        private float canvasWidth;
        private float canvasHeight;
        [SerializeField]
        private RectTransform aimRect;

        [Header("밸런싱")]
        private bool isGrab = false;

        private XRInteractorLineVisual lineVisual;  // 라인 비주얼
        private LineRenderer lr;                    // 라인 렌더러
        private Camera cam;                         // 메인 카메라
        private Transform grabbedTr;                // 그랩한 오브젝트 트랜스폼

        protected override void Awake()
        {
            base.Awake();
            lineVisual = GetComponent<XRInteractorLineVisual>();
            lr = GetComponent<LineRenderer>();
            cam = Camera.main;
            lineVisual.enabled = false;
            canvasHeight = canvasRect.sizeDelta.y;
            canvasWidth = canvasRect.sizeDelta.x;
            aimRect.gameObject.SetActive(true);
        }

        private void Update()
        {
            AimPosition();
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            InteractObject itrObject = interactable as InteractObject;
            if (itrObject == null)
                return false;

            if (!GrabableDistance(itrObject))
                return false;

            return base.CanHover(interactable);
        }
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            InteractObject itrObject = interactable as InteractObject;
            if (itrObject == null)
                return false;

            if (!GrabableDistance(itrObject))
                return false;

            return base.CanSelect(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            grabbedTr = args.interactableObject.transform;
            isGrab = true;
            lineVisual.enabled = true;
            //aimRect.gameObject.SetActive(false);
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            isGrab = false;
            grabbedTr = null;
            lineVisual.enabled = false;
            //aimRect.gameObject.SetActive(true);
        }

        // 오브젝트를 잡을 수 있는 거리체크
        private bool GrabableDistance(InteractObject itrObject)
        {
            if (itrObject == null)
                return false;

            // 오브젝트의 그랩 허용 길이
            float grabDist = itrObject.GrabDistance;
            // 현재 오브젝트와의 거리
            float distance = (itrObject.transform.position - transform.position).sqrMagnitude;

            if (distance > grabDist * grabDist)
            {
                return false;
            }

            return true;
        }

        // 에임 포지셔닝
        private void AimPosition()
        {
            Vector2 viewportPos = Vector2.zero;

            if (isGrab)
            {
                viewportPos = cam.WorldToViewportPoint(grabbedTr.position);
            }
            else
            {
                viewportPos = cam.WorldToViewportPoint(rayEndPoint);
            }

            Vector2 screenPos = new Vector2(
                ((viewportPos.x * canvasWidth) - (canvasWidth * 0.5f)),
                ((viewportPos.y * canvasHeight) - (canvasHeight * 0.5f)));

            aimRect.anchoredPosition = screenPos;

        }
    }
}

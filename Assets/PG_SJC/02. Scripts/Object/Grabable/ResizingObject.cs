using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class ResizingObject : InteractObject
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private float grabDistance;

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private Camera mainCamera;

        public bool IsGrabbed   // isGrabbed 프로퍼티
        {
            get { return isGrabbed; }
            set { isGrabbed = value; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!IsGrabbed) return;

            Resize();
            SetPosition();
        }
        private void Resize()
        {

        }
        private void SetPosition()
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * grabDistance;
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);

            IsGrabbed = true;
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);

            IsGrabbed = false;
        }
    }
}
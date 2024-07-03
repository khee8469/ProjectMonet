using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;

namespace Jc
{
    public class PhotoFrameSocket : CustomSocket, IPuzzleable
    {
        [Space(5)]
        [Header("---- 컴포넌트 커스텀 ----")]
        [Space(5)]
        [Header("카메라 액션 용 가상 카메라")]
        [SerializeField]
        public CinemachineVirtualCamera actionVC;

        [SerializeField]
        private PhotoFrame photoFrame;
        [SerializeField]
        private Collider col;

        [SerializeField]
        public PuzzleManager puzzle;

        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }

        // PhotoFrame 만 상호작용
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            if (interactable is not PhotoFrame) return false;
            if ((interactable.transform.position - attachTransform.position).sqrMagnitude > 0.3f) return false;
            return base.CanHover(interactable);
        }
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (interactable is not PhotoFrame) return false;

            return base.CanSelect(interactable);    
        }

        // 소켓에 액자가 놓여진 경우
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log("Socket Select Enter");
            PhotoFrame targetFrame = args.interactableObject as PhotoFrame;
            if (targetFrame == null) return;

            targetFrame.gameObject.transform.position = attachTransform.position;
            targetFrame.gameObject.transform.rotation = attachTransform.rotation;

            base.OnSelectEntered(args);
            targetFrame.IsActive = true;
            StartCoroutine(CameraActionRoutine(targetFrame));
            // 퍼즐 클리어
            puzzle.OnClearPuzzle();
        }

        IEnumerator CameraActionRoutine(PhotoFrame photoFrame)
        {
            // 페이드 인 / 아웃
            // 카메라 변경
            Manager.Camera.TrackedPose.enabled = false;
            yield return Manager.UI.FadeInRoutine(1.5f);
            Manager.Camera.SetPriority(actionVC, -1, 1.5f);
            yield return Manager.UI.FadeOutRoutine(1.5f);

            yield return photoFrame.ActivePhotoFrame();

            Destroy(photoFrame.gameObject);
            Manager.Camera.SetPriority(null, -1, 1f);
        }

        public void ActiveSetting()
        {
            col.enabled = true;
        }
        public void DisActiveSetting()
        {
            col.enabled = false;
        }
        public void CompleteSetting()
        {
            Destroy(photoFrame.gameObject);
            puzzle.OnClearPuzzle();
            col.enabled = false;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            return;
        }
    }
}

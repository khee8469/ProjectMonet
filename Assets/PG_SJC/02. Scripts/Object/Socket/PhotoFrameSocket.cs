using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;

namespace Jc
{
    public class PhotoFrameSocket : CustomSocket
    {
        [Space(5)]
        [Header("---- 컴포넌트 커스텀 ----")]
        [Space(5)]
        [Header("카메라 액션 용 가상 카메라")]
        [SerializeField]
        public CinemachineVirtualCamera actionVC;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log("Socket Select Enter");
            PhotoFrame targetFrame = args.interactableObject as PhotoFrame;
            if (targetFrame == null) return;

            targetFrame.gameObject.transform.rotation = attachTransform.rotation;

            base.OnSelectEntered(args);

            StartCoroutine(CameraActionRoutine(targetFrame));
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
    }
}

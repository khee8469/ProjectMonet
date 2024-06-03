using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 현재 카메라가 렌더링 중인 텍스쳐를 바탕으로 텍스쳐 이미지를 추출
    /// </summary>
    public class PhotoCapture : MonoBehaviour
    {
        // 캡쳐 용 카메라
        public Camera captureCamera;

        // 캡쳐된 텍스쳐 
        public RenderTexture renderTexture;

        private void Awake()
        {
            // 카메라가 렌더링중인 텍스쳐가 없는경우
            if (captureCamera.targetTexture == null)
                captureCamera.targetTexture = renderTexture;
        }

        public Texture2D CapturePhoto()
        {
            // 현재 렌더링중인 텍스쳐를 할당
            RenderTexture currentRT = RenderTexture.active;
            // 캡처 카메라의 렌더 텍스처 활성화
            RenderTexture.active = captureCamera.targetTexture;

            // 카메라 영역 내 현재 씬 렌더링
            captureCamera.Render();

            // 렌더링된 텍스쳐에서 텍스쳐 이미지 할당 (복사 작업)
            Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            // 원래의 렌더 텍스쳐로 변경
            RenderTexture.active = currentRT;

            // 추출한 텍스쳐 이미지 반환
            return image;
        }

        public void UpdatePicture()
        {

        }
    }
}

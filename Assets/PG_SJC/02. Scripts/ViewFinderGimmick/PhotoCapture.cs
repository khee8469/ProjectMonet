using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private Rect imageRect;
        private Vector2 imagePivot;

        [SerializeField]
        private GameObject quadObject;
        [SerializeField]
        private MeshRenderer quadRenderer; 


        private void Awake()
        {
            if (captureCamera.targetTexture == null)
                captureCamera.targetTexture = renderTexture;

            imagePivot = new Vector2(0.5f, 0.5f);
        }

        public void UpdatePicture()
        {
            quadObject.SetActive(false);
            Texture2D texture = CapturePhoto();
            quadRenderer.material.mainTexture = texture;
            StartCoroutine(CaptureRoutine());
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
        
        IEnumerator CaptureRoutine()
        {
            Vector3 originScale = new Vector3(1.92f, 1.08f, 1f);
            Vector3 upScale = new Vector3(3.84f, 2.16f, 1f);
            quadObject.SetActive(true);

            float rate = 0f;
            while(rate < 1)
            {
                quadObject.transform.localScale = Vector3.Lerp(originScale, upScale, rate);
                rate += Time.deltaTime*3f;
                yield return null;
            }

            rate = 0f;
            while(rate <1)
            {
                quadObject.transform.localScale = Vector3.Lerp(upScale, originScale, rate);
                rate += Time.deltaTime*3f;
                yield return null;
            }

            yield return null;
        }
    }
}

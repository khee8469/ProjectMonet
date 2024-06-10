using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Jc
{
    public class ScreenCapture : MonoBehaviour
    {
        [Header("캡쳐된 이미지를 표시할 UI")]
        [SerializeField]
        private Image captureImage;

        private Texture2D screenCapture;
        private Sprite currentSprite;
        public Sprite CurrentSprite {get { return currentSprite; } }

        private Coroutine captureRoutine;

        private string filePath = "Assets/PG_SJC/98. ScreenShot";

        private void Start()
        {
            // 스크린 크기의 텍스쳐틀 생성
            screenCapture = new Texture2D(1270, 720, TextureFormat.RGB24, false);
        }

        public void Capture()
        {
            if (captureRoutine != null)
                StopCoroutine(captureRoutine);

            captureRoutine = StartCoroutine(CaptureRoutine());
            
        }

        // 캡쳐사진 동기화 (스크린 샷은 모든 렌더링이 종료된 후 이루어져야함.)
        IEnumerator CaptureRoutine()
        {
            // 한 프레임이 렌더링될때까지 대기
            yield return new WaitForEndOfFrame();

            // 캡쳐될 영역을 설정
            Rect regionToRead = new Rect(325f, 180f, 1270f, 720f);

            // 해당 영역의 픽셀값을 할당
            screenCapture.ReadPixels(regionToRead, 0, 0, false);
            screenCapture.Apply();
            
            // 텍스쳐파일 저장 (시간 소요)
            SaveTextureToPNG(screenCapture);
            // 이미지 렌더링
            UpdateImage();

            captureRoutine = null;
        }

        private void UpdateImage()
        {
            currentSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100f);
            captureImage.sprite = currentSprite;
        }

        private void SaveTextureToPNG(Texture2D texture)
        {
            // 파일 경로가 존재하지 않을 경우
            if (string.IsNullOrEmpty(filePath))
                return;

            // 경로 내 파일이 존재하지 않을경우
            if (!Directory.Exists(filePath))
                return;

            // 텍스쳐를 PNG Bytes로 인코딩
            byte[] texturePNGBytes = texture.EncodeToPNG();
            int count = new DirectoryInfo(filePath).GetFiles().Length;
            File.WriteAllBytes($"{filePath}/capture{count}.png", texturePNGBytes);
        }
    }
}

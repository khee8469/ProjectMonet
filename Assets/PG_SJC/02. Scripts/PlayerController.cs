using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

namespace Jc
{
    public class PlayerController : MonoBehaviour
    {
        
        private Camera mainCam;

        // 캡쳐 파일경로
        private string capturePath = "Assets/PG_SJC/98. ScreenShot/ScreenShot";

        [SerializeField]
        private float moveSpeed;

        private Vector3 moveDir;
        private Vector2 rotDir;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            Move();
        }

        private void OnRotate(InputValue value)
        {
            rotDir = value.Get<Vector2>();
        }
        private void Rotate()
        {

        }

        // 움직임 콜백
        private void OnMove(InputValue value)
        {
            Vector2 inputDir = value.Get<Vector2>();
            moveDir.x = inputDir.x;
            moveDir.z = inputDir.y;
        }
        private void Move()
        {
            if (moveDir == Vector3.zero) return;

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }

        // 줌인/아웃 콜백
        private void OnZoom(InputValue value)
        {
            float yAxis = value.Get<Vector2>().y;
            if (yAxis >= 120)
                Zoom(true);
            else if (yAxis <= -120)
                Zoom(false);
        }
        private void Zoom(bool isZoomIn)
        {
            mainCam.fieldOfView = isZoomIn ? mainCam.fieldOfView + 10 : mainCam.fieldOfView - 10;
        }

        // 캡쳐 콜백
        private void OnCapture(InputValue value)
        {
            Capture();
        }
        private void Capture()
        {
            // 디렉토리 내 파일 개수 추출
            DirectoryInfo directoryInfo = new DirectoryInfo(capturePath);
            int fileCount = directoryInfo.GetFiles().Length;

            ScreenCapture.CaptureScreenshot($"{capturePath}/ScreenCapture{fileCount}.png");

        }
    }
}
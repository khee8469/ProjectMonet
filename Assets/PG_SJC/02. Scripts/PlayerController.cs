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

        private void OnRotate(InputValue value)
        {
            //Debug.Log(value.Get<Vector2>());
        }

        private void OnMove(InputValue value)
        {
            Vector2 inputDir = value.Get<Vector2>();
            moveDir.x = inputDir.x;
            moveDir.z = inputDir.y;
        }


        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void OnZoom(InputValue value)
        {
            float yAxis = value.Get<Vector2>().y;

            if (yAxis >= 120)
                mainCam.fieldOfView += 10;
            else if (yAxis <= -120)
                mainCam.fieldOfView -= 10;
        }

        private void OnCapture(InputValue value)
        {
            // 디렉토리 내 파일 개수 추출
            DirectoryInfo directoryInfo = new DirectoryInfo(capturePath);
            int fileCount = directoryInfo.GetFiles().Length;

            ScreenCapture.CaptureScreenshot($"{capturePath}/ScreenCapture{fileCount}.png");
        }
        private void Move()
        {
            if (moveDir == Vector3.zero) return;

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }

        private void Update()
        {
            Move();
        }
    }
}
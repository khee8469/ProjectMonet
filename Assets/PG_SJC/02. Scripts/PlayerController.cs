using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

namespace Jc
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PhotoCapture photoCapture;

        [SerializeField]
        private PhotoProjection photoProjection;

        [SerializeField]
        private CharacterController controller;

        [SerializeField]
        private float moveSpeed;

        private Vector3 moveDir;
        private Vector2 rotDir;

        private Camera mainCam;
        // 캡쳐 파일경로
        private string capturePath = "Assets/PG_SJC/98. ScreenShot/ScreenShot";

        [SerializeField]
        private Texture2D currentTexture;

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

            controller.Move(moveDir * moveSpeed * Time.deltaTime);
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
            currentTexture = photoCapture.CapturePhoto();
        }

        // 투영 콜백
        private void OnProjection(InputValue value)
        {
            if (currentTexture == null)
                return;

            photoProjection.ProjectPhoto(currentTexture, transform.position + transform.forward * 5f, Quaternion.identity);
        }
    }
}
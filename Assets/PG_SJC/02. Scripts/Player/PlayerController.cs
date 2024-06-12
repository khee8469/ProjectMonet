using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using UnityEngine.UI;

namespace Jc
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private ScreenCapture screenCapture;

        [SerializeField]
        private GameObject captureDisplay;

        [SerializeField]
        private CharacterController controller;

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float mouseSensitivity;

        private Vector3 moveDir;
        private Vector2 rotDir;

        private Camera mainCam;
        private Transform cameraTr;

        private void Awake()
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            mainCam = Camera.main;
            cameraTr = mainCam.transform;
        }

        private void OnEnable()
        {
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void OnRotate(InputValue value)
        {
            rotDir = value.Get<Vector2>();
        }
        private void Rotate()
        {
            cameraTr.Rotate(Vector3.right, -rotDir.y * mouseSensitivity * Time.deltaTime);
            transform.Rotate(Vector3.up, rotDir.x * mouseSensitivity * Time.deltaTime);
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

            controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
            controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
        }

        // 줌인/아웃 콜백
        private void OnZoom(InputValue value)
        {
            //float yAxis = value.Get<Vector2>().y;
            //if (yAxis >= 120)
            //    Zoom(true);
            //else if (yAxis <= -120)
            //    Zoom(false);
        }
        private void Zoom(bool isZoomIn)
        {
            mainCam.fieldOfView = isZoomIn ? mainCam.fieldOfView + 10 : mainCam.fieldOfView - 10;
        }

        // 캡쳐 콜백
        private void OnCapture(InputValue value)
        {
            screenCapture.Capture();
        }

        private void OnCapturePopUp(InputValue value)
        {
            captureDisplay.SetActive(!captureDisplay.activeSelf);

            if (captureDisplay.activeSelf && screenCapture.CurrentSprite != null)
                captureDisplay.transform.GetChild(0).GetComponent<Image>().sprite = screenCapture.CurrentSprite;
        }

        // 투영 콜백
        private void OnProjection(InputValue value)
        {

        }
    }
}
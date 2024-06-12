using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
namespace Jc
{
    public class PlayerControllerCallback : MonoBehaviour
    {
        [Header("왼손 Input")]
        public InputActionReference leftTriggerRef;         // 왼손 트리거       (상호작용)
        public InputActionReference leftGripRef;            // 왼손 그립         (잡기)
        public InputActionReference leftMenuBTNRef;         // 왼손 메뉴 버튼    (인벤토리 / 퀘스트)
        public InputActionReference leftXBTNRef;            // 왼손 X 버튼       
        public InputActionReference leftYBTNRef;            // 왼손 Y 버튼
        [Space(5)]
        [Header("오른손 Input")]
        public InputActionReference rightTriggerRef;        // 오른손 트리거     ( "" )
        public InputActionReference rightGripRef;           // 오른손 그립       ( "" )
        public InputActionReference rightOculusBTNRef;      // 오른손 오큘러스 버튼
        public InputActionReference rightABTNRef;           // 오른손 A 버튼
        public InputActionReference rightBBTNRef;           // 오른손 B 버튼

        private void OnEnable()
        {
            // 왼손 콜백 등록
            leftTriggerRef.action.performed += OnLeftTriggerEnter;
            leftGripRef.action.performed += OnLeftGripEnter;
            leftMenuBTNRef.action.performed += OnLeftMenuButtonEnter;
            leftXBTNRef.action.performed += OnLeftXButtonEnter;
            leftYBTNRef.action.performed += OnLeftYButtonEnter;

            leftTriggerRef.action.canceled += OnLeftTriggerExit;
            leftGripRef.action.canceled += OnLeftGripExit;
            leftMenuBTNRef.action.canceled += OnLeftMenuButtonExit;
            leftXBTNRef.action.canceled += OnLeftXButtonExit;
            leftYBTNRef.action.canceled += OnLeftYButtonExit;

            // 오른손 콜백 등록
            rightTriggerRef.action.performed += OnRightTriggerEnter;
            rightGripRef.action.performed += OnRightGripEnter;
            rightOculusBTNRef.action.performed += OnRightOculusButtonEnter;
            rightABTNRef.action.performed += OnRightAButtonEnter;
            rightBBTNRef.action.performed += OnRightBButtonEnter;

            rightTriggerRef.action.canceled += OnRightTriggerEnter;
            rightGripRef.action.canceled += OnRightGripEnter;
            rightOculusBTNRef.action.canceled += OnRightOculusButtonEnter;
            rightABTNRef.action.canceled += OnRightAButtonEnter;
            rightBBTNRef.action.canceled += OnRightBButtonEnter;
        }
        private void OnDisable()
        {
            // 왼손 콜백 해제
            leftTriggerRef.action.performed -= OnLeftTriggerEnter;
            leftGripRef.action.performed -= OnLeftGripEnter;
            leftMenuBTNRef.action.performed -= OnLeftMenuButtonEnter;
            leftXBTNRef.action.performed -= OnLeftXButtonEnter;
            leftYBTNRef.action.performed -= OnLeftYButtonEnter;

            leftTriggerRef.action.canceled -= OnLeftTriggerExit;
            leftGripRef.action.canceled -= OnLeftGripExit;
            leftMenuBTNRef.action.canceled -= OnLeftMenuButtonExit;
            leftXBTNRef.action.canceled -= OnLeftXButtonExit;
            leftYBTNRef.action.canceled -= OnLeftYButtonExit;

            // 오른손 콜백 해제
            rightTriggerRef.action.performed -= OnRightTriggerEnter;
            rightGripRef.action.performed -= OnRightGripEnter;
            rightOculusBTNRef.action.performed -= OnRightOculusButtonEnter;
            rightABTNRef.action.performed -= OnRightAButtonEnter;
            rightBBTNRef.action.performed -= OnRightBButtonEnter;

            rightTriggerRef.action.canceled -= OnRightTriggerEnter;
            rightGripRef.action.canceled -= OnRightGripEnter;
            rightOculusBTNRef.action.canceled -= OnRightOculusButtonEnter;
            rightABTNRef.action.canceled -= OnRightAButtonEnter;
            rightBBTNRef.action.canceled -= OnRightBButtonEnter;
        }

        #region 왼손 컨트롤러 콜백
        public void OnLeftTriggerEnter(InputAction.CallbackContext context) { }
        public void OnLeftGripEnter(InputAction.CallbackContext context) { }
        public void OnLeftMenuButtonEnter(InputAction.CallbackContext context) { }
        public void OnLeftXButtonEnter(InputAction.CallbackContext context) { }
        public void OnLeftYButtonEnter(InputAction.CallbackContext context) { }

        public void OnLeftTriggerExit(InputAction.CallbackContext context) { }
        public void OnLeftGripExit(InputAction.CallbackContext context) { }
        public void OnLeftMenuButtonExit(InputAction.CallbackContext context) { }
        public void OnLeftXButtonExit(InputAction.CallbackContext context) { }
        public void OnLeftYButtonExit(InputAction.CallbackContext context) { }
        #endregion

        #region 오른손 컨트롤러 콜백

        public void OnRightTriggerEnter(InputAction.CallbackContext context) { }
        public void OnRightGripEnter(InputAction.CallbackContext context) { }
        public void OnRightOculusButtonEnter(InputAction.CallbackContext context) { }
        public void OnRightAButtonEnter(InputAction.CallbackContext context) { }
        public void OnRightBButtonEnter(InputAction.CallbackContext context) { }

        public void OnRightTriggerExit(InputAction.CallbackContext context) { }
        public void OnRightGripExit(InputAction.CallbackContext context) { }
        public void OnRightOculusButtonExit(InputAction.CallbackContext context) { }
        public void OnRightAButtonExit(InputAction.CallbackContext context) { }
        public void OnRightBButtonExit(InputAction.CallbackContext context) { }
        #endregion
    }
}
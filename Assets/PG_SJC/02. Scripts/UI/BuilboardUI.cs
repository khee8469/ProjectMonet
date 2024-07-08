using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 월드 내 빌보드 UI
    /// </summary>
    public class BuilboardUI : MonoBehaviour
    {
        [SerializeField]
        private Transform mainCameraTr;

        private Coroutine lookAtRoutine;

        [SerializeField]
        private bool enableBuilboard = false;
        public bool EnableBuilboard
        {
            get { return enableBuilboard; }
            set
            {
                enableBuilboard = value;
            }
        }

        private Vector3 originPos;
        private void OnEnable()
        {
            originPos = transform.position;
            mainCameraTr = Camera.main.transform;

            if (mainCameraTr == null)
                Debug.Log("메인 카메라를 찾을 수 없습니다.");
        }

        private void OnDisable()
        {
            if (lookAtRoutine != null)
            {
                StopCoroutine(lookAtRoutine);
                lookAtRoutine = null;
            }
        }

        private void LateUpdate()
        {
            if (mainCameraTr != null && enableBuilboard)
            {
                Vector3 rotDir = (mainCameraTr.transform.position - originPos).normalized;
                Vector3 dir = new Vector3(rotDir.x, 0f, rotDir.z);

                transform.position = originPos + dir * 0.5f;   // UI 위치 설정
                transform.forward = rotDir; // 기울기 설정
            }
        }
    }
}

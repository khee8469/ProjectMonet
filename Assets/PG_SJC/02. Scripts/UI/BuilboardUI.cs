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

        private void OnEnable()
        {
            mainCameraTr = Camera.main.transform;
            if (mainCameraTr == null)
                Debug.Log("메인 카메라를 찾을 수 없습니다.");

            lookAtRoutine = StartCoroutine(LookAtRoutine());
        }

        private void OnDisable()
        {
            if (lookAtRoutine != null)
            {
                StopCoroutine(lookAtRoutine);
                lookAtRoutine = null;
            }
        }

        IEnumerator LookAtRoutine()
        {
            while (true)
            {
                // 0.1초에 한 번씩 플레이어의 메인 카메라를 바라봄
                yield return new WaitForSeconds(0.1f);
                Vector3 lookDir = (transform.position - mainCameraTr.position).normalized;
                transform.forward = lookDir;
            }
        }
    }
}

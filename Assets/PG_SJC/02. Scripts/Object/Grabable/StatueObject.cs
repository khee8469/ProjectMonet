using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Jc
{
    // 스테이지 1-3 퍼즐 조각상 오브젝트
    public class StatueObject : InteractObject
    {
        [Header("활성화 이벤트")]
        [SerializeField]
        private Light spotLight;
        [SerializeField]
        private Rigidbody rigid;
        [SerializeField]
        private BoxCollider col;
        [SerializeField]
        private float maxIntensity;
        [SerializeField]
        private float enableRoutineTime;

        [SerializeField]
        private Transform startTransform;

        private Coroutine enableRoutine;

        protected override void OnEnable()
        {
            base.OnEnable();

            enableRoutine = StartCoroutine(EnableRoutine());
        }

        // 시작 루틴 종료
        public void StopRoutine()
        {
            if(enableRoutine != null)
            {
                col.enabled = false;
                spotLight.enabled = false;
                transform.position = startTransform.position;
                rigid.useGravity = false;
                enableRoutine = null;
            }
        }

        IEnumerator EnableRoutine()
        {
            rigid.useGravity = false;
            col.enabled = false;

            // 초기세팅
            float rate = 0f;
            Vector3 startPos = transform.position;
            Vector3 endPos = startTransform.position;
            Vector3 startScale = Vector3.one * 2;
            Vector3 endScale = Vector3.one;
            
            while(rate < 1f)
            {
                rate += Time.deltaTime / enableRoutineTime;
                transform.Rotate(Vector3.up, 2f);
                transform.position = Vector3.Lerp(startPos, endPos, rate);
                transform.localScale = Vector3.Lerp(startScale, endScale, rate);
                spotLight.intensity = Mathf.Lerp(maxIntensity, 0f, rate);
                yield return null;
            }

            col.enabled = true;
            spotLight.enabled = false;
            transform.position = endPos;
            rigid.useGravity = true;
            enableRoutine = null;
            yield return null;
        }
    }
}

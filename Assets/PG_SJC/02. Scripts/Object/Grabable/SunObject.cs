using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 스테이지 1-4 태양 오브젝트
    public class SunObject : ResizingObject
    {
        [Header("태양 오브젝트 컴포넌트")]
        [SerializeField]
        private Light directionalLight;
        [SerializeField]
        private Light pointLight;

        private float initScale;

        protected override void OnEnable()
        {
            base.OnEnable();
            initScale = transform.localScale.x;
        }

        protected override IEnumerator ResizeRoutine()
        {
            while(isGrabbed)
            {
                yield return new WaitForSeconds(0.01f);
                SetPosition();
                SetIntensity();
            }
        }
        private void SetIntensity()
        {
            float rate = transform.localScale.x / initScale;
            pointLight.intensity = rate;
            pointLight.range = Mathf.Lerp(0, 5f, rate);
        }

        // 태양이 오두막 밖으로 나간 경우
        private void OnTriggerEnter(Collider other)
        {
            if (!Manager.Layer.puzzleLM.Contain(other.gameObject.layer))
                return;

            directionalLight.intensity = 0f;
        }

        // 태양이 오두막 내부에 들어온 경우
        private void OnTriggerExit(Collider other)
        {
            if (!Manager.Layer.puzzleLM.Contain(other.gameObject.layer))
                return;

            directionalLight.intensity = 1.5f;
        }
    }
}

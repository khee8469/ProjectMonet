using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 스테이지 1 체스 퍼즐 기물
    public class ChessObject : ResizingObject
    {
        [Header("오브젝트 활성화 여부")]
        [SerializeField]
        private bool isEnable;
        public bool IsEnable { get { return isEnable; } }

        public void ResetObject()
        {
            if (isEnable)
                return;

            StartCoroutine(ResetRoutine());
        }

        protected override IEnumerator ResetRoutine()
        {
            float rate = 0f;
            Vector3 startScale = transform.localScale;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            while (rate < 1f)
            {
                rate += Time.deltaTime * 2f;
                transform.position = Vector3.Lerp(startPos, resetTransform.position, rate);
                transform.rotation = Quaternion.Lerp(startRot, resetTransform.rotation, rate);
                transform.localScale = Vector3.Lerp(startScale, resetTransform.scale, rate);
                yield return null;
            }

            transform.position = resetTransform.position;
            transform.rotation = resetTransform.rotation;
            transform.localScale = resetTransform.scale;
        }
    }
}

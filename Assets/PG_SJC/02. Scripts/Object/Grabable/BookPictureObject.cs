using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 그림책 그림 오브젝트
    public class BookPictureObject : InteractObject
    {
        [Space(10)]
        [Header("---------------------")]
        [Header("그림책 오브젝트 세팅")]
        [SerializeField]
        private PictureBook book;
        [SerializeField]
        private Transform originSocket;
        [SerializeField]
        private Transform targetSocket;
        [SerializeField]
        private SpriteRenderer spr;
        [SerializeField]
        private float socketThreshold;

        [Header("밸런싱")]
        [SerializeField]
        private Vector3 socketPosition;
        [SerializeField]
        private Quaternion socketRotation;

        private Coroutine returnRoutine;

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            // 조건 체크
            // 조건값 충족
            if(CheckSocket())
            {

            }
            // 조건값 불충
            else
            {
                returnRoutine = StartCoroutine(ReturnRoutine());
            }
        }

        private bool CheckSocket()
        {
            // 거리계산
            if((transform.position - socketPosition).sqrMagnitude < socketThreshold*socketThreshold)
            {
                return true;
            }
            return false;
        }

        public void ReturnPosition()
        {
            transform.parent = originSocket;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        IEnumerator ReturnRoutine()
        {
            float rate = 0f;
            Color startColor = Color.white;
            Color endColor = new Color(1, 1, 1, 0);

            while(rate  < 1f)
            {
                rate += Time.deltaTime * 3f;
                spr.color = Color.Lerp(startColor, endColor, rate);
                yield return null;
            }

            ReturnPosition();
            spr.color = startColor;
            yield return null;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Jc
{
    public class Picture : InteractObject
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;

        [SerializeField]
        private BoxCollider boxCollider;
        [SerializeField]
        private Transform socketTransfrom;
        [SerializeField]
        private float minDistance;
        [SerializeField]
        private float trackingTime;

        [Header("퍼즐 인덱스")]
        public int puzzleIndex;

        private Vector3 originPos;      // 초기 위치
        private Quaternion originRot;   // 초기 회전 값


        protected override void Awake()
        {
            base.Awake();
            originPos = transform.position;
            originRot = transform.rotation;
        }

        private void ResetPosition()
        {
            transform.position = originPos; 
            transform.rotation = originRot; 
        }

        private bool CheckSocket()
        {
            float distance = (socketTransfrom.position - transform.position).sqrMagnitude;
            return distance <= minDistance;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if(CheckSocket())
            {
                // 성공
                boxCollider.enabled = false;
                transform.parent = socketTransfrom;
                StartCoroutine(SuccessRoutine());
            }
            else
            {
                // 실패
                StartCoroutine(ResetRoutine());
            }
        }

        // 오브젝트 소켓 트래킹 루틴 
        IEnumerator SuccessRoutine()
        {
            float rate = 0f;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            Vector3 endPos = socketTransfrom.position;
            Quaternion endRot = socketTransfrom.rotation;
            while (rate < 1f)
            {
                rate += Time.deltaTime / trackingTime;
                transform.position = Vector3.Lerp(startPos, endPos, rate);
                transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
                yield return null;
            }

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            // 퍼즐매니저 업데이트
            puzzle.UpdateCondition(puzzleIndex);
        }
        IEnumerator ResetRoutine()
        {
            float rate = 0f;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            Vector3 endPos = originPos;
            Quaternion endRot = originRot;

            boxCollider.enabled = false;
            while (rate < 1f)
            {
                rate += Time.deltaTime / trackingTime;
                transform.position = Vector3.Lerp(startPos, endPos, rate);
                transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
                yield return Time.deltaTime;
            }

            transform.position = originPos;
            transform.rotation = originRot;
            boxCollider.enabled = true;
        }
    }
}

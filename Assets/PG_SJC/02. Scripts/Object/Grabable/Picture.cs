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
        private BoxCollider boxCollider;
        [SerializeField]
        private Transform socketTransfrom;
        [SerializeField]
        private float minDistance;

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
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                // 실패
                ResetPosition();
            }
        }
    }
}

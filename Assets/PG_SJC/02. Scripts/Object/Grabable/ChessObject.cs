using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 스테이지 1 체스 퍼즐 기물
    public class ChessObject : ResizingObject
    {
        [Header("오브젝트 활성화 여부")]
        [SerializeField]
        private bool isEnable;
        public bool IsEnable { get { return isEnable; } set { isEnable = value; } }

        private Rigidbody rigid;
        private Collider col;

        [Header("밸런싱")]
        [SerializeField]
        private IXRSelectInteractor interactor;
        [SerializeField]
        private bool isEnterRoom = true;

        private Coroutine exitRoutine;

        protected override void Awake()
        {
            base.Awake();
            rigid = GetComponent<Rigidbody>();
            col = GetComponent<Collider>(); 
        }
        public void ResetObject()
        {
            if (isEnable)
                return;

            if (isSelected && interactor != null)
                interactionManager.SelectExit(interactor, this);

            if (exitRoutine != null)
            {    
                StopCoroutine(exitRoutine);
                exitRoutine = null;
            }

            StartCoroutine(ResetRoutine());
        }
        protected override IEnumerator ResetRoutine()
        {
            rigid.velocity = Vector3.zero;
            col.enabled = false;
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
            col.enabled = true;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            // 잡고있는 인터렉터 할당
            interactor = args.interactorObject;
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            // 기존에 잡고있던 인터렉터 해제
            interactor = null;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (Manager.Layer.puzzleLM.Contain(other.gameObject.layer))
            {
                isEnterRoom = true;
                if (exitRoutine != null)
                    StopCoroutine(exitRoutine);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (Manager.Layer.puzzleLM.Contain(other.gameObject.layer))
            {
                isEnterRoom = false;
                if (exitRoutine != null)
                    StopCoroutine(exitRoutine);

                // 범위 세팅
                exitRoutine = StartCoroutine(ExitRoutine());
            }
        }

        private IEnumerator ExitRoutine()
        {
            yield return new WaitForSeconds(2f);
            
            exitRoutine = null;
            if (!isEnterRoom)
                ResetObject();
        }
    }
}

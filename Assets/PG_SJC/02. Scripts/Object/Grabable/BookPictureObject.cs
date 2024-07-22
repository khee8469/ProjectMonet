using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 그림책 그림 오브젝트
    public class BookPictureObject : InteractObject, IPuzzleable
    {
        [Space(10)]
        [Header("---------------------")]
        [Header("그림책 오브젝트 세팅")]
        [SerializeField]
        private PictureBook book;
        [SerializeField]
        private Collider col; 
        [SerializeField]
        private Transform originSocket;
        [SerializeField]
        private Transform targetSocket;
        [SerializeField]
        private SpriteRenderer spr;
        [SerializeField]
        private Rigidbody rigid;
        [SerializeField]
        private float socketThreshold;
        [SerializeField]
        private int puzzleIndex = -1;

        [Header("밸런싱")]
        [SerializeField]
        private Vector3 socketPosition;
        [SerializeField]
        private Quaternion socketRotation;

        private Coroutine socketRoutine;
        private Coroutine returnRoutine;

        protected override void Awake()
        {
            base.Awake();
            RegistObject(book);

            socketPosition = targetSocket.position;
            socketRotation = targetSocket.rotation;
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            // 현재 페이지의 소켓을 할당하지 않은 상태로 변경
            if (book.CurrentDepth.isAssigned)
                book.CurrentDepth.isAssigned = false;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            // 소켓위치 체크 성공
            if (CheckPosition())
            {
                socketRoutine = StartCoroutine(SocketRoutine());
            }
            // 소켓 위치 벗어남
            else
            {
                // 초기 소켓으로 리턴
                returnRoutine = StartCoroutine(ReturnRoutine());
            }
        }

        // 소켓 위치 체크
        private bool CheckPosition()
        {
            // 거리계산
            if ((transform.position - socketPosition).sqrMagnitude < socketThreshold * socketThreshold)
            {
                // 해당 소켓에 할당된 이미지가 있을 경우
                if (book.CurrentDepth.isAssigned)
                    return false;
                // 소켓이 없는 첫 페이지일 경우
                if (book.CurrentDepth.socketTransform == null)
                    return false;

                return true;
            }
            return false;
        }
        // 소켓 체크
        private bool CheckSocket()
        {
            // 이미지가 속한 페이지를 부모로 설정
            transform.parent = book.CurrentDepth.socketTransform;

            // 현재 페이지의 소켓을 체크
            // 타깃 소켓인 경우
            if(book.CurrentDepth.socketTransform == targetSocket)
                return true;
            
            return false;
        }
        public void ReturnPosition()
        {
            // 최초 페이지를 부모로 설정
            transform.parent = originSocket;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            rigid.velocity = Vector3.zero;
        }
        IEnumerator ReturnRoutine()
        {
            float rate = 0f;
            Color startColor = Color.white;
            Color endColor = new Color(1, 1, 1, 0);
            rigid.velocity = Vector3.zero;

            while (rate < 1f)
            {
                rate += Time.deltaTime * 3f;
                spr.color = Color.Lerp(startColor, endColor, rate);
                yield return null;
            }

            ReturnPosition();
            spr.color = startColor;
            returnRoutine = null;
            yield return null;
        }
        IEnumerator SocketRoutine()
        {
            float rate = 0f;
            col.enabled = false;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            Vector3 endPos = socketPosition;
            Quaternion endRot = socketRotation;

            while (rate < 1f)
            {
                rate += Time.deltaTime * 3f;
                transform.position = Vector3.Lerp(startPos, endPos, rate);
                transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
                yield return null;
            }

            transform.parent = book.CurrentDepth.socketTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            rigid.velocity = Vector3.zero;

            book.CurrentDepth.isAssigned = true;
            // 소켓 확인
            if (CheckSocket())
            {
                UpdatePuzzleManager(book,puzzleIndex);
            }
            else
            {
                col.enabled = true;
            }
            socketRoutine = null;
            yield return null;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
        {
            puzzle.UpdateCondition(index);
        }

        public void ActiveSetting()
        {
        }

        public void DisActiveSetting()
        {
        }

        public void CompleteSetting()
        {
        }
    }
}
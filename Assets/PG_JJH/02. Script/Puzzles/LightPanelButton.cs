using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;
using Jc;
using UnityEngine.Events;

namespace JJH
{
    


    public class LightPanelButton : CustomButton
    {

        public enum Direction
        {
            UP , DOWN , LEFT , RIGHT
        }


        [Tooltip("등대 머리 ")]
        [SerializeField]
        private GameObject lightHouse;

        [Tooltip("각 버튼들의 스타트 포지션")]
        [SerializeField] private Vector3 startPosition;

        [Tooltip("각 버튼 들의 다 눌린 포지션")]
        [SerializeField] private Vector3 lastPositiion;

        [Tooltip("버튼이 완전히 눌렸는지를 확인 할 bool 변수")]
        [SerializeField] private bool isComplete = false;

        [Tooltip("보간이 지속 되는 시간")]
        [SerializeField] private float duration = 1f;

        [Tooltip("여러 버튼을 동시에 입력 할 시에 입력을 방지해 줄 bool 변수")]
        [SerializeField] private bool isPushing = false;

        [Tooltip("버튼이 얼마나 눌릴지 체크")]
        [SerializeField] private float checkPosition = -0.045f;
        
        // button property를 이용하여 그 오브젝트의 위치를 조정해준다.

        private void Start()
        {
            startPosition = transform.localPosition;
            lastPositiion = new Vector3(startPosition.x, startPosition.y +checkPosition, startPosition.z);
        }

        // 눌렀을 때 보간으로 들어가고 떼는 순간 보간으로 올라오고 
        // 그 눌렀다 떼는 거를 파악해서 오브젝트의 각도 움직여주기. 

        [Tooltip("버튼 자신의 방향")]
        public Direction myDirection;

        public void UpButtonPush()
        {
            Debug.Log("업 버튼 누름");
            if (isPushing == true) return;
            StartCoroutine(PushLerpRoutine(startPosition, lastPositiion, duration));
        }

        public void DownButtonPush()
        {
            Debug.Log("다운 버튼 누름");
            if (isPushing == true) return;
            StartCoroutine(PushLerpRoutine(startPosition , lastPositiion , duration));
        }

        public void LeftButtonPush()
        {
            Debug.Log("왼쪽 버튼 누름");
            if (isPushing == true) return;
            StartCoroutine(PushLerpRoutine(startPosition, lastPositiion, duration));

        }

        public void RightButtonPush()
        {
            Debug.Log("오른쪽 버튼 누름");
            if (isPushing == true) return;
            StartCoroutine(PushLerpRoutine(startPosition, lastPositiion, duration));

        }

        public void UpButtonRelease()
        {
            Debug.Log("업 버튼 뗌");
            isPushing = false; // 어떤 버튼이든 일단 떼면 다른 버튼을 누를 수 있어야 하기 때문에 False 로 변경 
            StartCoroutine(ReleaseLerpRoutine(startPosition, lastPositiion, duration));
        }

        public void DownButtonRelease()
        {
            Debug.Log("다운 버튼 똄");
            isPushing = false;
            StartCoroutine(ReleaseLerpRoutine(startPosition, lastPositiion, duration));
        }

        public void RightButtonRelease()
        {
            Debug.Log("라이트 버튼 뗌");
            isPushing = false;
            StartCoroutine(ReleaseLerpRoutine(startPosition, lastPositiion, duration));

        }

        public void LeftButtonRelease()
        {
            Debug.Log("왼쪽 버튼 뗌");
            isPushing = false;
            StartCoroutine(ReleaseLerpRoutine(startPosition, lastPositiion, duration));

        }


        // 버튼이 들어가는 모습을 구현 할 코루틴 
        private IEnumerator PushLerpRoutine(Vector3 start , Vector3 end , float duration)
        {
            float elapsed = 0f;

            while(elapsed < duration)
            {
                button.transform.localPosition = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.fixedDeltaTime;
                yield return null;
            }

            button.localPosition = end; // last Position 을 받아서 저장.        
        }

        // 버튼이 돌아오는 모습을 구현 할 코루틴 
        private IEnumerator ReleaseLerpRoutine(Vector3 start, Vector3 end, float duration)
        {
            float elsaped = 0f;
            while(elsaped < duration)
            {
                button.transform.localPosition = Vector3.Lerp(end , start , elsaped / duration);
                elsaped += Time.fixedDeltaTime;
                yield return null;
            }
            button.localPosition = start;
        }

        private void LightHouseRotation(Direction myDirection)
        {
            switch (myDirection)
            {
                case Direction.UP:

                    break;
                case Direction.DOWN:

                    break;
                case Direction.LEFT:

                    break;
                case Direction.RIGHT:

                    break;
            }


        }
    }
}
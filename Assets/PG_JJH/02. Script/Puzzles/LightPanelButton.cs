using Jc;
using System.Collections;
using UnityEngine;

namespace JJH
{
    public class LightPanelButton : CustomButton , IPuzzleable
    {

        public enum Direction
        {
            UP, DOWN, LEFT, RIGHT
        }


        [Tooltip("등대 머리 -> 인스펙터 참조")]
        [SerializeField]
        private GameObject lightHouseHead;

        [Tooltip("각 버튼들의 스타트 포지션")]
        [SerializeField] private Vector3 startPosition;

        [Tooltip("각 버튼 들의 다 눌린 포지션")]
        [SerializeField] private Vector3 lastPositiion;

        [Tooltip("버튼이 완전히 눌렸는지를 확인 할 bool 변수")]
        [SerializeField] private bool isComplete = false;

        [Tooltip("등대 회전의 보간이 지속 되는 시간")]
        [SerializeField] private float duration = 0.5f;

        [Tooltip("버튼 눌리는 보간이 지속 되는 시간")]
        [SerializeField] private float buttonDuration = 0.2f;

        [Tooltip("여러 버튼을 동시에 입력 할 시에 입력을 방지해 줄 bool 변수 --> 공유 하려면 스태틱?")]
        [SerializeField] private static bool isPushing = false;

        [Tooltip("버튼이 얼마나 눌릴지 체크")]
        [SerializeField] private float checkPosition = -0.045f;

        [Tooltip("코루틴을 관리 해 줄 중앙로직--> 안써도 될지도?")]
        [SerializeField] private CoroutineManager coroutineManager;

        [Tooltip("한 번에 회전할 각도 5도")]
        [SerializeField] private float rotationAngle = 5f;

        [Tooltip("최대 회전 각도 45도")]
        [SerializeField] private float limitAngle = 45f;

        [Tooltip("등대가 움직이는 소리 재생할 sound")]
        [SerializeField] AudioClip lightHouseSound;

        [Tooltip("계기판의 on off 상태 체크 --> 퍼즐 시작 전 /진행중 /후 ")]
        private bool isStart;

        //등대의 각도가 미리 정해둔 각도 내에 들어왔을 때. --> 완료 체크 할 것. 


        private void Start()
        {
            startPosition = button.localPosition;
            lastPositiion = new Vector3(startPosition.x, startPosition.y + checkPosition, startPosition.z);

            /*if(isStart==false) // 이런 식으로 manager에 접근해서 패널 이벤트 꺼주기.
            {
            // 오브젝트 끄는게 아니라 이벤트를 꺼줘야함. --> 또는 그냥 나 자신의 스크립트를 비활성화하기.
                this.gameObject.SetActive(false);
            }*/
        }

        [Tooltip("버튼 자신의 방향")]
        public Direction myDirection;

        public void UpButtonPush()
        {
            Debug.Log("업 버튼 누름");
            if (isPushing == true) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));
        }

        public void DownButtonPush()
        {
            Debug.Log("다운 버튼 누름");
            if (isPushing == true) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));


        }

        public void LeftButtonPush()
        {
            Debug.Log("왼쪽 버튼 누름");
            if (isPushing == true) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));

        }

        public void RightButtonPush()
        {
            Debug.Log("오른쪽 버튼 누름");
            if (isPushing == true) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));
        }

        public void UpButtonRelease()
        {
            Debug.Log("업 버튼 뗌");
            isPushing = false; // 어떤 버튼이든 일단 떼면 다른 버튼을 누를 수 있어야 하기 때문에 False 로 변경 
            LightHouseRotation(Direction.UP);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));
        }

        public void DownButtonRelease()
        {
            Debug.Log("다운 버튼 똄");
            isPushing = false;
            LightHouseRotation(Direction.DOWN);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));


        }

        public void RightButtonRelease()
        {
            Debug.Log("라이트 버튼 뗌");
            isPushing = false;
            LightHouseRotation(Direction.RIGHT);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));


        }
        public void LeftButtonRelease()
        {
            Debug.Log("왼쪽 버튼 뗌");
            isPushing = false;
            LightHouseRotation(Direction.LEFT);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));


        }

        // 버튼이 들어가는 모습을 구현 할 코루틴 
        private IEnumerator PushLerpRoutine(Vector3 start, Vector3 end, float duration)
        {
            Debug.Log("Push 루틴 시작");
            float elapsed = 0f;
            isPushing = true; // 누르고 있으면 다른 키 누르지 못하게 함.
            Debug.Log("ispushing ->" + isPushing);

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                button.transform.localPosition = Vector3.Lerp(start, end, t);
                elapsed += Time.fixedDeltaTime;
                yield return null;
            }

            Debug.Log("push 루틴 종료");
            button.localPosition = end; // last Position 을 받아서 저장.        
        }

        // 버튼이 돌아오는 모습을 구현 할 코루틴 
        private IEnumerator ReleaseLerpRoutine(Vector3 start, Vector3 end, float duration)
        {
            Debug.Log("Release 루틴 시작");
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                button.transform.localPosition = Vector3.Lerp(end, start, t);
                elapsed += Time.fixedDeltaTime;
                yield return null;
            }
            Debug.Log("Release 루틴 종료");
            button.localPosition = start;

        }


        // 등대 머리의 로테이션 상태 체크 
        private Quaternion end;
        private Quaternion start;

        private void LightHouseRotation(Direction myDirection)
        {

            switch (myDirection)
            {
                case Direction.UP: // 위 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, 0, -5));
                    start = lightHouseHead.transform.localRotation;

                    Debug.Log(lightHouseHead.transform.localEulerAngles+"로컬 오일러 앵글");

                    // 이게 마이너스 값으로 가면 local euler값이 355 이렇게 됨 --> 355 : -5 와 같음. 

                    if (lightHouseHead.transform.localEulerAngles.z >= 315 || lightHouseHead.transform.localEulerAngles.z <= 45  )
                    {
                        StartCoroutine(RotationRoutine(start, end, duration));

                    }
                    break;

                case Direction.DOWN: // 아래 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, 0, +5));
                    start = lightHouseHead.transform.localRotation;

                    Debug.Log(lightHouseHead.transform.localEulerAngles + "로컬 오일러 앵글");



                    if (lightHouseHead.transform.localEulerAngles.z >= 315 || lightHouseHead.transform.localEulerAngles.z <= 45)
                    {
                        StartCoroutine(RotationRoutine(start, end, duration));
                    }
                    break;

                case Direction.LEFT: // 좌 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, -5, 0));
                    start = lightHouseHead.transform.localRotation;

                    if (lightHouseHead.transform.localEulerAngles.y >= 315 || lightHouseHead.transform.localEulerAngles.y <= 45)
                    {
                        StartCoroutine(RotationRoutine(start, end, duration));
                    }                  
                    break; 

                case Direction.RIGHT: // 우 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, +5, 0));
                    start = lightHouseHead.transform.localRotation;

                    if (lightHouseHead.transform.localEulerAngles.y >= 315 || lightHouseHead.transform.localEulerAngles.y <= 45)
                    {
                        StartCoroutine(RotationRoutine(start, end, duration));
                    }
                    break;
            }
        }

        

        // 얘는 단독으로 돌려줘야하는 코루틴이니까 코루틴 매니저 이용 없이 코루틴 따로 돌려주자. 
        private IEnumerator RotationRoutine(Quaternion start, Quaternion end, float duration)
        {
            if (lightHouseSound != null) // manager를 통한 사운드 출력 --> 등대 움직이는 소리
            {
                Manager.Sound.PlaySFX(lightHouseSound);
            }

            float elapsed = 0f;
            while(elapsed < duration)
            {
                // Lerp 의 T값 증가
                float t = elapsed / duration;
                // 두 회전 사이 보간
                lightHouseHead.transform.localRotation = Quaternion.Lerp(start, end, t);
                elapsed += Time.deltaTime;
                yield return null;
                
            }

            Mathf.Clamp(elapsed, 0f, 1f);


            lightHouseHead.transform.localRotation = end;
            CheckMyAngel(); 
        }

        // 스크립트 별로 코루틴을 저장 해줘서 놓는 순간 다시 돌아오도록 하기. 
        private Coroutine activeCoroutine;
        private void StartAndStopCoroutine(IEnumerator coroutine)
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);

            }
            activeCoroutine = StartCoroutine(coroutine);
        }


        // 자신의 앵글을 체크해서 앵글이 일정값이라면 정답으로 체크 해준다. 
        private void CheckMyAngel()
        {
            // 내 앵글이 x y z 를 검사해서 x y z 가 그 해당 내부에 있으면 완료 체크를 해주면 되겠죠? 



        }

        public void RegistObject(PuzzleManager puzzle)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            throw new System.NotImplementedException();
        }

        public void ActiveSetting()
        {
            throw new System.NotImplementedException();
        }

        public void DisActiveSetting()
        {
            throw new System.NotImplementedException();
        }

        public void CompleteSetting()
        {
            throw new System.NotImplementedException();
        }
    }
}
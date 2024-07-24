using Jc;
using System.Collections;
using UnityEngine;

namespace JJH
{
    public class LightPanelButton : CustomButton, /*IPuzzleable,*/ IInteractable
    {

        public enum Direction
        {
            UP, DOWN, LEFT, RIGHT
        }

        // 이게 지금 좌 우 회전 이랑 상 하 회전이랑 서로 다른 애들을 돌려줘야함. 

        // 상 하 는 --> 그냥 그대로 등대 머리만 이용해서 회전시키자. 


        [Tooltip("등대 머리 -> 인스펙터 참조")]
        [SerializeField]
        private GameObject lightHouseHead;

        [Tooltip("등대의 컴포넌트")]
        [SerializeField]
        private LightHouseHead house;

        [Tooltip("등대 바닥 및 기둥  -> 좌 우 회전 위한")]
        [SerializeField] private GameObject lightBottom;

        [Tooltip("퍼즐 매니저")]
        [SerializeField] private Chapter2SunHole puzzleSun;


        [Tooltip("각 버튼들의 스타트 포지션")]
        [SerializeField] private Vector3 startPosition;

        [Tooltip("각 버튼 들의 다 눌린 포지션")]
        [SerializeField] private Vector3 lastPositiion;

        [Tooltip("버튼이 완전히 눌렸는지를 확인 할 bool 변수")]
        [SerializeField] private bool isComplete = false;

        [Tooltip("등대 회전의 보간이 지속 되는 시간")]
        [SerializeField] private float duration = 1f;

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

        [Tooltip("등대 회전 중 버튼 클릭 방지 위한 bool 변수")]
        [SerializeField] private static bool isRotating;


        [SerializeField] private float distance = 3f;

        /*[Tooltip("Ray로 눌리기 방지 ")]
        [SerializeField] private bool isSelecting;*/

        //등대의 각도가 미리 정해둔 각도 내에 들어왔을 때. --> 완료 체크 할 것. 

        protected override void Awake()
        {
            //RegistObject(puzzleSun);
        }

        private void Start()
        {
            startPosition = button.localPosition;
            lastPositiion = new Vector3(startPosition.x, startPosition.y + checkPosition, startPosition.z);

        }


        [Tooltip("버튼 자신의 방향")]
        public Direction myDirection;

        public void UpButtonPush()
        {
            Debug.Log($"isPushing->{isPushing}");
            Debug.Log($"isRotation ->{isRotating}");

            if (isPushing == true || isRotating == true/* || isSelecting*/) return;

            Debug.Log("업 버튼 눌림 체크");
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));
        }

        public void DownButtonPush()
        {
            if (isPushing == true || isRotating == true/* || isSelecting*/) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));

        }

        public void LeftButtonPush()
        {
            if (isPushing == true || isRotating == true/* || isSelecting*/) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));

        }

        public void RightButtonPush()
        {
            if (isPushing == true || isRotating == true/* || isSelecting*/) return;
            StartAndStopCoroutine(PushLerpRoutine(startPosition, lastPositiion, buttonDuration));
        }

        public void UpButtonRelease()
        {
            isPushing = false; // 어떤 버튼이든 일단 떼면 다른 버튼을 누를 수 있어야 하기 때문에 False 로 변경 
            LightHouseRotation(Direction.UP);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));
        }

        public void DownButtonRelease()
        {
            isPushing = false;
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));
            LightHouseRotation(Direction.DOWN);


        }

        public void RightButtonRelease()
        {
            isPushing = false;
            LightHouseRotation(Direction.RIGHT);
            StartAndStopCoroutine(ReleaseLerpRoutine(startPosition, button.localPosition, duration));


        }
        public void LeftButtonRelease()
        {
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

        private Quaternion bottomEnd;
        private Quaternion bottomStart;

        private void LightHouseRotation(Direction myDirection)
        {
            if (isRotating == true) return;

            Debug.Log("로테이션 함수 진입");
            switch (myDirection)
            {
                case Direction.UP: // 위 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, 0, -5));
                    start = lightHouseHead.transform.localRotation;

                    // 상 하 는 바닥이 회전 할 필요가 없다.
                    bottomEnd = Quaternion.Euler(lightBottom.transform.localEulerAngles + new Vector3(0, 0, 0));
                    bottomStart = lightBottom.transform.localRotation;

                    Debug.Log(lightHouseHead.transform.localEulerAngles + "로컬 오일러 앵글");

                    // 이게 마이너스 값으로 가면 local euler값이 355 이렇게 됨 --> 355 : -5 와 같음. 

                    if (lightHouseHead.transform.localEulerAngles.z >= 315 || lightHouseHead.transform.localEulerAngles.z <= 45+0.1f)
                    {
                       

                        StartCoroutine(RotationRoutine(start, end, bottomStart, bottomEnd, duration)); // 눌리는 루틴인데 이 조건이 지금 
                    }
                    break;

                case Direction.DOWN: // 아래 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(0, 0, +5));
                    start = lightHouseHead.transform.localRotation;

                    bottomEnd = Quaternion.Euler(lightBottom.transform.localEulerAngles + new Vector3(0, 0, 0));
                    bottomStart = lightBottom.transform.localRotation;

                    

                    if (lightHouseHead.transform.localEulerAngles.z >= 315 - 0.1f || lightHouseHead.transform.localEulerAngles.z <= 45)
                    {
                        
                        StartCoroutine(RotationRoutine(start, end, bottomStart, bottomEnd, duration));
                    }
                    break;

                case Direction.LEFT: // 좌 --> 이게 바닥회전은 y축 회전이 맞다. 그런데 헤드랑 같이 회전한다면? 

                    // y축 이니까 어차피 head 회전도 y축 회전하면 되고 --> 바닥도 같이 y축 회전 하면 된다.

                    // 헤드는 x축 회전이고 바닥은 y축 회전인대 이거 통일 못하나? 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(-5, 0, 0));
                    start = lightHouseHead.transform.localRotation;

                    bottomEnd = Quaternion.Euler(lightBottom.transform.localEulerAngles + new Vector3(0, -5, 0));
                    bottomStart = lightBottom.transform.localRotation;


                    if (lightHouseHead.transform.localEulerAngles.x >= 315 || lightHouseHead.transform.localEulerAngles.x <= 45+0.1f)
                    {
                        

                        StartCoroutine(RotationRoutine(start, end, bottomStart, bottomEnd, duration));
                    }
                    break;

                case Direction.RIGHT: // 우 --> 라이트 바텀 같이 회전 시키기. 

                    end = Quaternion.Euler(lightHouseHead.transform.localEulerAngles + new Vector3(+5, 0, 0));
                    start = lightHouseHead.transform.localRotation;

                    bottomEnd = Quaternion.Euler(lightBottom.transform.localEulerAngles + new Vector3(0, +5, 0));
                    bottomStart = lightBottom.transform.localRotation;


                    if (lightHouseHead.transform.localEulerAngles.x >= 315 - 0.1f || lightHouseHead.transform.localEulerAngles.x <= 45)
                    {
                        StartCoroutine(RotationRoutine(start, end, bottomStart, bottomEnd, duration));
                    }
                    break;
            }
        }

        // 얘는 단독으로 돌려줘야하는 코루틴이니까 코루틴 매니저 이용 없이 코루틴 따로 돌려주자. 
        private IEnumerator RotationRoutine(Quaternion start, Quaternion end, Quaternion bottomStart, Quaternion bottomEnd, float duration)
        {
            Debug.Log("로테이팅 루틴");
            if (lightHouseSound != null) // manager를 통한 사운드 출력 --> 등대 움직이는 소리
            {
                Manager.Sound.PlaySFX(lightHouseSound);
            }

            isRotating = true; // 로테이팅 중이면 버튼 안 눌리도록 
            float elapsed = 0f;
            while (elapsed < duration)
            {
                // Lerp 의 T값 증가
                float t = elapsed / duration;
                // 두 회전 사이 보간
                lightHouseHead.transform.localRotation = Quaternion.Lerp(start, end, t);

                lightBottom.transform.localRotation = Quaternion.Lerp(bottomStart, bottomEnd, t);

                /*if (lightHouseHead.transform.localEulerAngles.x >= 45)
                {
                    break;
                }
                if (lightHouseHead.transform.localEulerAngles.x <= 315)
                {
                    break;
                }
                if(lightHouseHead.transform.localEulerAngles.z <=315)
                {
                    break;
                }
                if (lightHouseHead.transform.localEulerAngles.z >=45)
                {
                    break;
                }*/

                elapsed += Time.deltaTime;
                yield return null;

            }

            Mathf.Clamp(elapsed, 0f, 1f);

            lightHouseHead.transform.localRotation = end;
            CheckMyAngel(end);
            isRotating = false; // 코루틴 끝나면 버튼 눌리도록 
            Debug.Log("로테이팅 루틴 종료");
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
        private void CheckMyAngel(Quaternion rotation)
        {
            // 내 앵글이 x y z 를 검사해서 x y z 가 그 해당 내부에 있으면 완료 체크를 해주면 되겠죠? 
            house.MyCheckRotation(rotation);

        }


        public void MyEnable(bool boolean)
        {
            this.enabled = boolean;
        }

       /* public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)  // 여기서 업데이트 할 거는 없다. 버튼이니까.
        {
            puzzle.UpdateCondition(index);
        }

        public void ActiveSetting()
        {
            MyEnable(true);
            Debug.Log("버튼의 액티브 세팅");
            //puzzleSun.moonPanel.gameObject.SetActive(true); --> 여기서 말고 이거는 트리거로 따로 관리 해 줘야 할 듯함.
        }

        public void DisActiveSetting()
        {
            MyEnable(false);  // 이거 왜 세팅이 안되는거지? 계속 눌리는데.. ㅠㅠ 
        }

        public void CompleteSetting() // 그냥 컴플리트 되면 눌리지 않도록만 해주자. 
        {
            MyEnable(false);
            Debug.Log("버튼의 컴플리트 세팅");
        }*/

        public float GetInteractDistance()
        {
            return 10f;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetDistanceThreshold()
        {
            return distance;
        }

        public bool GetSingleGrab()
        {
            return true;
        }
    }
}
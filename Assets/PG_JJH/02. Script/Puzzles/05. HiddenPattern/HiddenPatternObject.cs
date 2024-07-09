using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class HiddenPatternObject : XRKnob, IPuzzleable
    {
        // 회전 되어야 하는 오브젝트임. 
        // --> 1. 가장 내부에 있는 오브젝트는 움직이지 않음.
        // --> 2. 가장 내부 부터 하나 씩 맞춰야 하기 때문에 1 2 3 4 이면
        // 최초 시작은 1 3 4 가 고정되어서 움직일 수 없게 해야 한다.
        // 2번 조각과 1번이 맞으면 1 2 를 고정 시키고 3을 움직일 수 있게 한다.

        // 추가로 회전 시키는 거니까 이거 xr sample 확인하자. 
        // Advanced option 의 sorting priority 설정해주면 같은 위치에 놓여있어도 rendering 순서를 정해 줄 수 있다. 

        [Header("원반 Spec")]
        [Tooltip("각 원반 들의 아이디")]
        [SerializeField] private int patternID;

        public int PatternID { get { return patternID; } }


        [Tooltip("자식으로 있는 Hnadle의 콜라이더 --> 인스펙터에서 직접 할당 ")]
        [SerializeField] public new Collider collider;

        [Header("자식의 Renderer")]
        [SerializeField] private Renderer handleRenderer;


        [Header("퍼즐 매니저 5번 퍼즐")]
        [Tooltip("5번 퍼즐의 퍼즐 매니저")]
        [SerializeField] private HiddenPatternManager puzzle;

        [Header("유니티 이벤트 등록 필요함. --> collider")]
        [Tooltip("완료 했을 때 발동시킬 유니티 이벤트 --> 인스펙터에 등록하자.")]
        [SerializeField] private UnityEvent ColliderEvent = new UnityEvent();


        // 이거 플레이어 Hand에 CustomCheck 붙여주기. 

        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle); // 5번 매니저 퍼즐에 regist 등록. 
        }

        private void Start()
        {
            if (patternID == 1) // ID 1번인 가장 내부는 콜랑리더 꺼주고 인터액션 꺼줘서 못 만지도록 하기. 
            {
                interactionLayers = 0; // 0번이 nothing 임!!
                collider.enabled = false;
            }

            
        }

        Coroutine coroutine;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            StartCheckRoutine();

        }


        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            StopCheckRoutine();
        }

        public void StartCheckRoutine()  // raycast hit 에서 불러 줄 함수? 
        {
            if (coroutine == null) // 중복 실행 방지 위함. 
            {
                coroutine = StartCoroutine(CheckRoutine(patternID));
            }
        }

        public void StopCheckRoutine() // Hit에서 else 시에 불러줄 함수? 
        {
            if (coroutine != null)
            {
                StopCoroutine(CheckRoutine(patternID)); // OnEnterd 했을 때 
            }

        }

        private IEnumerator CheckRoutine(int _patternID)
        {
            while (true)
            {
                //  1번과 2번의...  2번과 3번의.. 3번과 4번의.. 쿼너티언 값 체크. 
                CheckMyRotation(_patternID);
                yield return new WaitForSeconds(0.1f); // 막 움직이다가 맞춰지는거 방지를 위한 0.1초 루틴 
            }
        }


        // 처음 1번과 2번이 일치하게 되면 이벤트 할당해주자. 
        private void CheckMyRotation(int ID)
        {
            if (ID == 2) // id가 2번 일 때 
            {
                if (value == 0 || value == 1) // 0 ~ 360 이므로 0 또는 1 에서 원상복구 상태라고 친다면 
                {
                    CorrectPatternRotation(ID); // ID에 맞는 오브젝트의 콜라이더 꺼주고 
                    Debug.Log("체크 마이 루틴 완료");
                    StopAllCoroutines();
                    ColliderEvent.Invoke();
                    
                }
            }
            else if (ID == 3)
            {
                if (value == 0 || value == 1)
                {
                    CorrectPatternRotation(ID); // ID에 맞는 오브젝트의 콜라이더 꺼주고 
                    Debug.Log("체크 마이 루틴 완료");
                    StopAllCoroutines();
                    ColliderEvent.Invoke();
                }
            }
            else if (ID == 4)
            {
                if (value == 0 || value == 1)
                {
                    CorrectPatternRotation(ID); // ID에 맞는 오브젝트의 콜라이더 꺼주고 
                    Debug.Log("체크 마이 루틴 완료");
                    StopAllCoroutines();
                    ColliderEvent.Invoke(); // 마지막 인보크는 overrider 한 onClearPuzzle임. 
                }
            }
        }

        // 정답을 맞추면 자신의 콜라이더를 꺼준다. 
        private void CorrectPatternRotation(int ID)
        {
            if (ID == patternID)
            {
                Debug.Log("콜라이더 꺼짐");
                collider.enabled = false;
            }
        }

        // 유니티 이벤트에 달아 둘 콜라이더 On 이벤트 
        public void ColliderOn()
        {
            Debug.Log("콜라이더 On");
            collider.enabled = true;

        }

        


        // 씬 간 저장도 생각 할 필요 없음. --> 그냥 완료 되었는지 아닌지만 하면 된다. 
        public void ActiveSetting()
        {
            collider.enabled = false;

        }

        // 추가로 완성된 상태이므로 그림도 나와 있는 상태여야 한다.
        public void CompleteSetting()
        {

            collider.enabled = false;
            value = 0; // --> 0 이나 360이 기본 상태 라고 가정하자. 

        }
        public void DisActiveSetting()
        {
            // collider.enabled = false;

            //  -> 이거 지금 실행되고 있어서 꺼주기. 
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }


        // 원반이 하나 맞을 때 마다 update 돌리기. --> 돌리고 다음 원반의 interactionLayer를 everyThing으로 변경
        // 충돌은 그대로 가져가서 못 뚫으면서 grab이 되지는 않도록 해야한다.
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index);
        }
    }

    [System.Serializable] // json으로 저장해서 씬 간 저장해 둘 쿼터니언 값 
    public class PatternData
    {
        // 원반의 현재 회전값 --> 그런데 이거 저장 해줘야하나?
        Quaternion rotation;

    }



}


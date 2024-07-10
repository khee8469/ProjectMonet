using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH

{
    public class HiddenPatternflashlight : InteractObject, IPuzzleable
    {
        // 패턴 퍼즐 용 플래시 라이트 
        // 더이상 잡지 못하도록
        [Tooltip("랜턴의 콜라이더")]
        [SerializeField] private new Collider collider;

        [Tooltip("5번 퍼즐의 퍼즐 매니저")]
        [SerializeField] private HiddenPatternManager puzzle;

        [Tooltip("랜턴이 원상 복귀 될 위치")]
        [SerializeField] private Vector3 flashLightPosition;
        [Tooltip("랜턴이 원상 복귀 될 회전값")]
        [SerializeField] private Quaternion flashLightRotation;

        [Tooltip("랜턴 불 빛 spot Light ")]
        [SerializeField] private Light spotLight;

        [Tooltip("레이캐스트 발사 위치 ")]
        [SerializeField] private Transform rayTransform;

        [Tooltip("레이 캐스트 발사를 관리 할 bool 변수")]
        [SerializeField] private bool isGrabed = false;

        [Tooltip("스포트라이트가 비추는 Renderer 참조")]
        public Renderer targetRenderer;
        
        private new void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }

        private void Start()
        {
            // 씬 시작 시 원래 위치 저장.
            flashLightPosition = transform.localPosition;
            flashLightRotation = transform.localRotation;

        }

        private void Update()
        {
            if(isGrabbed) // 잡혀 있는 상태라면 RayCast 발사  --> ray에 닿으면 문양을 나타낸다.
            {
                RaycastHit hit;

                if (Physics.Raycast(rayTransform.position, rayTransform.forward, out hit, 10))
                {
                    if(hit.transform.gameObject.layer==14) // 14번 레이어 라면.
                    {
                        hit.transform.gameObject.layer = 10; // 10번으로 변경. 
                    }
                }

                // 저장 안된다. 따로 또 저장해야 하는듯. or 어차피 hit가 누군지를 알고 있으니까. 
                // 무조건 저 판때기들 이니께.. baseMap 업해주는거 보다는 그래도 느낌 내려면
                // 이거 판 때기 몇개 동그렇게 두고 ... 박스 캐스트 같은거로 해서
                // 가운데 맞히면 한 번에 없애버리게 하고 
                
                Debug.Log($"히트 저장되나?{hit.transform.gameObject.name}");

            }

        }
        public void FlashLightReturn() // 원 위치 복귀
        {
            transform.localPosition = flashLightPosition;
            transform.localRotation = flashLightRotation;
        }


        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            // 사람에게 잡히면 랜턴 불빛이 켜져야 한다.
            // player의 손에 Custom Check 붙여주기. --> 손 판단용임. 
            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {
                Debug.Log("사람에게 잡힘");
                spotLight.enabled = true;  // player에게 닿으면 손전등의 불빛을 켜준다. 
                isGrabbed = true;
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {
                Debug.Log("사람에게 벗어남");
                spotLight.enabled = false; // 사람의 손을 벗어나면 불을 꺼준다.
                isGrabbed = false;
            }

        }

        // 퍼즐 저장에 대해서 신경 쓰지 말고
        // 그냥 시작 전 상태 완료 상태 두 가지만 생각하자. 
        public void ActiveSetting()
        {
            Debug.Log("액티브 세팅");
            collider.enabled = true; // 퍼즐이 활성화 되면 만질 수 있도록.
        }

        public void CompleteSetting()
        {
            Debug.Log("컴플리트 세팅");
            collider.enabled = false; // 더 이상 만지지 못함. 
            spotLight.enabled = false; // 불 꺼줌.
            isGrabbed = false;
        }

        public void DisActiveSetting()
        {
            // 이거 지금 발동되니까 일단 주석 처리 해놓고 시작하자.
            Debug.Log("디스액티브 세팅");
            // collider.enabled = false;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            Debug.Log("레지스트");
            puzzle.puzzleObjects.Add(this);
        }

        // 구현 할 필요 x 
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            Debug.Log("업데이트 퍼즐");
        }
    }
}



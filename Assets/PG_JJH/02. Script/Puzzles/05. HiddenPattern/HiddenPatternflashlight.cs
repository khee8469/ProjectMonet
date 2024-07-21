using Jc;
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
        [SerializeField] private HiddenPatternPuzzle puzzle;

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

        public bool IsGrabed { get { return isGrabed; } }


        [Tooltip("setActive를 변환 시켜 줄  Wall의 Layer")]
        [SerializeField] private LayerMask wallLayer;

        [Tooltip("스피어 캐스트 크기 조정")]
        [SerializeField] private float sphereSize = 0.01f;

        [Tooltip("레이캐스트 Distance")]
        [SerializeField] private float distance = 10f;

        [Tooltip("벽과 부딪힐 콜라이더 ")]
        [SerializeField] private CapsuleCollider capsuleCollider;

        private new void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }

        private void Start()
        {
            // 씬 시작 시 원래 위치 저장.
            /*flashLightPosition = transform.localPosition;
            flashLightRotation = transform.localRotation;*/

            flashLightPosition = transform.position;
            flashLightRotation = transform.rotation;
            spotLight.enabled = false;

        }

        RaycastHit[] hits = new RaycastHit[10];

        // update에서 저장된 배열을 다시 저장해 줄 배열 --> else 에서 원상복귀 시켜야 하기 때문에. 

       /* private void Update()
        {
            
            if (isGrabbed) // 잡혀 있는 상태라면 RayCast 발사  --> ray에 닿으면 문양을 나타낸다.
            {
                int hitCount = Physics.SphereCastNonAlloc(rayTransform.position, sphereSize, rayTransform.forward, hits,
                    distance, wallLayer);

                for (int i=0;i <hitCount; i++)
                {
                    hits[i].transform.GetComponent<HiddenPatternWall>();
                    
                }
            }

        }*/

        public void FlashLightReturn() // 원 위치 복귀
        {
            /*transform.localPosition = flashLightPosition;   
            transform.localRotation = flashLightRotation;*/

            // 글로벌로 빼둬서 글로벌 기준으로 해주자. 
            transform.position = flashLightPosition;
            transform.rotation = flashLightRotation;


        }


        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            // 사람에게 잡히면 랜턴 불빛이 켜져야 한다.
            // player의 손에 Custom Check 붙여주기. --> 손 판단용임. 
            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {             
                spotLight.enabled = true;  // player에게 닿으면 손전등의 불빛을 켜준다. 
                isGrabed = true;
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {               
                spotLight.enabled = false; // 사람의 손을 벗어나면 불을 꺼준다.
                isGrabed = false;
            }

        }

        // 퍼즐 저장에 대해서 신경 쓰지 말고
        // 그냥 시작 전 상태 완료 상태 두 가지만 생각하자. 
        public void ActiveSetting()
        {
            /*Debug.Log("액티브 세팅");
            collider.enabled = true; // 퍼즐이 활성화 되면 만질 수 있도록.*/
        }

        public void CompleteSetting()
        {
            /*Debug.Log("컴플리트 세팅");
            collider.enabled = false; // 더 이상 만지지 못함. 
            spotLight.enabled = false; // 불 꺼줌.
            isGrabbed = false;*/
        }

        public void DisActiveSetting()
        {
            
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            
            puzzle.puzzleObjects.Add(this);
        }

        // 구현 할 필요 x 
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            
        }
    }
}



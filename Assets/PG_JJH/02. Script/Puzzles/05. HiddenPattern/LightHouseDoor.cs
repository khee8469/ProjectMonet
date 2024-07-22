using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class LightHouseDoor : XRSocketInteractor, IPuzzleable
    {
        // 등대 입구 문 --> 열쇠가 있어야 열 수 있다.
        // 플레이어의 인벤토리를 한 번 순회해서 아이템이 있을 때
        // 문을 그냥 transform 이동 이나 힌지 조인트 써서 열어주자.
        // 그 뒤로는 문이 계속 열려 있어야 하니까. 그 부분을 저장하는 변수 하나 추가 할 것.

        // 소켓에 열쇠 넣으면 힌지 조인트 On 시키자. 이런 방식으로 진행 할 것.
        // 재천이 문 사용해서 힌지 조인트 그대로 이용하자.

        [Tooltip("문의 힌지 조인트")]
        [SerializeField] private new HingeJoint hingeJoint;

        [Tooltip("열쇠의 item ID ")]
        [SerializeField] private int keyID = 1410002;  // 임시로 내가 체크해야 하는 itemID를 가지고 있다고 치자. 

        [Tooltip("5번 퍼즐 퍼즐 매니저")]
        [SerializeField] private HiddenPatternPuzzle puzzle;

        [Tooltip("자신의 리지드 바디")]
        [SerializeField] private Rigidbody rb;


        // 소켓에 넣으면 힌지 조인트 on 해서 밀고 들어 갈 수 있도록 한다.
        // 앞으로 힌지 조인트 계속 켜주면 된다. 

        // 이거는 퍼즐이 아니니까 인터페이스를 상속하면 안될텐데 관리를 어떻게 해 줄지 고민해야함. 


        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle); // 퍼즐 등록 
            hingeJoint.useLimits = true; // 이 부분 순서 잘 생각하기. --> start 에서 안돌려도 되면 인터페이스로 옮기기.
        }

        protected override void Start()
        {
            base.Start();
      
        }


        // doorkey의 itemID를 통해서 체크한다. 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            ItemObject item = args.interactableObject as ItemObject;
            if(item!=null)
            {
                if(item.ItemID == keyID)
                {
                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    Collider col = item.GetComponent<Collider>();
                    DoorOpen();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    col.enabled = false;

                    //StartCoroutine(DelayCoroutine(item));                 
                }
            }
            base.OnSelectEntered(args);
        }

        // door는 ipuzzle 대신에 여기서 아이템을 사용했는지를 체크해서
        // 아이템을 제대로 사용한 상태면 문을 계속 열어놓기.

        private IEnumerator DelayCoroutine(ItemObject item)
        {
            Debug.Log("도어오픈");
            DoorOpen();
            yield return new WaitForSeconds(0.5f);
            item.interactionLayers = 0; // nothing
        }

        public void DoorOpen() 
        {
            Debug.Log("문이 열렸음.");





            hingeJoint.useLimits = false;
        }

        public void ActiveSetting()
        {
            hingeJoint.useLimits = true;
        }

        public void CompleteSetting()
        {
            // 소켓 비활성 
            hingeJoint.useLimits = false; // 컴플리트 상태면 문이 열려야함. 
        }

        public void DisActiveSetting() 
        {
            Debug.Log("Door의 디스액티브 세팅");
            hingeJoint.useLimits = true; 
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index);
        }
    }

}


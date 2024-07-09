using Jc;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 아이템을 체크 하기 위한 인터페이스 상속 --> 실제 아이템에 붙을 친구.
    public class InventoryItem : InteractObject, IInventory
    {
        // 실제 아이템이 가지고 있을 아이템의 기본적인 id , 타입 등의 데이터 
        [Header("아이템이 가지고 있을 데이터")]
        public InvenItem itemData; // 실제 아이템의 데이터 --가지고 있어야 데이터 쓸 수 있을듯? 
        private Transform originalParent; // 원래 부모 trasform 

        [Tooltip("현재 그랩되어 있는지를 확인하는 bool 변수")]
        private bool isGraped;

        public bool ISGraped { get { return isGraped; } }

        [Tooltip("Start 에서 저장 해 줄 아이템의 원래 스케일")]
        public Vector3 originalScale { get; set; }

        public static UnityEvent RestoreSclaeObject = new UnityEvent();

        [Tooltip("크기 구할 일 있으면 사용")]
        public new Renderer renderer;

        public Rigidbody rigid { get; set; }

        [Tooltip(" 소켓에 들어갔을 때 조정 해 줄 아이템의 스케일...")]
        [SerializeField] private Vector3 socketScale;


        [Tooltip("원래 item의 isKinematic 체크 --> 원래부터 kinematic 인지 아닌지 판단하기.")]
        [SerializeField] private bool isKinematic; // 각 아이템 마다 체크 해주기. 

        public Vector3 SocketScale { get { return socketScale; } private set { socketScale = value; } }


        protected override void Awake()
        {
            base.Awake();
            originalScale = transform.localScale;
            rigid = GetComponent<Rigidbody>();
            retainTransformParent = false; // socket 에서 해제 시에도 부모의 자식으로 붙어 있으려함 -> False
        }

        private void Start()
        {
            InitializeItemData();
            isGraped = false;
            // 아 그냥 이거 각 오브젝트마다 시작할 때 자신의 transform을 저장하고 시작하자. 

            renderer = GetComponent<Renderer>();
            //Debug.Log(renderer.bounds.size + "오브젝트들의 사이즈 체크 --> bound. size");

            trackScale = false; //스케일 조정을 위한 트랙 스케일 제거.
            SaveScale();

            throwOnDetach = false; // Kinematic item 이라면 
        }

        public void SaveScale()
        {
            // 시작 시의 자신의 로컬 스케일을 저장한다. 
            Debug.Log("자신의 로컬 스케일 " + originalScale);
            transform.localScale = originalScale;
            itemData.SaveOriginalTransform(transform); //자신의 오브젝트의 트랜스폼을 저장해준다. 

        }
        public void RestoreScale()
        {
            itemData.RestoreOriginalTransform(transform);
        }

        private void InitializeItemData()
        {
            // 잘못된 방법. 할당해주는 건 Inspector 에서 하는거고 이거를 하면 new로 빈 껍데기생성임
            // 추가로 인스펙터 할당이 아니라 데이터 테이블을 이용해서 아이템을 받고 매칭 시켜야함. 
            /*if (itemData == null) 
            {
                itemData = new InvenItem();
                Debug.Log("itemData가 null이어서 새로운 InvenItem 인스턴스를 생성했습니다.");
            }*/
        }


        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
        }

        public void AdjustScale()
        {
            // 임시 --> 나중에는 각자 스케일 조정 해 줄 예정 
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        //XR interaction을 똑같이 상속 하기 때문에 소켓에 닿앗을 때도 발동하는 문제가 발생한다. 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

        }
        // 아이템이 놓이는 순간에 슬롯 안에 있는지 확인.
        // 이거 interactor 에서 exit을 발동시킬 수 있도록 할 수가 있나? 
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            // 아니 말이 안되는게 재상속을 해서 override를 했는데 도대체 왜 이게 뜨지?
            // 여기서 selected 되던 exit 하던 소켓 내부에 있는 상황이라면 ( slot과 상호작용 하고 있다면)
            // scale의 회복을 발동 시킬 필요가 없음. 
            base.OnSelectExiting(args);
            if (args.interactorObject.transform.GetComponent<InventorySlot>()) //슬롯과 상호작용 중이라면.
            {

            }        
            else
            {
                if (originalScale.x != 0 && originalScale.y != 0 && originalScale.z != 0) return;

                RestoreScale(); // 이게 지금 무조건 발동한단 말이지 inventoryItem을 상속해서 base를 찍어버리면...
                // 그런데 그렇다고 다시 상속해서 해봤자 의미가 없는게 inventoryitem은 이 효과를 내야 하는게 맞기 때문임.
                rigid.useGravity = true;

            }
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if(args.interactorObject.transform.GetComponent<InventorySlot>())
            {
                rigid.isKinematic = true;
            }
            else
            {
                if(isKinematic ==true) // 원래 키네마틱이 true인 아이템이라면
                {
                    rigid.isKinematic = true;
                }
                else // 원래는 kinematic이 flase인 아이템 이라면
                {
                    rigid.isKinematic=false;
                }
            }
        }

        //IsGrab 쓰게 되면 사용할 코루틴 
        private IEnumerator ExitRoutine()
        {
            yield return new WaitForSeconds(0.7f);
        }


    }
}




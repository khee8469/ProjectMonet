using System.Collections;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
namespace JJH
{
    public class InventorySlot : XRSocketInteractor
    {
        // 실제 인벤토리 슬롯에 (world Space UI) 붙을 스크립트 --> 실제 인벤토리 창

        // 인벤토리에 넣는 함수 --> 이 슬롯은 player에게 붙어있기 때문에 매 씬 마다 같이 start를 돈다.
        // 그 부분을 염두에 두고 데이터를 연계하자. 

        [Tooltip("아이템의 id")]
        [SerializeField]
        private int itemID = -1; //슬롯에 아무것도 없으면 -1 할당.
        public int ItemID { get { return itemID; } 
            set 
            {
                itemID = value; 
            }
        }
        // 아이템 슬롯의 ID --> -1 로 설정 하여 MANAGER에서 자동할당 시킨다. 
        public int slotID = -1;

        [Header("슬롯에 할당된 아이템")]
        [SerializeField]
        public InventoryItem currentItem;
        
        [Tooltip("실패 시 나올 사운드")]
        [SerializeField] AudioClip InventoryAddFailure; //인벤토리 add 실패 시 재생.

        [Tooltip("아이템의 숫자를 보여줄 text")]
        [SerializeField] TextMeshProUGUI countText; // 그냥 이거 start에서 getcomponent 하고 쓰면 될듯?

        [Tooltip("아이템의 숫자")]
        private int itemCount;

        public int ItemCount { get { return itemCount; } set { itemCount = value; } }

        [Tooltip("넣기 불가능 text 출력")]
        [SerializeField] TextMeshProUGUI notAddText;

        [Tooltip("슬롯의 이미지 컬러")]
        [SerializeField] public Image slotImage;

        [Tooltip("슬롯의 기본 이미지 색깔")]
        private Color originalColor;
        public Color OriginalColor { get { return originalColor; } private set { originalColor = value; } }


        [Tooltip("레이 닿는지 확인용")]
        private bool isRayHovering { get; set; } = false;

        public SlotData slotData;

        [Tooltip("npc에게 받은 아이템이 있는지 확인해줄 bool 변수")]
        public bool isAddNPCItem;


        protected override void Awake() // 이거 처음부터 active false로 있는 상태면 Awake도 발동안함. 켜야 발동됨. 
        {
            base.Awake();
            countText = GetComponentInChildren<TextMeshProUGUI>();
            countText.text = $" ";
            // 이게 list에 slot을 할당 시키려면 처음에 켜둬서 slot 할당을 다 시키고 그게 마무리 되면 active false로 꺼줘야 한다. 
        }
        protected override void Start() // 슬롯을 인벤토리 매니저에 등록한다. 
        {
            base.Start();
            notAddText.enabled = false;
            originalColor = slotImage.color;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateSlot();
        }

        // 슬롯 데이터 업데이트
        private void UpdateSlot()
        {
            if(!Manager.Item.ItemDataDic.ContainsKey(slotID))
            {
                Debug.Log($"{slotID} : 슬롯 정보가 할당되지 않았습니다.");
                return;
            }

            //if(Manager.Item)

  
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args); // 잡을 수 있는 아이템 체크 

            if (!isRayHovering)
                return;

            InventoryItem item = args.interactableObject as InventoryItem;
            if (item != null)
            {
                item.transform.parent = transform; // 아 이 자식으로 만드는 위치를 어디서 해줘야 될지 너무 고민되는데... 
                AddItem(item);
            }
            else
            {
                if (InventoryAddFailure != null)
                {
                    //Manager.Sound.PlaySFX(InventoryAddFailure); // 노란 오류 발생 방지
                }
            }
        }

        // 이게 지금 인벤토리를 닫으면 isEnable 이 false가 되는데 그 때 인벤토리를 닫고 아이템을 제거해도 RemoveItem 이라는 함수를
        // 발동 시킬 수가 없다. 문제가 이제 isEnable 상태가 아닐 때도 exit을 해버리면 자동적으로 무조건 Exit 이 발동을 하게 된다.
        // 예외처리를 해줘야 하는데 이 때 자신이 슬롯을 할까?

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            Debug.Log("Slot -> OnSelectedExiting");

            Debug.Log($"IS HOVERING 상태 ->{isRayHovering}");

            if (!isRayHovering) return;
            Debug.Log("IS HOVERING 다음 부분 진입함");

            InventoryItem item = args.interactableObject as InventoryItem;
            if (item != null)
            {
                RemoveItem(item);
            }
        }

        public void SetRayHovering(bool isHovering)
        {
            isRayHovering = isHovering;
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!isRayHovering)
                return false;

            return base.CanSelect(interactable);
        }
        public void AddItem(InventoryItem item)
        {
            // 실제 슬롯 데이터 할당

            item.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            item.transform.localRotation = Quaternion.identity;

            Debug.Log("아이템의 소켓 스케일 작동");
            item.transform.localScale = item.SocketScale; // 아이템의 스케일 변경
            Debug.Log($"아이템의 로컬 스케일 상태 ->{item.transform.localScale}");
            item.rigid.isKinematic = true; // 키네마틱 On 

            currentItem = item;

            // 더미
            slotImage.color = Color.yellow;
            itemID = item.itemData.itemID;  
        }

        // 아이템 삭제 ( 꺼내기) --> Selected Exit 에서만 발동된다. 
        public void RemoveItem(InventoryItem item)
        {
            // 실제 슬롯 데이터 할당
            currentItem = null;

            if (item.transform.parent != null)
            {
                Debug.Log("자식 해제");
                item.transform.SetParent(null); //자식 해제 --> 소켓에서 때면 자동으로 자식이 해제가 되는데요?? 
            }
            // 이게 인벤토리를 그냥 닫으면 실행되는거라 그냥 자동적으로 원래 스케일이 리턴되는듯하다. 
            if (item.rigid != null)
            {
                Debug.Log("RemoveItem 발동");
                item.rigid.isKinematic = false; // 다시 키네마틱 꺼주기. 
            }
            slotImage.color = originalColor;

            itemID = -1;
        }

        private IEnumerator DetachAndRestore()
        {
            yield return new WaitForEndOfFrame(); // 부모 오브젝트의 상태 변경 후 한 프레임 대기
        }


        // 아이템 획득 실패 시 
        Coroutine notAddCoroutine;
        public void NotAddText()
        {
            if (notAddCoroutine == null)
            {
                notAddCoroutine = StartCoroutine(NotAddRoutine());
            }
        }
        private IEnumerator NotAddRoutine()
        {
            notAddText.enabled = true;
            yield return new WaitForSecondsRealtime(1f);
            notAddText.enabled = false;
            notAddCoroutine = null;
        }

        public void AddItemNumber()
        {
            ItemCount++;
            countText.text = $"{ItemCount}";
        }
        public void MinusItemNumber()
        {
            ItemCount--;
            countText.text = $"{ItemCount}";

            if (ItemCount <= 0)
            {
                ItemCount = 0;
                countText.text = $" "; // 0 이면 그냥 안보이게 하자. 
            }

            // 0 이라는 거는 어쨋든 아이템이 전부 빠진 상태니까 안 나오도록 고정한다. 
        }
    }
}



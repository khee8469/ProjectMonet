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
        [Tooltip("슬롯 자신의 Transform")]
        public Transform itemTransform; // 아이템의 크기 조절을 위한 트랜스폼
        // 아이템 슬롯의 ID --> -1 로 설정 하여 MANAGER에서 자동할당 시킨다. 
        public int slotID = -1;
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
            itemID = -1;
            slotData = new SlotData();
            countText = GetComponentInChildren<TextMeshProUGUI>();
            countText.text = $" ";
            itemTransform = GetComponent<Transform>();

            // 이게 list에 slot을 할당 시키려면 처음에 켜둬서 slot 할당을 다 시키고 그게 마무리 되면 active false로 꺼줘야 한다. 
        }
        protected override void Start() // 슬롯을 인벤토리 매니저에 등록한다. 
        {
            base.Start();
            Manager.Inventory.RegisterSlot(this); //THIS 시에 슬롯 아이디를 설정해줘야한다. 
            notAddText.enabled = false;
            originalColor = slotImage.color;

        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args); // 잡을 수 있는 아이템 체크 

            if (Manager.Inventory.isEnable == false)
                return;
            if (!isRayHovering)
                return;

            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                if (item is InventoryItem)
                {

                    InventoryItem inventoryItem = item as InventoryItem;
                    inventoryItem.transform.SetParent(itemTransform); // 아 이 자식으로 만드는 위치를 어디서 해줘야 될지 너무 고민되는데... 
                    AddItem(inventoryItem);
                }
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

            if (!Manager.Inventory.isEnable) return;

            /*if(args.interactorObject.transform.GetComponent<InventorySlot>()!=null)
            {
                Debug.Log("닫는 순간에 slot이 닫히면서 exit이 발동되었다.");
                return;
            }*/

            if(args.interactorObject.transform.GetComponent<CustomCheck>()!=null)
            {
                Debug.Log("사람과 상호작용함.");
            }

            Debug.Log($"IS HOVERING 상태 ->{isRayHovering}");

            if (!isRayHovering) return;
            Debug.Log("IS HOVERING 다음 부분 진입함");


            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;
                RemoveItem(inventoryItem);
            }
        }
        //ADD 하는 부분에서 추가적으로 함수를 더 부른던 해서 열거형 체크하고 데이터테이블과 연동시켜줘야한다. 

        // 소켓안에 있는 아이템과 교체 시도. 
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            // 이미 소켓 안에 아이템이 존재하는 경우에 제거 시도.

            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;
                // 리스토어가 먼지인지 이게 먼저이지 확인할것. 
            }

        }

        /*public void ChangeItem(InventoryItem _item)
        {
            InventoryItem item = null;
            foreach (Transform child in itemTransform)
            {
                item = child.GetComponent<InventoryItem>();
                if (item != null)
                {
                    Debug.Log("체인지 아이템의 remove 진입했음.");
                    RemoveItem(item);
                    break;
                }
            }
            AddItem(_item);
        }*/


        /*public override bool CanSelect(IXRSelectInteractable interactable)
        {
            IInventory item = interactable.transform.GetComponent<IInventory>();

            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;

                if (inventoryItem != null && inventoryItem.ISGraped) // 현재 그랩되어 있으면 true를 리턴한다. 
                {
                    Debug.Log("is graped 상태임");
                    return true;
                }
            }
            return false;
        }*/

        public void SetRayHovering(bool isHovering)
        {
            isRayHovering = isHovering;

        }


        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

        }


        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!isRayHovering)
                return false;

            return base.CanSelect(interactable);
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {

            return base.CanHover(interactable);

        }

        public void AddItem(InventoryItem item)
        {
            
            InventoryItem inventoryItem = item as InventoryItem; 

            inventoryItem.transform.SetParent(itemTransform); // 아 이 자식으로 만드는 위치를 어디서 해줘야 될지 너무 고민되는데... 

            inventoryItem.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            inventoryItem.transform.localRotation = Quaternion.identity;

            Debug.Log("아이템의 소켓 스케일 작동");
            inventoryItem.transform.localScale = inventoryItem.SocketScale; // 아이템의 스케일 변경
            Debug.Log($"아이템의 로컬 스케일 상태 ->{inventoryItem.transform.localScale}");
            inventoryItem.rigid.isKinematic = true; // 키네마틱 On 
            //ResizeItemToFitSlot(item.transform); // 이거 load save 할 때 써야되지 원래 크기 가지고 있어야지. 아닌가?

            slotImage.color = Color.yellow;

            itemID = inventoryItem.itemData.itemID;  

            //Manager.Inventory.UpdateInventoryData();
        }

        // 아이템 삭제 ( 꺼내기) --> Selected Exit 에서만 발동된다. 
        public void RemoveItem(InventoryItem item)
        {
           
            if (item.transform.parent != null)
            {
                Debug.Log("자식 해제");
                item.transform.SetParent(null); //자식 해제 --> 소켓에서 때면 자동으로 자식이 해제가 되는데요?? 
            }
            //item.itemData.RestoreOriginalTransform(item.transform); //오브젝트의 실제 scale을 리턴해줌. 
            // 이게 인벤토리를 그냥 닫으면 실행되는거라 그냥 자동적으로 원래 스케일이 리턴되는듯하다. 
            Rigidbody rigidbody = item.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                Debug.Log("RemoveItem 발동");
                rigidbody.isKinematic = false; // 다시 키네마틱 꺼주기. 
            }
            slotImage.color = originalColor;

            itemID = -1;

            //Manager.Inventory.UpdateInventoryData(); // 인벤토리 데이터를 업데이트
        }

        private IEnumerator DetachAndRestore()
        {
            yield return new WaitForEndOfFrame(); // 부모 오브젝트의 상태 변경 후 한 프레임 대기
        }

        // 스택용 아이템을 위한 추가 함수 --> IF문 분기 등으로 체크해주기. 
        private void StackItemAdd(InventoryItem item) // 스택 아이템이 불러질 때 이거 막 원래 있던 템이 나오니까
                                                      // 그 템을 누적해주고... 뺄 때 instantiate 하고. 추가로 막 아이템이 튀어나오면 이거 삭제해주고
                                                      // 추가로 ITEM의 id가 일치해야 스택이 가능함. 
        {
            if (ItemCount == 0) // 아직 하나도 없는 경우라면 
            {
                AddItem(item);
                ItemCount++;
                countText.text = $"{ItemCount}";
            }
            else if (ItemCount >= 1) // 1개 이상 이미 스택 아이템이 들어가 있는 경우라면 
            {
                ItemCount++; // 아이템 text와 연계 
                countText.text = $"{ItemCount}";
            }
        }
        private void StackItemRemove(InventoryItem item)
        {
            if (item.itemData.stackType == StackTypeItem.Stackable)
            {
                if (ItemCount >= 2) // 2개 이상 겹쳐있는 상태 
                {
                    // 그대로 오브젝트를 꺼내고
                    ItemCount--;

                    int id = item.itemData.itemID; // Item id 등을 받아서
                    string name = item.itemData.itemName;

                    GameObject instance = Resources.Load<GameObject>($"{name}"); //아이템 슬롯에 생성해주는 로직 
                    // 생성 후 Add 처럼 그대로 다시 슬롯에 넣기
                }
                else // 2개가 안됨 -> 1개 그냥 그대로 Remove 로직 이용.
                {

                }
            }
        }

        // 이벤트 아이템용 추가 /삭제 (바로 인벤토리에서 추가 삭제 진행해야함.) 
        // 즉 결국 비활성화 상태에서도 데이터에 접근 가능해야 하기 때문에
        // id 값 같은거를 지속적으로 manager에서 저장해둬서 id 일치 체크가 필요. (특히 아이템 제출 시에)
        // add 시에는 비활성화 상태에서 id값을 받아서 그 값에 해당하는 프리팹을 생성. 

        // 나중에 수정 할 것. 
        // 아이템이 가진 인터페이스 등 하면 되고 매개변수--> 
        // 체크를 해야 하는게 아이템의 ID를 체크해서
        // 저장을 해둔 다음에 다음에 인벤토리를 킬 때 그 SLOT위치에 그 ID를 생성하는

        public void EventItemAdd(IInventory item) // 인벤토리의 빈 공간에 바로 들어가져야함.
        {

        }

        public void EventItemRemove(IInventory item) // 인벤토리를 순회하고 id가 같으면 그 때 상태체크 필요. 
        {

        }


        public int GetItemIDInSlot(int slotID) // 이 부분 SLOT ID FOR문 안돌릴 수 있도록 수정하기. 
        {
            Manager.Inventory.UpdateInventoryData(); //인벤토리 최신화

            foreach (var item in Manager.Inventory.inventoryData.items)
            {
                if (item.slotID == slotID)
                {
                    return item.itemID;
                }
            }

            return -1; // slot에서 id가 겹치지 않으면 -1을 리턴한다. 
        }

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

            //item.itemData.itemCount = ItemCount; // itemdata의 카운트는 현재 슬롯의 카운트와 같다.

            //Debug.Log($"Plus 저장된 아이템의 카운트 data ->{item.itemData.itemCount}");

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



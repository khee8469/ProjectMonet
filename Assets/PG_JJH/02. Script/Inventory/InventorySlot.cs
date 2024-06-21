using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
namespace JJH
{
    public class InventorySlot : XRSocketInteractor
    {
        // 실제 인벤토리 슬롯에 (world Space UI) 붙을 스크립트 --> 실제 인벤토리 창

        // 인벤토리에 넣는 함수 --> 이 슬롯은 player에게 붙어있기 때문에 매 씬 마다 같이 start를 돈다.
        // 그 부분을 염두에 두고 데이터를 연계하자. 

        [Tooltip("슬롯 자신의 Transform")]
        public Transform itemTransform; // 아이템의 크기 조절을 위한 트랜스폼
        // 아이템 슬롯의 ID --> -1 로 설정 하여 MANAGER에서 자동할당 시킨다. 
        public int slotID = -1;
        [Tooltip("실패 시 나올 사운드")]
        [SerializeField] AudioClip InventoryAddFailure; //인벤토리 add 실패 시 재생.

        [Tooltip("아이템의 숫자를 보여줄 text")]
        [SerializeField] TextMeshProUGUI countText; // 그냥 이거 start에서 getcomponent 하고 쓰면 될듯?
        [Tooltip("아이템의 숫자")]
        [SerializeField] int itemCount = 0;

        [Tooltip("레이 닿는지 확인용")]
        private bool isRayHovering = false;

        public SlotData slotData;

        protected override void Awake() // 이거 처음부터 active false로 있는 상태면 Awake도 발동안함. 켜야 발동됨. 
        {
            base.Awake();
            slotData = new SlotData();
            countText = GetComponentInChildren<TextMeshProUGUI>();
            countText.text = $"{itemCount}";
            itemTransform = GetComponent<Transform>();
            Debug.Log("인벤토리 슬롯의 awake가 발동됨");
        }

        protected override void Start() // 슬롯을 인벤토리 매니저에 등록한다. 
        {
            base.Start();
            Manager.Inventory.RegisterSlot(this); //THIS 시에 슬롯 아이디를 설정해줘야한다. 
            Debug.Log("인벤토리 슬롯의 start가 발동됨.");
        }
        /*protected override void OnDestroy() // 만약 슬롯이 파괴된다면
        {
            base.OnDestroy();
            Manager.Inventory.UnregisterSlot(this);
        }*/

        public void AddSlots() // 혹시 만약 슬롯을 추가 할 일이 생긴다면...
        {
            Manager.Inventory.RegisterSlot(this);
        }


        protected override void OnHoverEntering(HoverEnterEventArgs args)
        {
            base.OnHoverEntering(args);

        }
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            Debug.Log("selecting enter");
            base.OnSelectEntering(args);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args); // 잡을 수 있는 아이템 체크 
            Debug.Log("Slot Select");
            if (Manager.Inventory.isEnable == false) return;
            if (!isRayHovering) return;

            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;

                // 아이템의 스택 가능 여부를 파악해서 Add 함수를 두가지로 나눈다. 

                inventoryItem.RestoreScale(); // 스케일을 정상 복구 하고 Add 하기. 
                if (inventoryItem.itemData.stackType == StackTypeItem.Stackable) 
                {



                    Debug.Log("이거 스택형 아이템임.");
                    Debug.Log("아이템의 id가 같음.");



                    StackItemAdd(inventoryItem);
                }
                else
                {
                    AddItem(inventoryItem); //Add 가능한 Item은 오로지 Inventory 아이템이다.
                }
            }
            else // 넣을 수 없는 아이템이라면.. 실패 사운드 재생해줘야함.. 사운드매니저 이용하자. 
            {
                if (InventoryAddFailure != null)
                {
                    Manager.Sound.PlaySFX(InventoryAddFailure); // 노란 오류 발생 방지
                }
            }
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            Debug.Log("SLOT SELECTED EXIT");
            if (!isRayHovering) return;
            // 에디터 끌 때 붉은색 발생하는데 어쩌죵?
            if (!Manager.Inventory.isEnable || !Application.isPlaying) return;

            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;

                // 아이템의 스택 가능 여부를 판단해서 Remove를 두가지로 나누기

                RemoveItem(inventoryItem);
            }
        }
        //ADD 하는 부분에서 추가적으로 함수를 더 부른던 해서 열거형 체크하고 데이터테이블과 연동시켜줘야한다. 

        // 소켓안에 있는 아이템과 교체 시도. 
        /*protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);

            Debug.Log("최초의 한 번만 작동되나?");
            // 이미 소켓 안에 아이템이 존재하는 경우에 제거 시도.

            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;
                ChangeItem(inventoryItem);
            }

        }*/

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

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!isRayHovering) return false;

            return base.CanSelect(interactable);
        }


        public void AddItem(InventoryItem item)
        {
            // item.itemData.SaveOriginalTransform(item.transform);
            // 아이템의 원래 트랜스폼을 저장
            item.transform.SetParent(itemTransform);

            item.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            item.transform.localRotation = Quaternion.identity;

            Rigidbody rigidbody = item.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
            //ResizeItemToFitSlot(item.transform); // 이거 load save 할 때 써야되지 원래 크기 가지고 있어야지. 아닌가?
            // 아이템을 슬롯의 자식으로 설정
            Manager.Inventory.UpdateInventoryData();
        }

        // 아이템 삭제 ( 꺼내기)
        public void RemoveItem(InventoryItem item)
        {

            if (item.transform.parent != null && gameObject.activeSelf)
            {
                item.transform.SetParent(null); //자식 해제 --> 소켓에서 때면 자동으로 자식이 해제가 되는데요?? 
            }
            item.itemData.RestoreOriginalTransform(item.transform); //오브젝트의 실제 scale을 리턴해줌. 
            // 이게 인벤토리를 그냥 닫으면 실행되는거라 그냥 자동적으로 원래 스케일이 리턴되는듯하다. 

            Rigidbody rigidbody = item.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.isKinematic = false; // 다시 키네마틱 꺼주기. 
            }

            Manager.Inventory.UpdateInventoryData(); // 인벤토리 데이터를 업데이트

        }

        private IEnumerator DetachAndRestore()
        {
            yield return new WaitForEndOfFrame(); // 부모 오브젝트의 상태 변경 후 한 프레임 대기
        }


        private void ItemState() // 아이템의 상태 -> 중력 , 콜라이더 등등의 컴포넌트를 변경해줄 함수.
        {

        }

        private void ResizeItemToFitSlot(Transform itemTransform)
        {
            // 슬롯의 크기를 구하기 위해 슬롯의 bounds를 사용
            Renderer slotRenderer = GetComponent<Renderer>();
            Vector3 slotSize = slotRenderer.bounds.size;
            Debug.Log($"슬롯의 크기: {slotSize}");

            Renderer itemRenderer = itemTransform.GetComponent<Renderer>();
            if (itemRenderer != null)
            {
                Vector3 itemSize = itemRenderer.bounds.size;
                Debug.Log($"아이템의 크기: {itemSize}");

                // Z축 크기는 무시하고 X, Y 축만을 고려하여 스케일을 조정
                float scaleFactorX = slotSize.x / itemSize.x;
                float scaleFactorY = slotSize.y / itemSize.y;
                float scaleFactor = Mathf.Min(scaleFactorX, scaleFactorY);

                Debug.Log($"스케일 팩터 크기 {scaleFactor}");

                // 여기서 아이템의 트랜스폼도 x y 만 하고 싶은데 그게 반영이 된건지 모르겠네... 
                Vector3 newLocalScale = itemTransform.localScale * scaleFactor;
                newLocalScale.z = itemTransform.localScale.z; // Z축 크기 유지
                itemTransform.localScale = newLocalScale;
                Debug.Log($"조정된 아이템 로컬 스케일: {itemTransform.localScale}");
            }
            else
            {
                //Debug.LogWarning("아이템에 Renderer 컴포넌트가 없습니다.");
            }
        }

        // 스택용 아이템을 위한 추가 함수 --> IF문 분기 등으로 체크해주기. 
        private void StackItemAdd(InventoryItem item) // 스택 아이템이 불러질 때 이거 막 원래 있던 템이 나오니까
            // 그 템을 누적해주고... 뺄 때 instantiate 하고. 추가로 막 아이템이 튀어나오면 이거 삭제해주고
            // 추가로 ITEM의 id가 일치해야 스택이 가능함. 
        {
            itemCount++; // 아이템 text와 연계 
            countText.text = $"{itemCount}";
            Debug.Log(itemCount);

        }
        private void StackItemRemove(InventoryItem item)
        {
            if (item.itemData.stackType == StackTypeItem.Stackable)
            {
                if (itemCount >= 2) // 2개 이상 겹쳐있는 상태 
                {
                    // 그대로 오브젝트를 꺼내고
                    itemCount--;

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
    }
}



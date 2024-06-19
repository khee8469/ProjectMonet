using System.Collections;
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

        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start() // 슬롯을 인벤토리 매니저에 등록한다. 
        {
            base.Start();
            itemTransform = GetComponent<Transform>();
            Manager.Inventory.RegisterSlot(this); //THIS 시에 슬롯 아이디를 설정해줘야한다. 
        }

        protected override void OnDestroy() // 만약 슬롯이 파괴된다면
        {
            base.OnDestroy();
            Manager.Inventory.UnregisterSlot(this);
        }

        public void AddSlots() // 혹시 만약 슬롯을 추가 할 일이 생긴다면...
        {
            Manager.Inventory.RegisterSlot(this);
        }

       /* public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!base.CanSelect(interactable)) return false;

            // InventoryItem 컴포넌트를 가져옵니다.
            IInventory item = interactable.transform.GetComponent<IInventory>();

            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;
                if (inventoryItem.ISGraped == true)
                {
                    Debug.Log($"IS GRAPED -> {inventoryItem.ISGraped}");
                    return true;
                }
                else
                {
                    Debug.Log("폴스");
                }
            }

            return false;

            
        }*/

        // 이거 Add 하는 순간에 조건 추가 해줘야함. bool 변수 같은거 써서 
        // item 에서 Grab 되었을 때 bool 변수 하나 넣고 하는 식으로 하자. 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args); // 잡을 수 있는 아이템 체크 
            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;
                AddItem(inventoryItem); //Add 가능한 Item은 오로지 Inventory 아이템이다.

                /*if (inventoryItem.ISGraped==true)
                {
                     
                }*/
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
            IInventory item = args.interactableObject.transform.GetComponent<IInventory>();
            if (item != null)
            {
                Debug.Log("아이템 꺼냄");
                InventoryItem inventoryItem = item as InventoryItem;

                RemoveItem(inventoryItem);
            }
        }

        //ADD 하는 부분에서 추가적으로 함수를 더 부른던 해서 열거형 체크하고 데이터테이블과 연동시켜줘야한다. 

        public void AddItem(InventoryItem item)
        {
            
            item.itemData.SaveOriginalTransform(item.transform);
            item.transform.SetParent(itemTransform);
            // 아이템의 원래 트랜스폼을 저장
            item.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            item.transform.localRotation = Quaternion.identity;

            Rigidbody rigidbody = item.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }

            Debug.Log("Add 성공함");

            //ResizeItemToFitSlot(item.transform); // 이거 load save 할 때 써야되지 원래 크기 가지고 있어야지. 아닌가?
            // 아이템을 슬롯의 자식으로 설정
            
            Manager.Inventory.UpdateInventoryData();
        }

        // 아이템 삭제 ( 꺼내기)
        public void RemoveItem(InventoryItem item)
        {
            //StartCoroutine(DetachAndRestore(item));

            if(item.transform.parent!= null)
            {
                item.transform.SetParent(null); //자식 해제 --> 소켓에서 때면 자동으로 자식이 해제가 되는데요?? 
                Debug.Log("자식 해제 진입");
            }
            item.itemData.RestoreOriginalTransform(item.transform); //오브젝트의 실제 scale을 리턴해줌. 

            Rigidbody rigidbody = item.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.isKinematic = false; // 다시 키네마틱 꺼주기. 
            }

            Manager.Inventory.UpdateInventoryData(); // 인벤토리 데이터를 업데이트

        }

        private IEnumerator DetachAndRestore(InventoryItem item)
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
        private void StackItemAdd()
        {

        }

        private void StackItemRemove()
        {

        }
    }
}



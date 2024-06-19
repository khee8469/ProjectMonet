using UnityEngine;
using JJH;

namespace JJH
{
    public class InventorySlot : MonoBehaviour
    {
        // 실제 인벤토리 슬롯에 (world Space UI) 붙을 스크립트 --> 실제 인벤토리 창

        // 인벤토리에 넣는 함수 --> 이 슬롯은 player에게 붙어있기 때문에 매 씬 마다 같이 start를 돈다.
        // 그 부분을 염두에 두고 데이터를 연계하자. 

        [Tooltip("오브젝트를 얼마나 줄여줄지 결정하는 변수")]
        public float shrinkedSize = 0.1f;
        [Tooltip("슬롯 자신의 Transform")]
        public Transform itemTransform; // 아이템의 크기 조절을 위한 트랜스폼
        // 아이템 슬롯의 ID --> -1 로 설정 하여 MANAGER에서 자동할당 시킨다. 
        public int slotID = -1;
        [Tooltip("실패 시 나올 사운드")]
        [SerializeField] AudioClip InventoryAddFailure; //인벤토리 add 실패 시 재생.

        private void Start() // 슬롯을 인벤토리 매니저에 등록한다. 
        {
            itemTransform= GetComponent<Transform>();

            Manager.Inventory.RegisterSlot(this); //THIS 시에 슬롯 아이디를 설정해줘야한다. 
        }

        private void OnDestroy() // 만약 슬롯이 파괴된다면
        {
            Manager.Inventory.UnregisterSlot(this);
        }

        public void AddSlots() // 혹시 만약 슬롯을 추가 할 일이 생긴다면...
        {
            Manager.Inventory.RegisterSlot(this);
        }


        private void OnTriggerEnter(Collider other)
        {
            IInventory item = other.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;

                AddItem(inventoryItem); //Add 가능한 Item은 오로지 Inventory 아이템이다. 
            }
            else // 넣을 수 없는 아이템이라면.. 실패 사운드 재생해줘야함.. 사운드매니저 이용하자. 
            {
                if (InventoryAddFailure != null)
                {
                    Manager.Sound.PlaySFX(InventoryAddFailure); // 노란 오류 발생 방지
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInventory item = other.GetComponent<IInventory>();
            if (item != null)
            {
                InventoryItem inventoryItem = item as InventoryItem;

                RemoveItem(inventoryItem); 
            }
        }

        //ADD 하는 부분에서 추가적으로 함수를 더 부른던 해서 열거형 체크하고 데이터테이블과 연동시켜줘야한다. 

        public void AddItem(InventoryItem item)
        {
            item.itemData.SaveOriginalTransform(itemTransform);
            item.transform.SetParent(itemTransform); // 자기 자신 슬롯의 자식으로 아이템을 만든는건가?
            item.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            item.transform.localRotation = Quaternion.identity;

            Debug.Log($"Item {item.itemData.itemName} added to slot {slotID} as child of {this.transform.name}");

            Rigidbody rigidbody = item.GetComponent<Rigidbody>();
            if( rigidbody != null )
            {
                rigidbody.isKinematic = true;
            }


            RectTransform slotRectTransfrom = GetComponent<RectTransform>(); // ui는 RectTransform 있음.
            if(slotRectTransfrom != null)
            {
                ResizeItemToFitSlot(item.transform , slotRectTransfrom);
            }

            Manager.Inventory.UpdateInventoryData();
        }

        // 아이템 삭제 ( 꺼내기)
        public void RemoveItem(InventoryItem item)
        {
            // 소켓에 자동으로 생기는 자식오브젝트인 [attack]를 피하기 위한 추가 로직.

            if(item.transform.parent ==this.transform) // 이 슬롯의 자식이라면. 
            {
                item.transform.SetParent(null); //자식 해제 
                item.itemData.RestoreOriginalTransform(item.transform); //오브젝트의 실제 scale을 리턴해줌. 

                Rigidbody rigidbody = item.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    rigidbody.isKinematic = false; // 다시 키네마틱 꺼주기. 
                }

                Manager.Inventory.UpdateInventoryData(); // 인벤토리 데이터를 업데이트
            }
            else
            {
                Debug.Log("No item to remove from the inventory slot.");
            }           
        }

        private void ItemState() // 아이템의 상태 -> 중력 , 콜라이더 등등의 컴포넌트를 변경해줄 함수.
        {

        }

        // ui 크기에 맞춰 아이템을 슬롯크기에 맞춘다. 
        private void ResizeItemToFitSlot(Transform itemTransform , RectTransform slotRectTransform)
        {
            Vector2 slotSize = slotRectTransform.rect.size; // rectangle의 size 측정 (사각형 형태임)
            Renderer itemRenderer = itemTransform.GetComponent<Renderer>(); //오브젝트의 Renderer 가져옴.
            // 오브젝트의 Renderer의 크기를 계산한다. 
            if(itemRenderer != null)
            {
                Vector3 itemSize = itemRenderer.bounds.size; // 렌더러의 박스 크기 바운더리

                // 슬롯 크기에 맞추기 위한 비율 계산
                // 두 비율 중 작은값을 Return하는 Min API. 
                float scaleFactor = Mathf.Min((slotSize.x / itemSize.x), (slotSize.y / itemSize.y));
                itemTransform.localScale = itemTransform.localScale * scaleFactor; //크기 줄여주기.
            }
        }
    }
}



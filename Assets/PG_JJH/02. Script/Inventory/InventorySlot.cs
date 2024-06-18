using UnityEngine;

namespace JJH
{
    public class InventorySlot : MonoBehaviour
    {
        // 실제 인벤토리 슬롯에 (world Space UI) 붙을 스크립트 --> 실제 인벤토리 창

        // 인벤토리에 넣는 함수

        public float shrinkedSize = 0.1f;
        public Transform itemTransform; // 아이템의 크기 조절을 위한 트랜스폼

        public void AddItem(InventoryItem item)
        {
            item.itemData.SaveOriginalTransform(itemTransform);
            item.transform.SetParent(itemTransform); // 자기 자신 슬롯의 자식으로 아이템을 만든는건가?
            item.transform.localPosition = Vector3.zero; // 슬롯 위치에 딱 맞도록 로컬 포지션을 0 으로 설정
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one * shrinkedSize; // 아이템 크기 축소
            Manager.Inventory.inventoryData.items.Add(item.itemData); // List에 Add 하는 작업
            Manager.Inventory.SaveInventoryData(); //Json을 통한 데이터 저장.
        }

        // 아이템 삭제 ( 꺼내기)
        public InventoryItem RemoveItem()
        {
            // 자기 자신의 transform의 첫 번 째 자식 --> 실제 오브젝트 아이템
            InventoryItem item = itemTransform.GetChild(0).GetComponent<InventoryItem>();
            item.transform.SetParent(null); // 부모 자식 관계를 해제시킴.
            item.itemData.RestoreOriginalTransform(item.transform); //아이템의 원래 transfomr으로 복구 시킴
            Manager.Inventory.inventoryData.items.Remove(item.itemData); //리스트에서 삭제.
            Manager.Inventory.SaveInventoryData(); // json을 통한 데이터 저장
            return item;

        }


        private void ItemState() // 아이템의 상태 -> 중력 , 콜라이더 등등의 컴포넌트를 변경해줄 함수.
        {

        }
        

    }
}



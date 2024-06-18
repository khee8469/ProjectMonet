using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // 인벤토리를 전체적으로 관리할 싱글톤 매니저

        public InventoryData inventoryData; // 이 데이터는 같이 Don`t Destroy를 해줘야하나? 

        private void Start()
        {
            LoadInventoryData(); // 싱글턴 이므로 게임 시작시 인벤토리 데이터를 Load 
        }

        public void SaveInventoryData() // 인벤토리의 데이터 저장
        {
            string json = inventoryData.ToJson(); // return JsonUtility.ToJson(this); 
            PlayerPrefs.SetString("InventoryData", json);
        }

        public void LoadInventoryData() // 인벤토리의 데이터를 불러옴 
        {
            string json = PlayerPrefs.GetString("InventoryData", "{}");
            inventoryData = InventoryData.FromJson(json);
            RestoreItemInScene();
        }
         
        // 로드한 인벤토리 데이터에 따라 인벤토리에 아이템 생성 및 복원
        private void RestoreItemInScene() // 실제 게임오브젝트를 생성해주는 방식으로 ?? 일단 해보고 분석하고... 
        {
            foreach ( var item in inventoryData.items) // List에 접근 
            {
                GameObject itemObject = InstantiateItem(item); //리스트에 존재하는 item을 생성
                item.RestoreOriginalTransform(itemObject.transform); // 아이템의 트랜스폼을 original로 돌리는데
                // 그냥 템창에 생성해주는게 아닌가? 이따 확인.
            }
        }


        private GameObject InstantiateItem(InvenItem itemData)
        {
            GameObject itemPrefab = Resources.Load<GameObject>($"{itemData.itemName}"); // 이름 맞추기
            if(itemPrefab!=null)
            {
                GameObject itemObject = Instantiate(itemPrefab);
                // 실제 아이템은 모두 인벤토리 아이템 스크립트를 가지고 있어햐 하기 때문에.
                InventoryItem inventoryItem = itemObject.GetComponent<InventoryItem>();
                inventoryItem.itemData = itemData;
                return itemObject;
            }

            return null; 
        }
    }

}

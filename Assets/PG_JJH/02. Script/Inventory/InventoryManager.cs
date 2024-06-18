using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEditor.Rendering;

namespace JJH
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // MonoBehavior를 상속하지 않은 일반 c# 클래스는 씬 전환되도 참조가 파괴되지 않는다.
        public InventoryData inventoryData; 

        private void Start()
        {
            LoadInventoryData(); // 싱글턴 이므로 게임 시작시 인벤토리 데이터를 Load 
        }

        public void SaveInventoryData() // 인벤토리의 데이터 저장
        {
            string json = inventoryData.ToJson(); // return JsonUtility.ToJson(this); 
            PlayerPrefs.SetString("InventoryData", json);
        }

        public void LoadInventoryData() // 인벤토리의 데이터를 json으로 로드함.
        {
            string json = PlayerPrefs.GetString("InventoryData", "{}");
            inventoryData = InventoryData.FromJson(json);
            
        }


        public void SaveCurrentInventoryState()
        {
            inventoryData.items.Clear(); // 기존 아이템 리스트 초기화 (무결성 유지)
        }



        // 로드한 인벤토리 데이터에 따라 인벤토리에 아이템 생성 및 복원
        private void RestoreItemInScene() // 실제 게임오브젝트를 생성해주는 방식으로 ?? 일단 해보고 분석하고... 
        {
            foreach ( var item in inventoryData.items) // List에 접근 
            {
                GameObject itemObject = InstantiateItem(item); //리스트에 존재하는 item을 생성
                item.RestoreOriginalTransform(itemObject.transform); 
                // 프리팹을 눈 앞에 생성해주는 방식으로 해보자. 
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

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
            
        }

        public void SaveInventoryData() // 인벤토리의 데이터 저장
        {

        }

        public void LoadInventoryData() // 인벤토리의 데이터를 불러옴 
        {

        }
         


    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 아이템 매니저
    //  : 아이템 풀링, 아이템 데이터 딕셔너리
    //  리턴하지 않는 풀링
    public class ItemManager : Singleton<ItemManager>
    {
        private Dictionary<int, ItemData> itemDataDic; // 아이템 데이터 매핑
        public Dictionary<int, ItemData> ItemDataDic{get {return itemDataDic;}}

        private List<InventorySlot> slotList; // 슬롯 딕셔너리
        public List<InventorySlot> SlotList {get {return slotList;} }

        private Dictionary<int, Stack<ItemObject>> itemPoolDic;  // 아이템 풀 딕셔너리

        private ItemObject[] itemPrefabs;     // 아이템 프리팹 리스트 (최초 로드 시 할당)

        protected override void Awake()
        {
            base.Awake();
        }

        private void CreatePool(ItemObject prefab, int size)
        {
            for(int i=0; i<size; i++)
            {
                //InventoryItem inst = Instantiate(prefab, this.transform);
            }
        }
    }
}

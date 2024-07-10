using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jc
{
    // 아이템 매니저
    //  : 아이템 풀링, 아이템 데이터 딕셔너리
    //  리턴하지 않는 풀링
    public class ItemManager : Singleton<ItemManager>
    {
        private Dictionary<int, ItemData> itemDataDic; // 아이템 데이터 매핑
        public Dictionary<int, ItemData> ItemDataDic { get { return itemDataDic; } }

        #region 슬롯 관련
        // 슬롯 리스트관리
        [SerializeField]
        private List<InventorySlot> slotList; // 슬롯리스트
        public List<InventorySlot> SlotList { get { return slotList; } }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            RegistItems();
        }

        private void RegistItems()
        {
            itemDataDic = new Dictionary<int, ItemData>();

            ItemData[] itemDatas = Resources.LoadAll<ItemData>("ItemDatas");

            for(int i =0; i< itemDatas.Length; i++)
            {
                if(itemDatas[i].itemID < 1)
                {
                    Debug.Log($"{itemDatas[i]} : ID가 할당되지 않은 아이템입니다.");
                    continue;
                }
                itemDataDic.Add(itemDatas[i].itemID, itemDatas[i]);
            }
        }

        public bool PutInItem(ItemObject item)
        {
            if (slotList == null || slotList.Count < 1)
            {
                Debug.Log("리스트에 등록된 슬롯이 존재하지 않습니다.");
                return false;
            }

            for (int i = 0; i < slotList.Count; i++)
            {
                if(slotList[i].GetItemID == -1)
                {
                    slotList[i].GetItemID = item.ItemID;
                    // 아이템 아이디 할당 후 삭제
                    Destroy(item.gameObject);
                    return true;
                }
            }

            Debug.Log("슬롯이 가득차 아이템을 넣지 못했습니다.");
            return false;
        }
    }
}

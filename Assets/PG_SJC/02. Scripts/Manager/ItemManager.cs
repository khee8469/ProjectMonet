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

        protected override void Awake()
        {
            base.Awake();
            RegistItems();
        }

        private void RegistItems()
        {
            itemDataDic = new Dictionary<int, ItemData>();

            ItemData[] itemDatas = Resources.LoadAll<ItemData>("ItemDatas");

            for (int i = 0; i < itemDatas.Length; i++)
            {
                if (itemDatas[i].itemID < 1)
                {
                    Debug.Log($"{itemDatas[i]} : ID가 할당되지 않은 아이템입니다.");
                    continue;
                }
                itemDataDic.Add(itemDatas[i].itemID, itemDatas[i]);
            }
        }

        public void GetItem(List<int> itemIDList, bool isInventroyItem)
        {
            for(int i =0; i<itemIDList.Count; i++)
            {
                int itemID = itemIDList[i];
                GetItem(itemID, isInventroyItem);
            }
        }
        public bool GetItem(int itemID, bool isInventroyItem)
        {
            // 예외처리 추가
            //  : id에 해당하는 아이템이 없을 경우
            if (!itemDataDic.ContainsKey(itemID))
            {
                Debug.Log($"{itemID}에 해당하는 아이템이 존재하지 않습니다.");
                return false;
            }

            // 인벤토리 아이템 Get
            if (isInventroyItem)
            {
                if (Manager.PlayableData.slotDataDic == null || Manager.PlayableData.slotDataDic.Count < 1)
                {
                    Debug.Log("리스트에 등록된 슬롯이 존재하지 않습니다.");
                    return false;
                }

                if (Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))
                {
                    // 이미 사용한 아이템일 경우
                    if (Manager.PlayableData.itemInfoDataDic[itemID].isClear)
                    {
                        Debug.Log($"ID-{itemID} : 아이템은 사용완료 된 아이템입니다.");
                        return false;
                    }

                    // 아이템 무결성 검사
                    foreach (int key in Manager.PlayableData.slotDataDic.Keys)
                    {
                        if (Manager.PlayableData.slotDataDic[key].slotItemID == itemID)
                        {
                            Debug.Log($"ID-{itemID} : 아이템이 이미 존재합니다.");
                            return false;
                        }
                    }
                }

                // 비어있는 슬롯 탐색 후 아이템 추가
                foreach (int key in Manager.PlayableData.slotDataDic.Keys)
                {
                    if (Manager.PlayableData.slotDataDic[key].slotItemID == -1)
                    {
                        Manager.PlayableData.slotDataDic[key] = new SlotData(key, itemID, 1);
                        GetItem(itemID);
                        return true;
                    }
                }

                Debug.Log("슬롯이 가득차 아이템을 넣지 못했습니다.");
                return false;
            }
            // 로비 스폰 아이템 Get
            else
            {
                return false;
            }
        }
        // 아이템 획득 성공 (사용전까지 로드 시 아이템을 획득 가능함.)
        private void GetItem(int itemID)
        {
            if (!Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))
            {
                Manager.PlayableData.itemInfoDataDic.Add(itemID, new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, false));
            }
            else
            {
                Manager.PlayableData.itemInfoDataDic[itemID] = new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, false);
            }
            Manager.PlayableData.SaveItemData();
        }

        // 아이템 사용 성공 (다음 로드부터는 불러오지 않음)
        public void UseSuccessItem(int itemID)
        {
            if (!Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))
                Manager.PlayableData.itemInfoDataDic.Add(itemID, new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, true));
            else
                Manager.PlayableData.itemInfoDataDic[itemID] = new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, true);

            Manager.PlayableData.SaveItemData();
        }
        public void UseSuccessItem(List<int> itemIDList)
        {
            for (int i = 0; i < itemIDList.Count; i++)
            {
                int itemID = itemIDList[i];
                if (!Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))
                    Manager.PlayableData.itemInfoDataDic.Add(itemID, new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, true));
                else
                    Manager.PlayableData.itemInfoDataDic[itemID] = new ItemInfoData(itemID, itemDataDic[itemID].itemName, true, true);
            }

            Manager.PlayableData.SaveItemData();
        }
    }
}

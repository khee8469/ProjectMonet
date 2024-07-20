using Jc;
using JJH;
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
        private Dictionary<int, ItemData> itemDataDic; // 아이템 데이터 매feat
        public Dictionary<int, ItemData> ItemDataDic { get { return itemDataDic; } }

        protected override void Awake()
        {
            base.Awake();
            RegistItems();
        }

        private void OnEnable()
        {
            InitItem();
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

        public void InitItem()
        {
            Debug.Log("아이템 Init");

            if (Manager.PlayableData.itemInfoDataDic == null) return;
            if (Manager.PlayableData.slotDataDic == null) return;

            // 획득/사용이전이지만 인벤토리에 존재하지 않는 아이템
            foreach(int itemKey in Manager.PlayableData.itemInfoDataDic.Keys)
            {
                ItemInfoData itemData = Manager.PlayableData.itemInfoDataDic[itemKey];
                // 로비 스폰 아이템 예외처리
                if (!itemData.isInventoryItem) continue;
                // 수령하지 않은 아이템 예외처리
                if (!itemData.isAccepted) continue;
                // 사용 완료한 아이템 예외처리
                if (itemData.isClear) continue;

                // 인벤토리 슬롯에 존재하는지 체크
                if (Manager.PlayableData.CheckItemInInventory(itemKey))
                    continue;

                // 비어있는 슬롯 체크
                int emptySlotID = Manager.PlayableData.FindEmptySlot();
                if (emptySlotID == -1)
                    return;

                Manager.PlayableData.slotDataDic[emptySlotID] = new SlotData(emptySlotID, itemKey, 1);
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
            // 인벤토리 아이템 Get
            if (isInventroyItem)
            {
                // 예외처리 추가
                //  : id에 해당하는 아이템이 없을 경우
                if (!itemDataDic.ContainsKey(itemID))
                {
                    Debug.Log($"{itemID}에 해당하는 아이템이 존재하지 않습니다.");
                    return false;
                }

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
                if(Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))
                {
                    if (Manager.PlayableData.itemInfoDataDic[itemID].isClear)
                    {
                        Debug.Log($"\"로비 : {itemID}\" 가 이미 존재합니다.");
                        return false;
                    }
                    Manager.PlayableData.itemInfoDataDic[itemID] = new ItemInfoData(itemID, $"\"로비 : {itemID}\"", true, false, false);
                }
                else
                {
                    Manager.PlayableData.itemInfoDataDic.Add(itemID, new ItemInfoData(itemID, $"\"로비 : {itemID}\"", true, false, false));
                }

                Manager.PlayableData.SaveItemData();
                return true;
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

        // 초반에 프리팹데이터 매핑을 시켜놓지 않은 물감 아이템들을 딕셔너리에서 제거해주는 함수 
        public void NotItemDataDicUseSucessItem(int itemID)
        {
            if(!Manager.PlayableData.itemInfoDataDic.ContainsKey(itemID))                
            {
                // isClear 부분을 true로 바꿔주면 PaintManager에서 On 하지 않음. 
                Manager.PlayableData.itemInfoDataDic.Add(itemID,new ItemInfoData(itemID, $"\"로비 : {itemID}\"", true, true, false));
            }
            else
            {
                Manager.PlayableData.itemInfoDataDic [itemID] = new ItemInfoData(itemID, $"\"로비 : {itemID}\"", true, true, false);
            }

            Manager.PlayableData.SaveItemData();
        }

    }
}

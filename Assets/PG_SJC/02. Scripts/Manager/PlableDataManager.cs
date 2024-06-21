using JetBrains.Annotations;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{

    /// <summary>
    /// 게임 플레이 데이터 세이브/로드
    /// </summary>
    public class PlableDataManager : Singleton<PlableDataManager>
    {
        private Dictionary<int, QuestStateData> questStateData;                 // 퀘스트 상태 데이터
        public Dictionary<int, QuestStateData> QuestStateData {get { return questStateData; } }

        private Dictionary<int, InventorySlotStateData> inventorySlotStateData; // 인벤토리 슬롯 상태 데이터
        public Dictionary<int, InventorySlotStateData> InventorySlotStateData {get { return inventorySlotStateData; }}

        public void SaveQuestData(Quest quest)
        {
            int questID = quest.QuestID;

        }
        public void SaveSlotData(InventorySlot slot)
        {
            int slotID = slot.slotID;

        }
    }
}

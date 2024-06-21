using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 인벤토리 슬롯 상태 데이터 (세이브/로드 전용)
    /// </summary>
    [Serializable]
    public struct InventorySlotStateData
    {
        [Header("슬롯 ID")]
        public int slotID;

        [Header("아이템 ID")]
        public int itemID;

        [Header("아이템 카운트")]
        public int count;

        public InventorySlotStateData(int slotID, int itemID, int count)
        {
            this.slotID = slotID;
            this.itemID = itemID;
            this.count = count;
        }
    }
}
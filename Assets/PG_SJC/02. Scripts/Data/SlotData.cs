using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct SlotData
    {
        [Header("슬롯 ID")]
        public int slotID;

        [Header("슬롯 내부 아이템 ID")]
        public int slotItemID;

        [Header("아이템 개수")]
        public int itemCount;

        public SlotData(int slotID, int slotItemID, int itemCount)
        {
            this.slotID = slotID;
            this.slotItemID = slotItemID;
            this.itemCount = itemCount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct ItemInfoData
    {
        [Header("아이템 ID")]
        public int itemID;

        [Header("아이템 명")]
        public string itemName;

        [Header("아이템 획득 여부")]
        public bool isAccepted;

        [Header("아이템 사용완료 여부")]
        public bool isClear;

        [Header("인벤토리 전용 아이템")]
        public bool isInventoryItem;

        public ItemInfoData(int itemID, string itemName, bool isAccepted, bool isClear, bool isInventoryItem = true)
        {
            this.itemID = itemID;
            this.itemName = itemName;
            this.isAccepted = isAccepted;
            this.isClear = isClear;
            this.isInventoryItem = isInventoryItem;
        }
    }
}

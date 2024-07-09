using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    public struct ItemData
    {
        [Header("아이템 획득 여부")]
        public bool isAccepted;

        [Header("아이템 사용완료 여부")]
        public bool isClear;

        [Header("아이템 ID")]
        public int itemID;
    }
}

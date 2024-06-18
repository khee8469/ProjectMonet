using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    // 아이템을 체크 하기 위한 인터페이스 상속 
    public class InventoryItem : InteractObject , IInventory
    {
        // 실제 아이템이 가지고 있을 아이템의 기본적인 id , 타입 등의 데이터 
        public InvenItem InvenItem; // InvenItem이 가지고 있는 데이터
        private Transform originalParent; // 원래 부모 trasform 
    }
}



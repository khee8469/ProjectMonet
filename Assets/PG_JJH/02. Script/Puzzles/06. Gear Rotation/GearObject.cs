using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class GearObject : InventoryItem
    {
        // 인벤토리에 들어가고 잡을 수 있는 아이템

        [Header("할당된 아이템의 ID")] // 프리팹 연동 필요함. 
        [SerializeField] private int gearID;

        [Header("퍼즐 매니저 에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;

        





       
    }

}

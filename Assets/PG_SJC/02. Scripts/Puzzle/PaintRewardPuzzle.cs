using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PaintRewardPuzzle : PuzzleManager
    {
        [Header("활성화 할 페인트 아이템 ID")]
        [SerializeField]
        private int paintItemID;

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
            
            if(Manager.PlableData.paintDataList.ContainsKey(paintItemID))
            {
                Debug.Log($"ID({paintItemID}) : 이미 활성화된 페인트 아이템이 존재합니다.");
                return;
            }
            // 페인트 아이템 활성화
            Manager.PlableData.paintDataList.Add(paintItemID, true);
        }
    }
}
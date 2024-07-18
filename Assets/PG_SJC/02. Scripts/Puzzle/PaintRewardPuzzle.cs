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
            Debug.Log(123);
            // 아이템 획득
            Manager.Item.GetItem(paintItemID, false);
        }
    }
}
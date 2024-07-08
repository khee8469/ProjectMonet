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




            //현재 퍼즐 상태 성공으로 변경
            Manager.PlableData.PuzzleSuccessCheck[PuzzleIndex] = true;

            //모든 퍼즐을 완료했는지 확인
            for (int i = 0; i < Manager.PlableData.PuzzleSuccessCheck.Count; i++)
            {
                //실패한게 있으면 취소
                if (Manager.PlableData.PuzzleSuccessCheck[i] == false)
                    return;
            }

            //모든 퍼즐이 완성된 상태
            Debug.Log("퍼즐 올클리어");
        }
    }
}
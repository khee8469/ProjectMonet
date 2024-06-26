using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PuzzleFlow : MonoBehaviour
    {
        [Header("에디터 세팅")]
        private int rewardItemID;

        [Header("밸런싱")]
        [Header("퍼즐 클리어 조건 체크")]
        [SerializeField]
        private bool[] conditions;

        // 조건 성공
        public void UpdateCondition(int index = -1)
        {
            if(index == -1) // 조건 인덱스가 설정되지 않았다면 바로 클리어
            {
                OnClearPuzzle();
                return;
            }

            // 조건 인덱스 범위 초과
            if (conditions.Length <= index)
                return;

            // 조건 인덱스 상태변경
            conditions[index] = true;
        }

        public void OnClearPuzzle()
        {
            // 아이템 추가
        }
    }
}

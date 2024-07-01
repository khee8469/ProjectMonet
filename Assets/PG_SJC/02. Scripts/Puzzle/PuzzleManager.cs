using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class PuzzleManager : MonoBehaviour
    {
        [Header("에디터 세팅")]
        private int rewardItemID;

        [Header("퍼즐 인덱스")]
        private int puzzleIndex;

        [Header("퍼즐 클리어 조건 체크")]
        [SerializeField]
        private bool[] conditions;

        [Header("퍼즐 클리어 액션")]
        public UnityEvent OnClear;

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private bool isClear = false;

        protected virtual void OnEnable()
        {
            // 퍼즐 로드 세팅
            LoadSetting();
        }

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

            if (CheckCondition())
                OnClearPuzzle();
        }
        private bool CheckCondition()
        {
            for(int i =0; i<conditions.Length; i++)
            {
                if (!conditions[i])
                    return false;
            }
            return true;
        }

        public virtual void OnClearPuzzle()
        {
            // 아이템 추가
            OnClear?.Invoke();
            isClear = true;
        }

        public virtual void LoadSetting()
        {

        }
    }
}

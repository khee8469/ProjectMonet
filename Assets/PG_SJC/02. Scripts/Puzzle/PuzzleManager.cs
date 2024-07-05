using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public enum PuzzleState { DisActive = 0, Proceed = 1, Clear = 2 }
    public class PuzzleManager : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private int rewardItemID;

        [Header("퍼즐 id")]
        [SerializeField]
        private int puzzleIndex;

        [Header("퍼즐 진행상태")]
        public PuzzleState state = PuzzleState.DisActive;

        [Header("연계된 퍼즐 오브젝트 (퍼즐 상태를 업데이트하는 오브젝트)")]
        public List<IPuzzleable> puzzleObjects = new List<IPuzzleable>();

        [Header("퍼즐 클리어 조건 체크")]
        [SerializeField]
        private bool[] conditions;

        [Header("퍼즐 클리어 액션")]
        public UnityEvent OnClear;

        [Header("연계 퀘스트 ID")]
        public int linkedQuestID = -1;

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private bool isClear = false;

        private Quest linkedQuest;

        private void OnEnable()
        {
            if (linkedQuestID != -1)
                linkedQuestID -= DataID.QUEST;  // 퀘스트 ID 매핑

            if (!Manager.Quest.QuestDic.ContainsKey(linkedQuestID))
            {
                Debug.Log($"(Puzzle : {this} / QuestID : {linkedQuestID}) : 할당된 퀘스트가 없습니다.");
                return;
            }

            linkedQuest = Manager.Quest.QuestDic[linkedQuestID];

            // 이미 완료된 퀘스트가 아닐경우 콜백 등록
            if (linkedQuest.State != QuestState.Complete)
                linkedQuest.OnChangeState += PuzzleSetting;

            // 최초 퀘스트 상태에 따른 퍼즐 상태를 설정
            InitPuzzleSetting(linkedQuest.State);
        }
        // 퀘스트의 초기 상태에 따른 처리
        // 현재 객체에서 진행
        private void InitPuzzleSetting(QuestState state)
        {
            if (puzzleObjects == null || puzzleObjects.Count < 1)
                return;

            switch (state)
            {
                case QuestState.Active:
                case QuestState.DisActive:
                    // 로딩된 퍼즐 데이터가 없을 경우
                    if (!LoadPuzzleData())
                    {
                        // 퍼즐 비활성화
                        foreach (IPuzzleable ob in puzzleObjects)
                            ob.DisActiveSetting();    // 모든 퍼즐 오브젝트 비활성화
                    }
                    break;
                case QuestState.Proceed:
                    // 퍼즐 활성화
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.ActiveSetting();    // 모든 퍼즐 오브젝트 활성화
                    break;
                case QuestState.Clear:      // 수락 대기 상태
                case QuestState.Complete:   // 이미 완료된 상태
                    // 퍼즐 완료
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.CompleteSetting();    // 모든 퍼즐 오브젝트 활성화
                    break;
                default:
                    break;
            }
        }

        // 퍼즐 세팅 (퀘스트 객체에서 호출)
        // 퀘스트의 상태 변화에 따른 퍼즐의 비활성화/활성화
        public void PuzzleSetting(QuestState state)
        {
            switch (state)
            {
                case QuestState.DisActive:
                    // 퍼즐 비활성화
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.DisActiveSetting();    // 모든 퍼즐 오브젝트 비활성화
                    break;
                case QuestState.Proceed:
                    // 퍼즐 활성화
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.ActiveSetting();    // 모든 퍼즐 오브젝트 활성화
                    break;
                default:
                    break;
            }
        }

        // 조건 성공
        public void UpdateCondition(int index = -1, bool condition = true)
        {
            if (index == -1) // 조건 인덱스가 설정되지 않았다면 바로 클리어
            {
                OnClearPuzzle();
                return;
            }

            // 조건 인덱스 범위 초과
            if (conditions.Length <= index)
                return;

            // 조건 인덱스 상태변경
            conditions[index] = condition;

            if (CheckCondition())
                OnClearPuzzle();
        }
        private bool CheckCondition()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i])
                    return false;
            }
            return true;
        }

        public virtual void OnClearPuzzle()
        {
            Debug.Log($"{puzzleIndex}번 퍼즐 성공");

            // 아이템 추가
            OnClear?.Invoke();
            isClear = true;

            // 퀘스트 예외처리 (이미 수락대기인 퀘스트 or 완료한 퀘스트)
            if (linkedQuest != null &&
                linkedQuest.State != QuestState.Clear &&
                linkedQuest.State != QuestState.Complete)
                linkedQuest.ChangeState(QuestState.Clear);
        }
    }
}

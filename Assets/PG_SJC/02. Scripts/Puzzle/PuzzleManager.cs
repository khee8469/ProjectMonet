using JJH;
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
        private int puzzleID = -1;
        public int PuzzleID { get { return puzzleID; }}

        [Header("퍼즐 진행상태")]
        [SerializeField]
        private PuzzleState state = PuzzleState.DisActive;
        public PuzzleState State
        {
            get { return state; }
            set
            {
                state = value;
                SavePuzzleData();
            }
        }

        [Header("연계된 퍼즐 오브젝트 (퍼즐 상태를 업데이트하는 오브젝트)")]
        public List<IPuzzleable> puzzleObjects = new List<IPuzzleable>();

        [Header("퍼즐 클리어 조건 체크")]
        [SerializeField]
        private bool[] conditions;

        [Header("퍼즐 클리어 액션")]
        public UnityEvent OnClear;

        [Header("활성화 퀘스트 ID")]
        public int activeQuestID = -1;

        [Header("클리어 퀘스트 ID")]
        public int clearQuestID = -1;

        [Space(5)]
        [Header("밸런싱")]
        [SerializeField]
        private bool isClear = false;

        private void OnEnable()
        {
            // 로드된 퍼즐 상태를 기반으로 퍼즐 최초세팅 진행
            InitPuzzleSetting();

            if (!Manager.Quest.QuestDic.ContainsKey(activeQuestID))
            {
                Debug.Log($"(Puzzle : {this} / QuestID : {activeQuestID}) : 할당된 퀘스트가 없습니다.");
                return;
            }

            Quest activeQuest = Manager.Quest.QuestDic[activeQuestID];

            // 이미 완료된 퀘스트가 아닐경우 콜백 등록
            if (activeQuest.State != QuestState.Complete)
                activeQuest.OnChangeState += PuzzleSetting;
        }

        // 로드된 데이터를 기반으로 퍼즐 최초세팅
        private void InitPuzzleSetting()
        {
            // 속한 퍼즐 오브젝트가 존재하지 않는다면 리턴
            if (puzzleObjects == null || puzzleObjects.Count < 1)
            {
                Debug.Log($"{puzzleID} 퍼즐 매니저에 속한 퍼즐 오브젝트가 존재하지 않습니다.");
                return;
            }

            if(!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleID))
            {
                Debug.Log($"{puzzleID} 퍼즐의 로드된 데이터가 존재하지 않습니다.");

                // 로드된 정보가 없을 경우 기본 세팅으로 할당
                foreach (IPuzzleable ob in puzzleObjects)
                    ob.DisActiveSetting();

                this.state = PuzzleState.DisActive;
                return;
            }

            // 로드된 데이터는 프로퍼티를 사용하여 다시 저장하지 않음.
            // State -> this.state
            switch(Manager.PlayableData.puzzleDataDic[puzzleID])
            {
                case PuzzleState.DisActive:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.DisActiveSetting();
                    
                    this.state = PuzzleState.DisActive;
                    break;
                case PuzzleState.Proceed:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.ActiveSetting();

                    this.state = PuzzleState.Proceed;
                    break;
                case PuzzleState.Clear:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.CompleteSetting();

                    this.state = PuzzleState.Clear;
                    break;
            }
        }

        // 퍼즐 세팅 (퀘스트 객체에서 호출)
        // 퀘스트의 상태 변화에 따른 퍼즐의 비활성화/활성화
        public virtual void PuzzleSetting(QuestState state)
        {
            switch (state)
            {
                case QuestState.DisActive:
                    // 퍼즐 비활성화
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.DisActiveSetting();    // 모든 퍼즐 오브젝트 비활성화

                    State = PuzzleState.DisActive;
                    break;
                case QuestState.Proceed:
                    // 퍼즐 활성화
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.ActiveSetting();    // 모든 퍼즐 오브젝트 활성화

                    State = PuzzleState.Proceed;
                    break;
                default:
                    break;
            }
        }

        public virtual void ChangeState(PuzzleState state)
        {
            if (this.state == state) 
                return;

            switch(state)
            {
                case PuzzleState.DisActive:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.DisActiveSetting();
                    break;
                case PuzzleState.Proceed:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.ActiveSetting();
                    break;
                case PuzzleState.Clear:
                    foreach (IPuzzleable ob in puzzleObjects)
                        ob.CompleteSetting();
                    break;
            }
            State = state;
        }

        // 퍼즐 데이터 로드
        // 퍼즐 데이터 세이브
        private void SavePuzzleData()
        {
            if (!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleID))
                Manager.PlayableData.puzzleDataDic.Add(puzzleID, State);
            else
                Manager.PlayableData.puzzleDataDic[puzzleID] = State;

            Manager.PlayableData.SavePuzzleData();
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
            Debug.Log($"ID : {puzzleID} 퍼즐 성공");

            // 아이템 추가
            OnClear?.Invoke();
            isClear = true;

            State = PuzzleState.Clear;

            if (clearQuestID == -1) return;
            Quest clearQuest = Manager.Quest.QuestDic[clearQuestID];

            // 퀘스트 예외처리 (이미 수락대기인 퀘스트 or 완료한 퀘스트)
            if (clearQuest == null) 
                return;
            if (clearQuest.State == QuestState.Clear || clearQuest.State == QuestState.Complete)
                return;

            clearQuest.ChangeState(QuestState.Clear);
        }
    }
}

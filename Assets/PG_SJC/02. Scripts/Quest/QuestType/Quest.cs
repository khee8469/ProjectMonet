using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    /// <summary>
    /// 퀘스트 최상위 클래스
    /// </summary>
    [Serializable]
    public class Quest : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [Tooltip("퀘스트 ID")]
        [SerializeField]
        protected int questID;
        public int QuestID { get { return questID; }}

        [SerializeField]
        protected int linkedEndQuestID;
        public int LinkedEndQuestID { get {return linkedEndQuestID; }}  // 마지막으로 링크된 퀘스트 ID

        [Tooltip("퀘스트 상태")]
        [SerializeField]
        protected QuestState state;
        public QuestState State { get { return state; } set { state = value; } }

        [Tooltip("퀘스트 데이터")]
        [SerializeField]
        protected QuestData questData;
        public QuestData QuestData { get { return questData; } set { questData = value; } }

        [SerializeField]
        protected bool isSaveQuest;
        public bool IsSaveQuest {get { return isSaveQuest; } set { isSaveQuest = value; } }

        [Tooltip("퀘스트 수주 나레이션 리스트")]
        public List<NarrtionData> receiveNarrations;

        [Tooltip("퀘스트 클리어 나레이션 리스트")]
        public List<NarrtionData> clearNarrations;

        public UnityAction<QuestState> OnChangeState;

        public UnityAction<Quest> OnClearQuest;

        // 퀘스트 상태변경
        public void ChangeState(QuestState state)
        {
            this.state = state;
            OnChangeState?.Invoke(state);
            switch (state)
            {
                case QuestState.DisActive:
                    DisActiveQuest();
                    break;
                case QuestState.Active:
                    ActiveQuest();
                    break;
                case QuestState.Proceed:
                    ProceedQuest();
                    break;
                case QuestState.Clear:
                    ClearQuest();
                    break;
                case QuestState.Complete:
                    CompleteQuest();
                    break;
            }
        }
        // 퀘스트 비활성화
        protected virtual void DisActiveQuest()
        {

        }
        // 다음 퀘스트 활성화
        protected virtual void ActiveNextQuest(int id)
        {
            // 다음 퀘스트 탐색
            Quest nextQuest = Manager.Quest.GetQuest(id);
            if(nextQuest == null)
            {
                Debug.Log($"{questData.id} : 다음 퀘스트가 존재하지 않습니다.");
                return;
            }
            // 다음 퀘스트 활성화
            nextQuest.ChangeState(QuestState.Active);
        }
        // 퀘스트 활성화 시
        protected virtual void ActiveQuest()
        {
            Debug.Log($"퀘스트 {questID} : 가 활성화 되었습니다.");
        }
        // 퀘스트 수주 시
        protected virtual void ProceedQuest()
        {
            Debug.Log($"퀘스트 {questID} : 가 진행됩니다.");

            // 링크 퀘스트의 경우 현재 ID 기준 다음 퀘스트를 활성화
            if(questData.type == QuestType.Link)
            {
                ActiveNextQuest(questID + 1);
            }
        }
        // 퀘스트 클리어 시 (수락 대기)
        protected virtual void ClearQuest()
        {
            Debug.Log($"퀘스트 {questID} : 가 완료되었습니다.");

            // 스테이지 데이터 저장
            if (isSaveQuest)
                Manager.PlableData.SaveStageData();

            // 자동 클리어 퀘스트의 경우 바로 Complete 상태로 전환 (보상 수령)
            if (questData.type == QuestType.AutoClear)
            {
                ChangeState(QuestState.Complete);
            }
        }
        // 퀘스트 완료
        protected virtual void CompleteQuest()
        {
            OnClearQuest?.Invoke(this);

            if (questData.next_id < 1)
            {
                // 연계된 퀘스트가 있다면 연계된 퀘스트 클리어
                return;
            }

            // 연결된 다음 퀘스트 활성화
            ActiveNextQuest(questData.next_id);
        }

        public void OnClearLinkedQuest(Quest quest)
        {
            // 링크된 퀘스트가 모두 클리어된 경우
            ChangeState(QuestState.Clear);
        }
    }
}
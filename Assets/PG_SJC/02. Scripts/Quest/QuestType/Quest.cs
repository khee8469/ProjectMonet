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
        [Tooltip("퀘스트 종류")]
        [SerializeField]
        protected QuestType questType;
        public QuestType QuestType { get { return questType; }}

        [Tooltip("퀘스트 상태")]
        [SerializeField]
        protected QuestState state;
        public QuestState State { get { return state; } }

        [Tooltip("퀘스트 ID")]
        [SerializeField]
        protected int questID;
        public int QuestID { get { return questID; } }

        [Tooltip("퀘스트 데이터")]
        [SerializeField]
        protected QuestData questData;
        public QuestData QuestData { get { return questData; }}

        public UnityAction<QuestState> OnChangeState;

        protected virtual void Awake()
        {
            if(questData == null)
            {
                Debug.Log($"{questID} : 퀘스트 데이터가 할당되어있지 않습니다.");
            }
            // 퀘스트 ID 세팅
            questID = QuestData.id;
        }

        // 퀘스트 상태변경
        public void ChangeState(QuestState state)
        {
            OnChangeState?.Invoke(state);

            switch(state)
            {
                case QuestState.Active:
                    OnActiveQuest();
                    break;
                case QuestState.Proceed:
                    OnProceedQuest();
                    break;
                case QuestState.Clear:
                    OnClearQuest();
                    break;
                case QuestState.DisActive:
                    OnDisActiveQuest();
                    break;
            }
        }

        // 다음 퀘스트 활성화
        protected virtual void ActiveNextQuest()
        {
            // 다음 퀘스트 탐색
            Quest nextQuest = Manager.Quest.GetQuest(questID + 1);
            if(nextQuest == null)
            {
                Debug.Log($"{questID} : 다음 퀘스트가 존재하지 않습니다.");
                return;
            }
            // 다음 퀘스트 활성화
            nextQuest.ChangeState(QuestState.Active);
        }

        // 퀘스트 활성화 시
        protected virtual void OnActiveQuest(){ }

        // 퀘스트 수주 시
        protected virtual void OnProceedQuest(){ }

        // 퀘스트 클리어 시 (수락 대기)
        protected virtual void OnClearQuest(){ }

        // 퀘스트 수락 이후 비활성화
        protected virtual void OnDisActiveQuest(){ }
    }
}
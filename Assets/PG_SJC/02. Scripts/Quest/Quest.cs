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
        [Tooltip("퀘스트 상태")]
        [SerializeField]
        protected QuestState state;
        public QuestState State { get { return state; } }

        [Tooltip("퀘스트 ID")]
        [SerializeField]
        protected QuestID questID;
        public QuestID QuestID { get { return questID; } }

        [Tooltip("퀘스트 데이터")]
        [SerializeField]
        protected QuestData questData;
        public QuestData QuestData { get { return questData; }}

        public UnityAction OnChangeState;

        // 퀘스트 상태변경
        protected void ChangeState(QuestState state)
        {
            OnChangeState?.Invoke();
        }
    }
}
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
        // 퀘스트를 수락한 경우 할당
        [HideInInspector]
        public PlayerQuestController owner;

        [Tooltip("퀘스트 상태")]
        [SerializeField]
        protected QuestState state;
        public QuestState State { get { return state; } }

        [Tooltip("퀘스트 ID")]
        [SerializeField]
        protected QuestID questID;
        public QuestID QuestID { get { return questID; } }  

        public virtual void OnClearQuest()
        {
            owner.ClearQuest(questID);
        }
        public virtual void OnActiveQuest()
        {

        }
    }
}
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
        // 퀘스트 타입
        public enum QuestType{ Main = 0, Sub, UnLock}
        // 퀘스트 상태타입
        public enum QuestState{ DisActive = 0, Active, Proceed, Success, Rewarded}

        [Tooltip("퀘스트 제공 NPC")]
        [SerializeField]
        protected NPC ownerNPC;
        public NPC OwnerNPC { get { return ownerNPC; } }

        [Tooltip("퀘스트 상태")]
        [SerializeField]
        protected QuestState state;
        public QuestState State { get { return state; } }

        public virtual void OnClearQuest()
        {

        }
        public virtual void OnActiveQuest()
        {
        
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 퀘스트 ID (데이터 테이블 연동)
    public enum QuestID { }
    
    // 퀘스트 상태타입
    public enum QuestState { DisActive = 0, Active, Proceed }

    public enum QuestType { Main = 0, Sub}

    public class QuestManager : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<QuestID, Quest> questDic;
        public Dictionary<QuestID, Quest> QuestDic { get { return questDic; } } 


    }
}
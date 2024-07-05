using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public enum StageClearType{ Proceed = -1, Clear = 3, Complete = 4}
    /// <summary>
    /// 스테이지와 연결된 퀘스트 데이터
    /// </summary>
    [Serializable]
    public struct StageData
    {
        [Header("연결된 퀘스트 ID 리스트")]
        public List<int> linkedQuestID;
        [Header("스테이지 분기점이 되는 퀘스트 ID")]
        public int baseQuestID;
        public StageData(List<int> linkedQuestID, int baseQuestID) 
        { 
            this.linkedQuestID = linkedQuestID;
            this.baseQuestID = baseQuestID;
        }


    }
}


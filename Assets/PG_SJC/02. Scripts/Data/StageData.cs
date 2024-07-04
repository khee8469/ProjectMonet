using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct StageData
    {
        [Header("연결된 퀘스트 ID 리스트")]
        public List<int> linkedQuestID;
        public StageData(List<int> linkedQuestID) { this.linkedQuestID = linkedQuestID;}
    }
}


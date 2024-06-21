using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 퀘스트 Plable Data 저장 전용 구조체
    /// </summary>
    [Serializable]
    public struct QuestStateData
    {
        [Header("퀘스트 ID")]
        public int questID;

        [Header("퀘스트 상태")]
        public int stateNum;

        public QuestStateData(int questID, int stateNum)
        {
            this.questID = questID;
            this.stateNum = stateNum;
        }
    }
}

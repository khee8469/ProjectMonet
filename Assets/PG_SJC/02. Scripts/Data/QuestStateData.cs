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
        public int id_quest;

        [Header("퀘스트 상태")]
        public int progress;

        public QuestStateData(int id_quest, int progress)
        {
            this.id_quest = id_quest;
            this.progress = progress;
        }
    }
}

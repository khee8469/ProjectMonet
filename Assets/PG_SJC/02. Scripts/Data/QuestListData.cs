using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct QuestListData
    {
        [Header("퀘스트 리스트 ID")]
        public int id;

        [Header("퀘스트 ID")]
        public int id_quest;

        [Header("진행도")]
        public int progress;

        public QuestListData(int id, int id_quest, int progress)
        {
            this.id = id;
            this.id_quest = id_quest;
            this.progress = progress;
        }
    }
}

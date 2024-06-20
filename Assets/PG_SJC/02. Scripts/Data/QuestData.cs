using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct QuestData
    {
        [Header("퀘스트 ID")]
        public int id;

        [Header("퀘스트 제목")]
        public string questName;

        [Header("퀘스트 타입")]
        public QuestType type;

        [Header("NPC ID")]
        public int npcID;

        [Header("보상 아이템 ID")]
        public int rewardItemID;

        [Header("수락 나레이션 번들 ID")]
        public int receiveNarrationBundleID;

        [Header("완료 나레이션 번들 ID")]
        public int clearNarrationBundleID;
    }
}

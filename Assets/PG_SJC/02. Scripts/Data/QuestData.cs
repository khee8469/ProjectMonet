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

        [Header("퀘스트 진행 타입")]
        public QuestProccedType proceedType;

        [Header("수주 NPC ID")]
        public int acceptNPCID;

        [Header("클리어 NPC ID")]
        public int clearNPCID;

        [Header("완료 시 활성화 퀘스트 ID")]
        public int next_id;

        [Header("보상 아이템 ID")]
        public int rewardItemID;

        [Header("수락 나레이션 번들 ID")]
        public int receiveNarrationBundleID;

        [Header("완료 나레이션 번들 ID")]
        public int clearNarrationBundleID;
    }
}

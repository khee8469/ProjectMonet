using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data", order = int.MaxValue)]
    public class QuestData : ScriptableObject
    {
        [Header("퀘스트 제목")]
        public string title;

        [Header("퀘스트 ID")]
        public int id;

        [Header("퀘스트 종류")]
        public QuestType questType;

        [Header("NPC 이름")]
        public string npcName;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct NPCData
    {
        [Header("NPC ID")]
        public int id;

        [Header("NPC 이름")]
        public string npcName;

        [Header("기본 나레이션 번들 ID 리스트")]
        public List<int> narrationBundleID;

        [Header("퀘스트 ID 리스트")]
        public List<int> questIDList;
    }
}

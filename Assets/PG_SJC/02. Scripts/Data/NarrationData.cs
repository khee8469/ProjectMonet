using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct NarrtionData
    {
        [Header("NPC ID")]
        public int npcID;
        [Header("나레이션 텍스트")]
        public string text;
    }
}
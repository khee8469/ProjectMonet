using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct NarrtionData
    {
        public int npcID;
        public string text;

        public NarrtionData(int npcID, string text)
        {
            this.npcID = npcID;
            this.text = text;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class LayerManager : Singleton<LayerManager>
    {
        [Header("NPC")]
        public LayerMask npcLM;

        [Header("Wall (?ш린 李⑹떆 ?ㅻ툕?앺듃)")]
        public LayerMask wallLM;
    }
}
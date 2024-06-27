using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class LayerManager : Singleton<LayerManager>
    {
        [Header("NPC")]
        public LayerMask npcLM;

        [Header("InteractorNPC")]
        public LayerMask InNpcLM;

        [Header("Wall")]
        public LayerMask wallLM;

        [Header("Inventory Slot")]
        public LayerMask slotLM;
    }
}
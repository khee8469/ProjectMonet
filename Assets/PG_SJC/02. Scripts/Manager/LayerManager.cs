using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [Header("NPC")]
    public LayerMask npcLM;

    [Header("Wall (?¬ê¸° ì°©ì‹œ ?¤ë¸Œ?íŠ¸)")]
    public LayerMask wallLM;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [Header("NPC")]
    public LayerMask npcLM;

    [Header("Wall (크기 착시 오브젝트)")]
    public LayerMask wallLM;
}

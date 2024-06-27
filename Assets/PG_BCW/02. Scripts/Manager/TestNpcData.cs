using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TestNPCData
{
    [Header("NPC ID")]
    public int id;

    [Header("NPC 이름")]
    public string npcName;

    [Header("퀘스트 ID 리스트")]
    public List<int> questIDList;
}

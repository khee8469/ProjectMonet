using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct MiniatureData
{
    [Header("씬 번호")]
    public int sceneId;
    [Header("미니어처 ID")]
    public int miniatureId;

    [Header("미니어처 포지션")]
    public float xPosition;
    public float yPosition;
    public float zPosition;


    public MiniatureData(int sceneId, int miniatureId, Vector3 position)
    {
        this.sceneId = sceneId;
        this.miniatureId = miniatureId;
        xPosition = position.x;
        yPosition = position.x;
        zPosition = position.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : MonoBehaviour
{
    //[Tooltip("미니어처와 Npc들 위치 연동을 위한 좌표 저장소")]
    private Dictionary<string, Vector3> savePosition = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> SavePosition 
    { get { return savePosition; } set { savePosition = value; } }

}

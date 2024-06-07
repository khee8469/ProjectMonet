using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : MonoBehaviour
{
    //위치 데이터 저장용
    private Dictionary<string, Vector3> savePosition = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> SavePosition 
    { get { return savePosition; } set { savePosition = value; } }

}

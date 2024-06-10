using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : MonoBehaviour
{
    //[Tooltip("미니어처와 Npc들 위치 연동을 위한 좌표 저장소")]
    /*private Dictionary<string, Vector3> savePosition = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> SavePosition 
    { get { return savePosition; } set { savePosition = value; } }*/
    

    private List<Dictionary<string, Vector3>> savePosition = new List<Dictionary<string, Vector3>>();
    public List<Dictionary<string, Vector3>> SavePosition { get { return savePosition; } }

    private Dictionary<string, Vector3> scene_1Position = new Dictionary<string,Vector3>();
    private Dictionary<string, Vector3> scene_2Position = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> scene_3Position = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> scene_4Position = new Dictionary<string, Vector3>();

    private void Awake()
    {
        savePosition.Add(scene_1Position);
        savePosition.Add(scene_2Position);
        savePosition.Add(scene_3Position);
        savePosition.Add(scene_4Position);
    }
}

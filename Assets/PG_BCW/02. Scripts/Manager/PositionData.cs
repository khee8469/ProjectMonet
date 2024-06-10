using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : MonoBehaviour
{
    //각 씬의 딕셔너리를 리스트로 저장
    private List<Dictionary<string, Vector3>> savePosition = new List<Dictionary<string, Vector3>>();
    public List<Dictionary<string, Vector3>> SavePosition { get { return savePosition; } }

    //리스트에 저장되어잇는 각씬의 위치 데이터들
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

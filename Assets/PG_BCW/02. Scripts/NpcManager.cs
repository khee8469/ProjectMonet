using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [Tooltip("Npc 리스트")]
    [SerializeField] List<Npc> npcs;
    public List<Npc> Npcs {  get { return npcs; } }

    int sceneNumber;

    private void Awake()
    {
        npcs = GetComponentsInChildren<Npc>().ToList<Npc>();
    }

    //위치 재배치
    private void Start()
    {
        if (gameObject.name == "Scene_1")
            sceneNumber = 0;
        else if (gameObject.name == "Scene_2")
            sceneNumber = 1;
        else if (gameObject.name == "Scene_3")
            sceneNumber = 2;
        else if (gameObject.name == "Scene_4")
            sceneNumber = 3;
        else
            Debug.Log("ERROR");

        Debug.Log(sceneNumber);

        SetNpcPosition();
    }

    private void SetNpcPosition()
    {
        //dictionary에 위치데이터 사용
        foreach (Npc npc in npcs)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(npc.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].TryGetValue(npc.name, out Vector3 position);
                npc.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(npc.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(npc.gameObject.name, npc.transform.localPosition);
            }
        }
    }
}

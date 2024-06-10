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

    /*//몇번째 씬과 미니어쳐인지 확인용
    [SerializeField] private PositionSyncManager.MiniatureNum miniatureNum;
    public PositionSyncManager.MiniatureNum MiniatureNum { get { return miniatureNum; } }

    int sceneNumber;

    private void Awake()
    {
        npcs = GetComponentsInChildren<Npc>().ToList();
    }

    //위치 재배치
    private void Start()
    {
        //몇번 씬정보인지
        sceneNumber = (int)miniatureNum;

        SetNpcPosition();
    }

    private void SetNpcPosition()
    {
        var positionData = PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber];
        foreach (Npc npc in npcs)
        {
            if (positionData.ContainsKey(npc.name))
            {
                npc.transform.localPosition = new Vector3(positionData[npc.name].x, 0.5f, positionData[npc.name].z);
            }
            else
            {
                positionData[npc.name] = npc.transform.localPosition;
            }
        }
    }*/
}

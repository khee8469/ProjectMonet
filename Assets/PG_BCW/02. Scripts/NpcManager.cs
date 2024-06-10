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

    //몇번째 씬과 미니어쳐인지 확인용
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
        switch (miniatureNum)
        {
            case PositionSyncManager.MiniatureNum.First: sceneNumber = 0; break;
            case PositionSyncManager.MiniatureNum.Second: sceneNumber = 1; break;
            case PositionSyncManager.MiniatureNum.Third: sceneNumber = 2; break;
            case PositionSyncManager.MiniatureNum.Fourth: sceneNumber = 3; break;
        }

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

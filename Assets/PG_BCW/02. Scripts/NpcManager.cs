using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [Tooltip("Npc 리스트")]
    [SerializeField] List<NpcMove> npcs;
    public List<NpcMove> Npcs {  get { return npcs; } }

    private void Awake()
    {
        npcs = GetComponentsInChildren<NpcMove>().ToList<NpcMove>();
    }

    //위치 재배치
    private void Start()
    {
        //dictionary에 위치데이터 사용
        foreach (NpcMove npc in npcs)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(npc.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.TryGetValue(npc.name, out Vector3 position);
                npc.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(npc.name))
            {
                continue;
            }
        }
    }
}

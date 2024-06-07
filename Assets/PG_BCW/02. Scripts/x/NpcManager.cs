using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [SerializeField] List<Npc> npcs;
    public List<Npc> Npcs {  get { return npcs; } }


    private void Start()
    {
        //miniature = GetComponentsInChildren<Transform>();
        npcs = GetComponentsInChildren<Npc>().ToList<Npc>();

        //dictionary에 위치데이터 사용
        foreach (Npc npc in npcs)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(npc.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.TryGetValue(npc.name, out Vector3 good);
                npc.transform.localPosition = new Vector3(good.x, 0.5f, good.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(npc.name))
            {
                continue;
            }
        }
    }
}

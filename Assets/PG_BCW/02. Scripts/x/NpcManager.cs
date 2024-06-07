using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [SerializeField] List<Npc> npcs;
    public List<Npc> Npcs {  get { return npcs; } }


    bool a;


    private void Start()
    {
        //miniature = GetComponentsInChildren<Transform>();
        npcs = GetComponentsInChildren<Npc>().ToList<Npc>();
    }

    private void Update()
    {
        if(!a)
        foreach (Npc npc in npcs)
        {
            //데이터가 잇으면
            if (PositionSyncManager.instance.positionData.SavePosition.ContainsKey(npc.name))
            {
                PositionSyncManager.instance.positionData.SavePosition.TryGetValue(npc.name, out Vector3 good);
                npc.transform.localPosition = new Vector3(good.x,0.5f,good.z);
            }
        }
        a = true;
    }
    private void OnEnable()
    {
        
    }
}

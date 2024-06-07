using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] List<Npc> npc;
    public List<Npc> Npc {  get { return npc; } }

    private void Start()
    {
        //miniature = GetComponentsInChildren<Transform>();
        npc = GetComponentsInChildren<Npc>().ToList<Npc>();
    }
}

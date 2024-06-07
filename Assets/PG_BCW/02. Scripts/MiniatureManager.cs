using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{

    [SerializeField] List<MiniatureMove> miniatures;
    [SerializeField] NpcManager npc;
    int i = 0;

    private void Start()
    {
        miniatures = GetComponentsInChildren<MiniatureMove>().ToList<MiniatureMove>();

        foreach (MiniatureMove miniature in miniatures)
        {
            miniature.Cube = npc.Npc[i].GetComponent<Npc>();
            miniature.transform.position = npc.Npc[i].transform.localPosition;

            i++;
        }
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }


}

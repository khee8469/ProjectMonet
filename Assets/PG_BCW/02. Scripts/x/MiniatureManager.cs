using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{
    [SerializeField] List<MiniatureMove> miniatures;
    public List<MiniatureMove> Miniatures {  get { return miniatures; } }

    private void Awake()
    {
        miniatures = GetComponentsInChildren<MiniatureMove>().ToList<MiniatureMove>();
    }
}

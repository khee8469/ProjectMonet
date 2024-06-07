using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{
    [SerializeField] List<MiniatureMove> miniatures;

    private void Start()
    {
        miniatures = GetComponentsInChildren<MiniatureMove>().ToList<MiniatureMove>();


    }

    public void SetPosition()
    {

    }
}

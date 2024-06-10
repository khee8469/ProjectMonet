using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{
    [Tooltip("미니어처 리스트")]
    [SerializeField] List<Miniature> miniatures;
    public List<Miniature> Miniatures { get { return miniatures; } }

    //몇번째 씬과 미니어쳐인지 확인용
    [SerializeField] private PositionSyncManager.MiniatureNum miniatureNum;
    public PositionSyncManager.MiniatureNum MiniatureNum { get { return miniatureNum; } }

    int sceneNumber;

    private void Awake()
    {
        miniatures = GetComponentsInChildren<Miniature>().ToList();
    }

    private void Start()
    {
        switch (miniatureNum)
        {
            case PositionSyncManager.MiniatureNum.First: sceneNumber = 0; break;
            case PositionSyncManager.MiniatureNum.Second: sceneNumber = 1; break;
            case PositionSyncManager.MiniatureNum.Third: sceneNumber = 2; break;
            case PositionSyncManager.MiniatureNum.Fourth: sceneNumber = 3; break;
        }

        SetMiniPosition();
    }

    private void SetMiniPosition()
    {
        foreach (Miniature miniature in miniatures)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(miniature.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].TryGetValue(miniature.name, out Vector3 position);
                miniature.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(miniature.name))
            {

                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(miniature.gameObject.name, miniature.transform.localPosition);
            }
        }
    }
}

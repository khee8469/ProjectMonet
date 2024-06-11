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
        //몇번 씬정보인지
        sceneNumber = (int)miniatureNum;

        SetMiniPosition();
    }

    private void SetMiniPosition()
    {
        foreach (Miniature miniature in miniatures)
        {
            var positionData = PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber];
            //데이터가 잇으면
            if (positionData.ContainsKey(miniature.name))
            {
                miniature.transform.localPosition = new Vector3(positionData[miniature.name].x, 0.5f, positionData[miniature.name].z);
            }
            //데이터가 없으면
            else
            {
                positionData[miniature.name] = miniature.transform.localPosition;
            }
        }
    }


    
}

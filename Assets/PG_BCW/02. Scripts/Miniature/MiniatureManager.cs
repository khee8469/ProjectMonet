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


    private void Awake()
    {
        miniatures = GetComponentsInChildren<Miniature>().ToList();
    }

    private void Start()
    {
        SetMiniPosition();
    }

    //씬 로드시 미니어처들 위치 지정
    private void SetMiniPosition()
    {
        foreach (Miniature miniature in miniatures)
        {
            var positionData = PositionSyncManager.Instance.PositionData.SavePosition[(int)miniatureNum];
            //데이터가 잇으면
            if (positionData.ContainsKey(miniature.name))
            {
                miniature.transform.localPosition = new Vector3(positionData[miniature.name].x, 0, positionData[miniature.name].z);
            }
            //데이터가 없으면
            else
            {
                positionData[miniature.name] = miniature.transform.localPosition;
            }
        }
    }

    private void PositionFixation()
    {
        
    }


}

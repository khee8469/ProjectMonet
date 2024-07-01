using Jc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum MiniatureNum { First, Second, Third, Fourth, }
public class MiniatureManager : PuzzleManager
{
    [Tooltip("미니어처 리스트")]
    [SerializeField] List<Miniature> miniatures;
    public List<Miniature> Miniatures { get { return miniatures; } }

    //몇번째 씬과 미니어쳐인지 확인용
    [SerializeField] private MiniatureNum miniatureNum;
    public MiniatureNum MiniatureNum { get {  return miniatureNum; } }
    


    private void Awake()
    {
        miniatures = GetComponentsInChildren<Miniature>().ToList();
    }

    private void Start()
    {
        Manager.PlableData.LoadMiniatureData();
        SetMiniPosition();
    }

    //씬 로드시 미니어처들 위치 지정
    private void SetMiniPosition()
    {
        var positionData = Manager.PlableData.PositionData.SavePosition_3;
        foreach (Miniature miniature in miniatures)
        {
            //데이터가 잇으면
            if (positionData.ContainsKey(miniature.Id))
            {
                miniature.transform.localPosition = new Vector3(positionData[miniature.Id].x, positionData[miniature.Id].y, positionData[miniature.Id].z);
            }
            //데이터가 없으면 초기화
            else
            {
                positionData[miniature.Id] = miniature.transform.localPosition;
            }
        }
    }
}

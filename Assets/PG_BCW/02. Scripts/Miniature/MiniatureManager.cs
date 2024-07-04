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

    // 바닥으로 설정한 레이어
    [SerializeField] LayerMask miniatureMapLayer;


    private void Awake()
    {
        miniatures = GetComponentsInChildren<Miniature>().ToList();
    }

    private void Start()
    {
        //데이터가 잇으면 로드
        Manager.PlableData.LoadMiniatureData();

        SetMiniPosition();
    }

    //씬 로드시 미니어처들 위치 지정
    private void SetMiniPosition()
    {
        var positionData = Manager.PlableData.PositionData.SavePosition_3;

        foreach (Miniature miniature in miniatures)
        {
            //데이터가 잇으면 미니어처 위치 세팅
            if (positionData.ContainsKey(miniature.Id))
            {
                //부모크기에 따라 위치 보정 로드
                float x = miniature.transform.parent.localScale.x * positionData[miniature.Id].x;
                float y = miniature.transform.parent.localScale.y * positionData[miniature.Id].y;
                float z = miniature.transform.parent.localScale.z * positionData[miniature.Id].z;

                miniature.transform.localPosition = new Vector3(x, 10, z);

                RaycastHit hit;
                Physics.Raycast(miniature.transform.position, Vector3.down, out hit, 1000, miniatureMapLayer);
                Debug.Log(Physics.Raycast(miniature.transform.position, Vector3.down, out hit, 1000, miniatureMapLayer));

                // 히트 포인트를 로컬 좌표로 변환
                Vector3 localHitPoint = miniature.transform.parent.InverseTransformPoint(hit.point);
                // 로컬 좌표로 변환된 값으로 설정
                miniature.transform.localPosition = new Vector3(x, localHitPoint.y, z);
            }
            //데이터가 없으면 미니어처 위치 딕셔너리 저장
            else
            {
                positionData[miniature.Id] = miniature.transform.localPosition; //미니어처 시작위치 저장
            }
        }
        Manager.PlableData.SavePositionData(miniatures); //저장용 구조체 세팅
    }
}

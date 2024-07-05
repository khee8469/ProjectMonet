using Jc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum MiniatureNum { First, Second, Third, Fourth, }
public class MiniatureManager : MonoBehaviour
{
    [Header("MiniatureManager")]
    [SerializeField] int sceneNumber;
    public int SceneNumber { get { return sceneNumber; } }

    [Tooltip("미니어처 리스트")]
    [SerializeField] List<Miniature> miniatures;
    public List<Miniature> Miniatures { get { return miniatures; } }

    [SerializeField] List<bool> miniatureQuest;
    public  List<bool> MiniatureQuest { get {  return miniatureQuest; } }

    // 바닥으로 설정한 레이어
    [SerializeField] LayerMask miniatureMapLayer;


    private void Awake()
    {
        //미니어처 리스트
        miniatures = GetComponentsInChildren<Miniature>().ToList();

        if(miniatures.Count == 0)
        {
            Debug.Log($"시작");
            //미니어처 위치 저장
            foreach (var miniature in miniatures)
            {
                Manager.PlableData.SavePosition[miniature.Id] = miniature.transform.localPosition;
                
                miniatureQuest[miniature.Id] = false;
            }
        }     
    }

    private void OnEnable()
    {
        SetMiniPosition();
    }

    //씬 로드시 미니어처들 위치 지정
    private void SetMiniPosition()
    {
        var positionData = Manager.PlableData.SavePosition;

        foreach (Miniature miniature in miniatures)
        {
            //데이터가 잇으면 미니어처 위치 세팅
            if (positionData.ContainsKey(miniature.Id))
            {
                //부모크기에 따라 위치 보정 로드
                float xLoad = miniature.transform.parent.localScale.x * positionData[miniature.Id].x;
                float yLoad = miniature.transform.parent.localScale.y * positionData[miniature.Id].y;
                float zLoad = miniature.transform.parent.localScale.z * positionData[miniature.Id].z;

                miniature.transform.localPosition = new Vector3(xLoad, 10, zLoad);

                RaycastHit hit;
                if(Physics.Raycast(miniature.transform.position, Vector3.down, out hit, 1000, miniatureMapLayer))
                {
                    // 히트 포인트를 로컬 좌표로 변환
                    Vector3 localHitPoint = miniature.transform.parent.InverseTransformPoint(hit.point);
                    // 로컬 좌표로 변환된 값으로 설정
                    miniature.transform.localPosition = localHitPoint;

                    //부모크기에 따라 위치 보정 저장
                    Manager.PlableData.MiniaturePositionSave(miniature, positionData);
                }
                else
                {
                    miniature.transform.localPosition = positionData[miniature.Id];

                    //부모크기에 따라 위치 보정 저장
                    Manager.PlableData.MiniaturePositionSave(miniature, positionData);
                }
            }
            //데이터가 없으면 미니어처 위치 딕셔너리 저장
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(miniature.transform.position, Vector3.down, out hit, 1000, miniatureMapLayer))
                {
                    // 히트 포인트를 로컬 좌표로 변환
                    Vector3 localHitPoint = miniature.transform.parent.InverseTransformPoint(hit.point);
                    // 로컬 좌표로 변환된 값으로 설정
                    miniature.transform.localPosition = localHitPoint;

                    //부모크기에 따라 위치 보정 저장
                    Manager.PlableData.MiniaturePositionSave(miniature, positionData);
                }
            }
        }
    }
}

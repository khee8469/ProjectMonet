using System.Collections.Generic;
using UnityEngine;

public class CreateAreaCollider : MonoBehaviour
{
    // 오브젝트의 전체 면적에 영역마다 콜라이더 생성

    [Tooltip("책상 콜라이더")]
    [SerializeField] private GameObject table;
    
    [Tooltip("타올 컴포넌트")]
    [SerializeField] private Taoru taoru;
    
    [Tooltip("생성할 박스 콜라이더 프리팹")]
    [SerializeField] private GameObject colliderPrefab;
    
    [Tooltip("박스 콜라이더의 높이")]
    [SerializeField] private float colliderY;
    
    [Tooltip("박스 콜라이더의 가로 크기")]
    [SerializeField] private float colliderX;
    
    [Tooltip("박스 콜라이더의 세로 크기")]
    [SerializeField] private float colliderZ;

    [Tooltip("박스 콜라이더의 세로 크기")]
    [SerializeField] private Vector3 rotation;

    [Tooltip("Cleaning Collider Check")]
    //수건이 부딪힌 부분은 true로 변경 전체중 80퍼 true로 바뀌면 클리어
    private Dictionary<GameObject, bool> cleaningCheck = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> CleaningCheck { get { return cleaningCheck; } set { cleaningCheck = value; } }  

    [Tooltip("생성된 박스콜라이더 트리거 레이어")]
    //CreatedCollider의 OnTrigger layerMask 설정
    [SerializeField] private LayerMask CreatedColliderLayer;

    [Tooltip("청소가 끝났는지 확인용")]
    private bool isSuccess;
    public bool IsSuccess { get { return isSuccess; } set { isSuccess = value; } }

    void Start()
    {
        BoxCollider tableRenderer = table.GetComponent<BoxCollider>();
        

        Vector3 tableSize = tableRenderer.bounds.size;
        Vector3 tablePosition = tableRenderer.bounds.center;

        // 시작 위치
        Vector3 startPosition = new Vector3(
            tablePosition.x - tableSize.x * 0.5f,
            tablePosition.y + tableSize.y * 0.5f,
            tablePosition.z - tableSize.z * 0.5f
        );

        // X축과 Z축을 기준으로 박스 콜라이더를 배치
        for (float x = 0; x < tableSize.x - colliderX * 0.5f; x += colliderX) //가로
        {
            for (float z = 0; z < tableSize.z - colliderZ * 0.5f; z += colliderZ) //세로
            {
                //콜라이더 위치 설정
                Vector3 colliderPosition = new Vector3(startPosition.x + colliderX*0.5f + x , startPosition.y, startPosition.z + colliderZ * 0.5f + z);

                //콜라이더 생성
                GameObject colliderObject = Instantiate(colliderPrefab, colliderPosition, Quaternion.identity);
                colliderObject.transform.parent = table.transform;
                BoxCollider boxCollider = colliderObject.GetComponent<BoxCollider>();
                // 콜라이더 크기 설정
                boxCollider.size = new Vector3(colliderX, colliderY, colliderZ);
                // 트리거 설정
                boxCollider.isTrigger = true;

                // 충돌감지용 컴포넌트 부착
                CreatedCollider cleaningCollider = colliderObject.AddComponent<CreatedCollider>();
                // CreatedCollider의 변수값 지정
                cleaningCollider.createAreaCollider = this;
                cleaningCollider.TriggerLayer = CreatedColliderLayer;
                // 딕셔너리에 저장
                cleaningCheck.Add(colliderObject, false);
            }
        }

        transform.rotation = Quaternion.Euler(rotation);
    }



    //콜라이더에 충돌하여 false를 true로 바꾸면 업데이트에서 딕셔너리 전체 검사해서 80퍼 이상 true면
    public void CheckCleaningProgress()
    {
        // 청소 진행 상태를 확인하고 80%가 청소되었는지 확인
        int cleanedCount = 0;
        foreach (var entry in cleaningCheck)
        {
            if (entry.Value)
            {
                cleanedCount++;
            }
        }

        float cleanedPercentage = (float)cleanedCount / cleaningCheck.Count;
        if (cleanedPercentage >= 0.80f)
        {
            Debug.Log("식탁 청소 완료");
            //콜라이더들 삭제
            cleaningCheck.Clear();
            //라인렌더러 삭제
            Destroy(taoru.LineObject);
            taoru.IsSuccess = true;
            //isSuccess = true;
        }
    }
}

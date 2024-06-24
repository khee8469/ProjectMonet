using System.Collections.Generic;
using UnityEngine;

public class CreateAreaCollider : MonoBehaviour
{
    // 오브젝트의 전체 면적에 영역마다 콜라이더 생성

    [SerializeField] public GameObject table; // 책상 오브젝트
    [SerializeField] public GameObject colliderPrefab; // 생성할 박스 콜라이더 프리팹
    [SerializeField] public float colliderWidth = 0.1f; // 박스 콜라이더의 가로 크기
    [SerializeField] public float colliderHeight = 0.1f; // 박스 콜라이더의 세로 크기

    //수건이 부딪힌 부분은 true로 변경 전체중 80퍼 true로 바뀌면 클리어
    [SerializeField] public Dictionary<GameObject, bool> cleaningCheck = new Dictionary<GameObject, bool>();

    //CreatedCollider의 OnTrigger layerMask 설정
    [SerializeField] LayerMask CreatedColliderLayer;


    void Start()
    {
        Renderer tableRenderer = table.GetComponent<Renderer>();
        // 오브젝트 윗 면적
        //float tableSize = tableRenderer.bounds.size.x * tableRenderer.bounds.size.z;
        Vector3 tableSize = tableRenderer.bounds.size;
        Vector3 tablePosition = tableRenderer.bounds.center;

        // 책상의 좌상단 위치를 계산
        Vector3 startPosition = new Vector3(
            tablePosition.x - tableSize.x * 0.5f,
            tablePosition.y + tableSize.y * 0.5f,
            tablePosition.z - tableSize.z * 0.5f
        );

        // X축과 Z축을 기준으로 박스 콜라이더를 배치
        for (float x = 0; x < tableSize.x - colliderWidth * 0.5f; x += colliderWidth) //가로
        {
            for (float z = 0; z < tableSize.z - colliderHeight * 0.5f; z += colliderHeight) //세로
            {
                //콜라이더 위치 설정
                Vector3 colliderPosition = new Vector3(startPosition.x + colliderWidth*0.5f + x, startPosition.y, startPosition.z + colliderHeight * 0.5f + z);

                //콜라이더 생성
                GameObject colliderObject = Instantiate(colliderPrefab, colliderPosition, Quaternion.identity);
                colliderObject.transform.parent = table.transform;
                BoxCollider boxCollider = colliderObject.GetComponent<BoxCollider>();
                // 콜라이더 크기 설정
                boxCollider.size = new Vector3(colliderWidth, 0.1f, colliderHeight);
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
        if (cleanedPercentage >= 0.8f)
        {
            Debug.Log("청소 완료!");
        }
    }
}

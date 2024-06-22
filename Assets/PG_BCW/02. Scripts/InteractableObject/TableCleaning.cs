using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCleaning : MonoBehaviour
{
    public GameObject table; // 식탁 오브젝트
    public float cleanedArea = 0f; // 닦은 영역의 크기
    public float totalArea; // 식탁의 총 면적
    public float requiredCleanPercentage = 80f; // 필요한 청소 비율

    void Start()
    {
        // 식탁의 총 면적 계산 가로*세로
        totalArea = table.GetComponent<MeshRenderer>().bounds.size.x * table.GetComponent<MeshRenderer>().bounds.size.z;
    }

    void Update()
    {
        if (IsCleaning())
        {
            UpdateCleanedArea();
            // 청소가 충분히 되었는지 확인
            if (CheckIfCleaningDone())
            {
                Debug.Log("완료");
            }
        }

        
    }

    bool IsCleaning()
    {
        // 행주와 식탁이 접촉하는지
        // Physics.Raycast를 아래로 쏴 거리가 가까우면 true
        return true;
    }

    void UpdateCleanedArea()
    {
        // 실제 환경에서는 여기서 닦은 영역을 계산할 로직을 추가
        cleanedArea += Time.deltaTime; // 단순 예시
    }

    bool CheckIfCleaningDone()
    {
        return (cleanedArea / totalArea) * 100 >= requiredCleanPercentage;
    }
}

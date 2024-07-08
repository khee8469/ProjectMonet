using Jc;
using System.Collections;
using UnityEngine;

public class Flower : MonoBehaviour, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [Tooltip("부모의 PuzzleManager 할당")]
    [SerializeField]
    private PuzzleManager puzzleManager;

    [Tooltip("PuzzleManager의 클리어 조건 체크용")]
    [SerializeField]
    private int puzzleIndex;

    [Tooltip("꽃 충돌체 끄기용")]
    [SerializeField]
    BoxCollider flowerCollider;

    //Lerp 보간용
    float leapPer = 0;


    private void Awake()
    {
        if (puzzleManager == null)
            Debug.LogError("puzzleManager를 할당하시오");
        /*if (flowerCollider == null)
            Debug.LogError("Collider 할당하시오");*/
        //퍼즐매니저에 등록
        if (puzzleManager != null)
            RegistObject(puzzleManager);
    }

    private void OnEnable()
    {
        //스테이지3의 상태에 따라
        if (Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] == PuzzleState.Clear)
        {
            CompleteSetting();
        }
    }

    //파티클과 충돌시 실행
    private void OnParticleCollision(GameObject gameObject)
    {
        CompleteSetting();
    }

    IEnumerator FlowerLerp()
    {
        while (leapPer <= 1)
        {
            leapPer += Time.deltaTime * 0.2f;

            //크기 키우기
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), leapPer);

            yield return null;
        }
    }


    public void RegistObject(PuzzleManager puzzle)
    {
        if (puzzle == null)
            return;
        puzzle.puzzleObjects.Add(this);
    }

    public void ActiveSetting()
    {
        //순서인 퀘스트 오브젝트 활성화
    }

    public void DisActiveSetting()
    {
        //순서아닌 퀘스트 오브젝트 비활성화
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        puzzle.UpdateCondition(index); //아니면 정재훈잘못
    }

    
    public void CompleteSetting()
    {
        UpdatePuzzleManager(puzzleManager, puzzleIndex);
        //콜라이더 비활성화해서 여러번 감지하는거 방지
        if(flowerCollider!= null)
            flowerCollider.enabled = false;
        //꽃이 크는 코르틴
        StartCoroutine(FlowerLerp());
    }
}

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

    [Tooltip("분무기 파티클 충돌 확인용")]
    [SerializeField]
    private LayerMask waterTrigger;

    float leapPer = 0;

    bool isFull;

    [Tooltip("꽃 충돌체 끄기용")]
    [SerializeField]
    BoxCollider flowerCollider;

    //미션 클리어 체크
    private bool isSucess;
    public bool IsSucess { get { return isSucess; } }

    private void Awake()
    {
        //퍼즐매니저에 등록
        RegistObject(puzzleManager);
    }


    public void ActiveSetting()
    {

    }


    public void DisActiveSetting()
    {

    }


    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        puzzle.UpdateCondition(index); //아니면 정재훈잘못
    }

    public void RegistObject(PuzzleManager puzzle)
    {
        puzzle.puzzleObjects.Add(this);
    }

    public void CompleteSetting()
    {
        //throw new System.NotImplementedException();
    }

    private void OnParticleCollision(GameObject gameObject)
    {
        Destroy(flowerCollider);
        StartCoroutine(FlowerLerp());

        UpdatePuzzleManager(puzzleManager, puzzleIndex);
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
}

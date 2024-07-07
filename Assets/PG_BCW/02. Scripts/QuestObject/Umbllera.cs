using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Umbllera : InteractObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]

    [Tooltip("퍼즐 확인용")]
    [SerializeField]
    PuzzleManager puzzleManager;

    [Tooltip("PuzzleManager의 클리어 조건 체크용")]
    [SerializeField]
    public int puzzleIndex;

    [Tooltip("Miniature를 놓을 바닥으로 설정한 레이어")]
    [SerializeField]
    LayerMask raycastPoint;

    [Tooltip("비활성화할 우산")]
    [SerializeField]
    GameObject umbllera;
    [Tooltip("활성화할 여성")]
    [SerializeField]
    GameObject umblleraWoman;


    // 시작위치 저장용
    Vector3 startPosition;
    Quaternion startRotation;
    

    protected override void Awake()
    {
        base.Awake();
        if (puzzleManager == null)
            Debug.LogError("puzzleManager를 할당하시오");
        if (raycastPoint == 0)
            Debug.LogError("LayerMask를 할당하시오");

        if (puzzleManager!=null)
            RegistObject(puzzleManager);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //스테이지3의 상태에 따라
        if (!(Manager.PlableData.StageInfo[2] == -1))
        {
            CompleteSetting();
        }
        //퍼즐을 완료했으면
        if (Manager.PlableData.PuzzleSuccessCheck[puzzleManager.PuzzleIndex])
        {
            umbllera.SetActive(false);
            umblleraWoman.SetActive(true);
        }
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        //왜 잡자마자 실행되는거지
        base.OnSelectExiting(args);
        GroundCheck();
    }

    //미니어처 위치 지정
    public void GroundCheck()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 100, raycastPoint))
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
        /*// hit.point 위치에 AttachPoint 생성
        GameObject tempObject = new GameObject("TempObject");
        tempObject.transform.position = hit.point;
        hit.transform.GetComponent<SocketInteractor>().attachTransform = tempObject.transform;*/
    }

    //raycastPoint 바닥에 닿으면 퍼즐 성공
    private void OnTriggerEnter(Collider collider)
    {
        if (raycastPoint.Contain(collider.gameObject.layer))
        {
            //퍼즐 성공
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 10);
    }


    //AWAKE에서 실행
    public void RegistObject(PuzzleManager puzzle)
    {
        //처음 퀘스트매니저에 등록
        if (puzzle == null)
            return;
        puzzle.puzzleObjects.Add(this);
    }

    //조건들을 클리어할때마다 실행, 모든 조건들을 완료하면 퍼즐 성공
    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        //하나의 퀘스트에 여러가지 클리어 조건이 있는거에 사용
        puzzle.UpdateCondition(index);
    }

    public void ActiveSetting()
    {
        //순서인 퀘스트 오브젝트 활성화
    }

    public void DisActiveSetting()
    {
        //순서아닌 퀘스트 오브젝트 비활성화
    }

    public void CompleteSetting()
    {
        UpdatePuzzleManager(puzzleManager, puzzleIndex);
        umbllera.SetActive(false);
        umblleraWoman.SetActive(true);
    }
}

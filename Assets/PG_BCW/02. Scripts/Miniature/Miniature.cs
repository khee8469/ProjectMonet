using Jc;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : InteractObject, IPuzzleable
{
    [Tooltip("퍼즐 확인용")]
    [SerializeField]
    PuzzleManager puzzleManager;
    public int puzzleIndex;

    [Tooltip("저장용 Id")]
    [SerializeField] int id;
    public int Id { get { return id; } }


    // 시작위치 저장용
    Vector3 startPosition;
    Quaternion startRotation;
    // 바닥으로 설정한 레이어
    [SerializeField]
    LayerMask miniatureMapLayer;
    // 소켓에 넣을때 좌표 조정용
    [SerializeField]
    MiniatureManager miniatureManager;


    protected override void Awake()
    {
        base.Awake();
        if (puzzleManager != null)
            RegistObject(puzzleManager);

        miniatureManager = GetComponentInParent<MiniatureManager>();
    }

    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        GroundCheck();
        SavePosition();

        
    }

    //미니어처 위치 지정
    public void GroundCheck()
    {
        var positionData = Manager.PlableData.SavePosition;
        //부모보다 높은 위치에 배치해 레이케스트 쏴서 확인
        transform.localPosition = new Vector3(transform.localPosition.x, transform.parent.position.y + 10, transform.localPosition.z);
 
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, miniatureMapLayer))
        {
            // 히트 포인트를 로컬 좌표로 변환
            Vector3 localHitPoint = transform.parent.InverseTransformPoint(hit.point);
            transform.localPosition = localHitPoint;
            transform.rotation = startRotation;
        }
        else
        {
            //원위치
            Vector3 localHitPoint = transform.parent.InverseTransformPoint(startPosition);
            transform.localPosition = localHitPoint;
            transform.rotation = startRotation;
        }
    }

    //미니어처의 현재 위치를 저장
    public void SavePosition()
    {
        //key 값은 씬번호_오브젝트이름  같은 이름의 오브젝트에 위치 데이터 전달 예정
        if (transform.parent != null)
        {
            transform.parent = miniatureManager.transform;
            //부모크기에 따라 위치 보정 저장
            Manager.PlableData.MiniaturePositionSave(this, Manager.PlableData.SavePosition);
        }
        //소켓에 올려놓으면
        else
        {
            //부모를 다시 지정해줘야 로컬좌표를 얻을수있음
            transform.parent = miniatureManager.transform;

            //부모크기에 따라 위치 보정 저장
            Manager.PlableData.MiniaturePositionSave(this, Manager.PlableData.SavePosition);
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
        GetComponent<Collider>().enabled = false;
    }

    public void CompleteSetting()
    {
        //스테이지 별로 클리어한 퍼즐이면 한번 실행
        puzzleManager.OnClearPuzzle();
    }
}

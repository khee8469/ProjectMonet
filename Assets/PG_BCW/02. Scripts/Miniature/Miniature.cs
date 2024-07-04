using Jc;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : InteractObject
{
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
        var positionData = Manager.PlableData.PositionData.SavePosition_3;
        //부모보다 높은 위치에 배치해 레이케스트 쏴서 확인
        transform.localPosition = new Vector3(transform.localPosition.x, transform.parent.position.y + 10, transform.localPosition.z);
 
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, miniatureMapLayer))
        {
            // 히트 포인트를 로컬 좌표로 변환
            Vector3 localHitPoint = transform.parent.InverseTransformPoint(hit.point);
            transform.localPosition = localHitPoint;
            transform.rotation = startRotation;
            Debug.Log(transform.localPosition);
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
            Debug.Log(3);
            transform.parent = miniatureManager.transform;
            //부모크기에 따라 위치 보정 저장
            Manager.PlableData.MiniaturePositionSave(this, Manager.PlableData.PositionData.SavePosition_3);
        }
        //소켓에 올려놓으면
        else
        {
            Debug.Log(4);
            //부모를 다시 지정해줘야 로컬좌표를 얻을수있음
            transform.parent = miniatureManager.transform;

            //부모크기에 따라 위치 보정 저장
            Manager.PlableData.MiniaturePositionSave(this, Manager.PlableData.PositionData.SavePosition_3);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 10);
    }
}

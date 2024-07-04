using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : InteractObject
{
    [Tooltip("저장용 Id")]
    [SerializeField] int id;
    public int Id {  get { return id; } }  
    
    // 시작위치 저장용
    Vector3 startPosition;
    Quaternion startRotation;

    [SerializeField] LayerMask miniatureMapLayer;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        startPosition = transform.position;
        startRotation = transform.rotation;
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
        //부모보다 높은 위치에 배치해 레이케스트 쏴서 확인
        transform.position = new Vector3(transform.position.x, transform.parent.position.y + 1, transform.position.z);

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000f);

        //미니어처맵 밖에두면
        if(!miniatureMapLayer.Contain(hit.transform.gameObject.layer))
        {
            Debug.Log("타일 밖에 두었습니다.");
            //원위치
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
        else //미니어처맵 안에두면
        {
            Debug.Log("타일 안에 두었습니다.");
            Debug.Log(hit.transform.position.y);
            transform.position = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
        }
    }

    //미니어처의 현재 위치를 저장
    public void SavePosition()
    {
        //key 값은 씬번호_오브젝트이름  같은 이름의 오브젝트에 위치 데이터 전달 예정
        Manager.PlableData.PositionData.SavePosition_3[id] = transform.localPosition;
        //데이터 저장
        //Manager.PlableData.SaveMiniatureData();
    }
}

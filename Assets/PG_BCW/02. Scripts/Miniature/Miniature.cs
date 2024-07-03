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
    Vector3 startPos;

    [SerializeField] LayerMask miniatureMapLayer;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //몇번 씬정보인지
        //sceneNumber = (int)transform.parent.GetComponent<MiniatureManager>().MiniatureNum;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        startPos = transform.position;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        GroundCheck();
        SavePosition();
    }

    //미니어처의 현재 위치를 저장
    public void SavePosition()
    {
        //key 값은 씬번호_오브젝트이름  같은 이름의 오브젝트에 위치 데이터 전달 예정
        Manager.PlableData.PositionData.SavePosition_3[id] = transform.localPosition;
        //positionData[$"{sceneNumber}_{transform.name}"] = transform.localPosition;

        //데이터 저장
        Manager.PlableData.SaveMiniatureData();
    }

    //미니어처가 장판 밖에 두었을때 원래 위치로 복구
    public void GroundCheck()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000f);

        //타일밖에두면
        //if(hit.transform != transform.parent)
        if(!Extension.Contain(miniatureMapLayer, hit.transform.gameObject.layer))
        {
            Debug.Log("타일 밖에 두었습니다.");
            //원위치
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.Log("타일 안에 두었습니다.");
            //미니어처 놓았을 때 높이와 회전 고정
            transform.position = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
        }
    }
}

using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : InteractObject
{
    int sceneNumber;  // 씬 위치데이터 접근용
    Vector3 startPos;  // 시작위치 저장용
    
    [SerializeField] float yHeight;


    protected override void Awake()
    {
        base.Awake();

        //XRBaseControllerInteractor a;
    }

    private void Start()
    {
        //몇번 씬정보인지
        sceneNumber = (int)transform.parent.GetComponent<MiniatureManager>().MiniatureNum;
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
        var positionData = PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber];
        positionData[transform.name] = transform.localPosition;
    }

    //미니어처가 장판 밖에 두었을때 원래 위치로 복구
    public void GroundCheck()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000f);

        //타일밖에두면
        if(hit.transform != transform.parent)
        {
            //원위치
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            //미니어처 놓았을 때 높이와 회전 고정
            transform.position = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
        }
    }
}

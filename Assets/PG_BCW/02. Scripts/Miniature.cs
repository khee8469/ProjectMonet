using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : XRGrabInteractable
{
    int sceneNumber;  // 씬 위치데이터 접근용

    XRGrabInteractable grabInteractable;

    Vector3 startPos;

    protected override void Awake()
    {
        base.Awake();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(StartGrab);
        grabInteractable.selectExited.AddListener(EndGrab);
    }

    private void Start()
    {
        switch (transform.parent.GetComponent<MiniatureManager>().MiniatureNum)
        {
            case PositionSyncManager.MiniatureNum.First: sceneNumber = 0; break;
            case PositionSyncManager.MiniatureNum.Second: sceneNumber = 1; break;
            case PositionSyncManager.MiniatureNum.Third: sceneNumber = 2; break;
            case PositionSyncManager.MiniatureNum.Fourth: sceneNumber = 3; break;
        }
    }

    public void StartGrab(SelectEnterEventArgs args)
    {
        startPos = transform.position;
    }

    //XR Grab Interactable의 Select Exited 이벤트에서 사용중
    public void EndGrab(SelectExitEventArgs args)
    {
        GroundCheck();
        GetPosition();
    }

    //미니어처의 현재 위치를 저장
    public void GetPosition()
    {
        //위치데이터 저장
        //딕셔너리에 같은 key의 데이터가 없으면 저장
        if (!PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(transform.name))
        {
            PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(transform.name, transform.localPosition);
        }
        //딕셔너리에 같은 key의 데이터가 있으면 삭제 후 다시 저장
        else if (PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(transform.name))
        {
            PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Remove(transform.name);
            PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(transform.name, transform.localPosition);
        }

        //미니어처 놓았을 때 높이와 회전 고정
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        transform.rotation = Quaternion.identity;
    }

    //미니어처가 장판 밖에 두었을때 원래 위치로 복구
    public void GroundCheck()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000f);

        if(hit.transform != transform.parent)
        {
            transform.position = startPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Miniature : XRGrabInteractable
{
    int sceneNumber;  // 임시 확인용

    XRGrabInteractable grabInteractable;

    protected override void Awake()
    {
        base.Awake();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(EndGrab);

        if (transform.parent.name == "Scene_1 Miniature")
            sceneNumber = 0;
        else if (transform.parent.name == "Scene_2 Miniature")
            sceneNumber = 1;
        else if (transform.parent.name == "Scene_3 Miniature")
            sceneNumber = 2;
        else if (transform.parent.name == "Scene_4 Miniature")
            sceneNumber = 3;
        else
            Debug.Log("ERROR");

        Debug.Log(sceneNumber);
    }

    //XR Grab Interactable의 Select Exited 이벤트에서 사용중
    public void EndGrab(SelectExitEventArgs args)
    {
        GetPosition();
    }

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
}

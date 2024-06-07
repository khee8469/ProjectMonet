using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureMove : MonoBehaviour
{
    public void SetPosition()
    {
        //위치데이터 저장
        //딕셔너리에 같은 key의 데이터가 없으면
        if(PositionSyncManager.instance.positionData.SavePosition.ContainsKey(transform.name) == null){
            PositionSyncManager.instance.positionData.SavePosition.Add(transform.name, transform.position);
        }
        //딕셔너리에 같은 key의 데이터가 있으면
        else if (PositionSyncManager.instance.positionData.SavePosition.ContainsKey(transform.name) != null)
        {
            PositionSyncManager.instance.positionData.SavePosition.Remove(transform.name);
            PositionSyncManager.instance.positionData.SavePosition.Add(transform.name, transform.position);
        }
        
        //미니어처 놓았을 때 높이와 회전 고정
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        transform.rotation = Quaternion.identity;
    }
}

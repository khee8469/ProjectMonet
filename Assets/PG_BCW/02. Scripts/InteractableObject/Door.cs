using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : XRSocketInteractor
{
    [Header("현재 오브젝트")]

    [Tooltip("키네마틱 끄기용")]
    [SerializeField]
    Rigidbody rb;

    [Tooltip("열쇠 오브젝트 ID")]
    [SerializeField]
    int openKeyID;

    [Tooltip("활성화 할 오브젝트")]
    [SerializeField]
    ItemObject activeObject;

    [Tooltip("체크할 퍼즐 아이디")]
    [SerializeField]
    int questID = 1510010;

    protected override void OnEnable()
    {
        if (Manager.Quest.QuestDic[questID].State == QuestState.Complete || Manager.Quest.QuestDic[questID].State == QuestState.Clear)
        {
            //이미 클리어했다면 문을 열어두고, 물뿌리게는 못잡게
            if (rb != null) rb.isKinematic = false;
            if (activeObject != null) activeObject.enabled = false;
        }
    }



    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //맞는 아이템이 아니면 셀렉트 취소
        if(args.interactableObject.transform.GetComponent<ItemObject>().ItemID != openKeyID)
        {
            this.interactionManager.SelectExit(args.interactorObject, args.interactableObject);
        }
        else
        {
            //문열기
            if (rb != null) rb.isKinematic = false;
            //활성화 할 오브젝트
            if (activeObject != null) activeObject.enabled = true;
        }
    }
}

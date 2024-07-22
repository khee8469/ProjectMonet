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
    int openKeyID = 1410007;

    [Tooltip("체크할 퍼즐 아이디")]
    [SerializeField]
    int questID = 1510010;

    [Tooltip("비활성화 할 오브젝트")]
    [SerializeField]
    GameObject[] activeObject;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (Manager.Quest.QuestDic[questID].State == QuestState.Complete || Manager.Quest.QuestDic[questID].State == QuestState.Clear)
        {
            Debug.Log(123123);
            //이미 클리어했다면 문을 열고, 비활성화할 오브젝트 세팅
            if (rb != null) rb.isKinematic = false;
            if (activeObject.Length != 0)
            {
                for(int i = 0; i < activeObject.Length; i++)
                {
                    activeObject[i].SetActive(false);
                }
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        //맞는 아이템이 아니면 셀렉트 취소
        if (args.interactableObject.transform.GetComponent<ItemObject>().ItemID != openKeyID)
        {
            Debug.Log(123);
            this.interactionManager.SelectExit(args.interactorObject, args.interactableObject);
        }
        else
        {
            //문열기
            if (rb != null) rb.isKinematic = false;


            args.interactableObject.transform.GetComponent<Collider>().enabled = false;
        }
    }
}

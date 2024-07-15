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
            Debug.Log("열쇠넣기");
            if (rb != null) rb.isKinematic = false;
        }
    }
}

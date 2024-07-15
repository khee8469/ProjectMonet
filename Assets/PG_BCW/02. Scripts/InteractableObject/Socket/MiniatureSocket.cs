using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniatureSocket : XRSocketInteractor
{
    [Header("현재 오브젝트")]

    [Tooltip("소켓 지정 시 변경할 레이어")]
    [SerializeField]
    InteractionLayerMask handTrackingMask;

    [Tooltip("소켓 지정 아이템")]
    [SerializeField]
    int[] miniaturaItems;

    [Tooltip("Exit 포지션")]
    [SerializeField]
    Transform exitPosition;

    //키를 꽃았을때
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);


        //미니어처 퍼즐에 필요한 아이템만 세팅 가능
        ItemObject itemObject = args.interactableObject.transform.GetComponent<ItemObject>();
        for(int i = 0; i < miniaturaItems.Length; i++)
        {
            //지정된 아이템이 아니면 소켓 세팅 불가
            if (miniaturaItems[i] != itemObject.ItemID)
            {
                this.interactionManager.SelectExit(args.interactorObject, args.interactableObject);
                //팅겨나올 위치
                itemObject.transform.position = exitPosition.position;
            }
            //지정된 아이템이면 레이어변환해서 핸드트래킹으로만 조작가능하게
            else
            {
                args.interactableObject.transform.GetComponent<ItemObject>().interactionLayers = handTrackingMask;
                args.interactableObject.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            }
        }

        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        
    }
}

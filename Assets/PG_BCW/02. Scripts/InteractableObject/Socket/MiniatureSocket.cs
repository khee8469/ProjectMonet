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

    [Tooltip("소켓에 들어 올 때 크기")]
    [SerializeField]
    float socketItemScale = 0.3f;

    [Tooltip("소켓에 들어오는 시간")]
    [SerializeField]
    float trackingTime = 0.5f;

    [Tooltip("Exit 포지션")]
    [SerializeField]
    Transform exitPosition;



    //키를 꽃았을때
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);


        //미니어처 퍼즐에 필요한 아이템만 세팅 가능
        ItemObject itemObject = args.interactableObject.transform.GetComponent<ItemObject>();

        if (itemObject == null) return;

        for(int i = 0; i < miniaturaItems.Length; i++)
        {
            //지정된 아이템이 아니면 소켓 세팅 불가
            if (miniaturaItems[i] != itemObject.ItemID)
            {
                this.interactionManager.SelectExit(args.interactorObject, args.interactableObject);
                //팅겨나올 위치
                if(exitPosition != null)
                    itemObject.transform.position = exitPosition.position;
            }
            //지정된 아이템이면 레이어변환해서 핸드트래킹으로만 조작가능하게
            else
            {
                args.interactableObject.transform.GetComponent<ItemObject>().interactionLayers = handTrackingMask;

                coroutine = StartCoroutine(SuccessRoutine(args));
            }
        }

        
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }



    Coroutine coroutine;
    // 오브젝트 소켓 트래킹 루틴 
    IEnumerator SuccessRoutine(SelectEnterEventArgs args)
    {
        float rate = 0f;
        Vector3 startPos = args.interactableObject.transform.position;
        Quaternion startRot = args.interactableObject.transform.rotation;
        Vector3 startScale = args.interactableObject.transform.localScale;
        Vector3 endPos = transform.position;
        Quaternion endRot = transform.rotation;
        Vector3 endScale = new Vector3(socketItemScale, socketItemScale, socketItemScale);

        while (rate < 1f)
        {
            rate += Time.deltaTime / trackingTime;
            args.interactableObject.transform.position = Vector3.Lerp(startPos, endPos, rate);
            args.interactableObject.transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
            args.interactableObject.transform.localScale = Vector3.Lerp(startScale, endScale, rate);
            yield return null;
        }

        //args.interactableObject.transform.localPosition = Vector3.zero;
        //args.interactableObject.transform.localRotation = Quaternion.identity;


        StopCoroutine(coroutine);
    }


    /*IEnumerator ResetRoutine()
    {
        float rate = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 endPos = originPos;
        Quaternion endRot = originRot;

        boxCollider.enabled = false;
        while (rate < 1f)
        {
            rate += Time.deltaTime / trackingTime;
            transform.position = Vector3.Lerp(startPos, endPos, rate);
            transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
            yield return Time.deltaTime;
        }

        transform.position = originPos;
        transform.rotation = originRot;
        boxCollider.enabled = true;
    }*/
}

using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniatureItem : ItemObject
{
    [Header("미니어처 크기 조절용")]
    [SerializeField]
    Vector3 scale;


    /*protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);


            StartCoroutine(SuccessRoutine(args));
        

            // 실패
            //StartCoroutine(ResetRoutine());
        
    }

    // 오브젝트 소켓 트래킹 루틴 
    IEnumerator SuccessRoutine(SelectEnterEventArgs args)
    {
        float rate = 0f;
        *//*Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;*//*
        Vector3 endPos = args.interactorObject
        Quaternion endRot = socketTransfrom.rotation;

        while (rate < 1f)
        {
            rate += Time.deltaTime / trackingTime;
            transform.position = Vector3.Lerp(startPos, endPos, rate);
            transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        // 퍼즐매니저 업데이트
        UpdatePuzzleManager(puzzle, puzzleIndex);
    }
    IEnumerator ResetRoutine()
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

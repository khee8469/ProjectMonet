using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerInterAction : XRGrabInteractable
{
    // 일단 인터 액터블 상속해서 트리거 받는지 확인하기.

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("온 셀렉티드 엔터드 진입");
    }


}

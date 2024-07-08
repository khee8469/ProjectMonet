using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniatureMode : InteractObject
{
    [Tooltip("플레이어 지정 위치")]
    [SerializeField]
    Transform attachPoint;
    [Tooltip("내 카메라 위치")]
    [SerializeField]
    Transform mine;

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        Debug.Log(1);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        Vector3 setPosition = attachPoint.position;
        mine.position = setPosition;
    }
}

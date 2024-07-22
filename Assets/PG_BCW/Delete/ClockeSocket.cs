using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClockeSocket : XRSocketInteractor
{
    [Header("현재 오브젝트")]

    [Tooltip("시침")]
    [SerializeField]
    Transform clockHour;
    [Tooltip("분침")]
    [SerializeField]
    Transform clockMinute;
    [Tooltip("초침")]
    [SerializeField]
    Transform clockSecond;




    //키를 꽃았을때
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //분침 활성화
        clockMinute.gameObject.SetActive(true);
        //분침 아이템 비활성화
        args.interactableObject.transform.gameObject.SetActive(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

    }
}

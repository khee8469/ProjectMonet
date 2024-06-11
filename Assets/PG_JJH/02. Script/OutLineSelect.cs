using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Outlinable))]
public class OutLineSelect : XRGrabInteractable
{
    [SerializeField] Outlinable outlineableToUse;


    private void Start()
    {
        outlineableToUse = GetComponent<Outlinable>();
        outlineableToUse.enabled = false;
    }


    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        outlineableToUse.enabled = true;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if(isSelected==false)  //xrBase의 자신이 selected중인지 확인 가능한 bool 리턴 프로퍼티 
        {
            outlineableToUse.enabled = false; 
            Debug.Log("isSelected false.");
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("on selected entered");
        // 추후에 들었을 때 또 아웃라인을 바꾸고 싶다면 이 부분에서 아웃라인 관련 컴포넌트 수정해주기. 
    }


    // 이 부분은 나중에 수정하면 됩니다. --> 아웃라인을 언제 끌지 결정하는 일. 
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        outlineableToUse.enabled = false;

    }

    




}

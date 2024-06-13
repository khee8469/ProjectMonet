using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;


//XR Ray Interactor 대신 사용
public class RaycastController : XRRayInteractor
{
    [Header("추가사항 : 레이저 On/Off")]

    [Tooltip("레이저 비쥬얼")]
    [SerializeField] XRInteractorLineVisual xRInteractorLineVisual;
    [Tooltip("레이 그랩 가능 레이어")]
    [SerializeField] LayerMask layerMask;
    [Tooltip("그랩 가능 거리")]
    [SerializeField] float distance;
    [Tooltip("호버 할 때 매터리얼 변경")]
    ColorMaterialPropertyAffordanceReceiver colorReceiver;

    XRRayInteractor abc;

    //호버한 오브젝트 위치
    Vector3 interactable;


    protected override void Awake()
    {
        base.Awake();
        xRInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
        
    }

    protected override void Start()
    {
        base.Start();
        //xRInteractorLineVisual.enabled = false;
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        LineVisualOn();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        LineVisualOff();
    }

    //레이저 시각화 키기
    private void LineVisualOn()
    {
        xRInteractorLineVisual.enabled = true;
    }
    private void LineVisualOff()
    {
        xRInteractorLineVisual.enabled = false;
        
    }
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        float distance = (interactable.transform.position - transform.position).sqrMagnitude;
        if (distance > Mathf.Pow(this.distance, 2f))
        {
            return false;
        }
        return base.CanHover(interactable);
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        float distance = (interactable.transform.position - transform.position).sqrMagnitude;
        if (distance > Mathf.Pow(this.distance, 2f))
        {
            return false;
        }

        return base.CanSelect(interactable);
    }
}

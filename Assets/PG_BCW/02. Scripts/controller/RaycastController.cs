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
        xRInteractorLineVisual.enabled = false;
        maxRaycastDistance = 1f;
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

    /*protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        SelectActiveCheck(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        //SelectActiveOff(args);
    }

    private void SelectActiveCheck(HoverEnterEventArgs args)
    {
        //colorReceiver = args.interactable.GetComponent<XRInteractableAffordanceStateProvider>();
        interactable = args.interactableObject.transform.position;

        //오브젝트와 컨트롤러의 거리를 비교해서 select 가능 여부 확인
        if((interactable - transform.position).sqrMagnitude < distance * distance)
        {
            //colorReceiver
            allowSelect = true;
            distanceCheck = StartCoroutine(DistanceCheck( args));
        }
        else
        {
            allowSelect = false;
            distanceCheck = StartCoroutine(DistanceCheck( args));
        }
    }

    private void SelectActiveOff(HoverExitEventArgs args)
    {
        //allowSelect = false;
        
        if (distanceCheck == null)
            return;
        StopCoroutine(distanceCheck);
    }

    Coroutine distanceCheck;
    private IEnumerator DistanceCheck(HoverEnterEventArgs args)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            if ((interactable - transform.position).sqrMagnitude < distance * distance)
            {
                allowSelect = true;
            }
            else
            {
                Debug.Log(13123213);
                //args.interactable.GetComponent<IXRHoverInteractable>().isHovered = false;
                allowSelect = false;
            }
        }
    }*/
}

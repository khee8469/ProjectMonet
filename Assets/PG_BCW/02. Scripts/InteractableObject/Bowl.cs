using Jc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Bowl : InteractObject
{
    //손과 각거리를 비교해 정하기
    [SerializeField] Transform[] attachPoints;
    

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        AttachSerach(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        
    }


    //왼손 기준 (오른손으로 잡으면 회전시켜줘야할듯)
    private void AttachSerach(SelectEnterEventArgs args)
    {
        Transform interactor = args.interactorObject.transform;
        
        float distance=9999;
        foreach (Transform attach in attachPoints)
        {
            /*Debug.Log($"interactor : {interactor.position}");
            Debug.Log($"attach : {attach.position}");*/

            float currentDistance = (interactor.position - attach.position).sqrMagnitude;
            Debug.Log($"Attach : {attach.name} , Current Distance: {currentDistance}");
            Debug.Log(currentDistance < distance);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                attachTransform = attach;
                Debug.Log(attach.name);
            }
        }
        Debug.Log(attachTransform.name); 
    }
}

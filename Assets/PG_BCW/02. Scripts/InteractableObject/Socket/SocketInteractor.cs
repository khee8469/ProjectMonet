using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniatureSocket : XRSocketInteractor
{
    [Tooltip("")]
    [SerializeField]
    InteractionLayerMask handTrackingMask;


    //키를 꽃았을때
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        Debug.Log("소켓에 넣음");
        args.interactableObject.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        args.interactableObject.transform.GetComponent<ItemObject>().interactionLayers = handTrackingMask;


    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);


    }
}

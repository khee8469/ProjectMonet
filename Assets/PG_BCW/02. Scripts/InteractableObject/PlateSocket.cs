using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateSocket : XRSocketInteractor
{
    [SerializeField]
    Plate plate;

    [Tooltip("오브젝트를 못잡게하는 레이어 설정")]
    [SerializeField]
    InteractionLayerMask grabOff;

    protected override void Awake()
    {
        base.Awake();
        plate = GetComponentInParent<Plate>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        plate.SelectSocket();
        //포도 못잡게하기
        args.interactableObject.transform.GetComponent<Grape>().interactionLayers = grabOff;
    }
}

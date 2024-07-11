using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : XRSocketInteractor
{
    [Header("키네마틱 끄기용")]
    [SerializeField]
    Rigidbody rb;


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("열쇠넣기");
        if(rb != null ) rb.isKinematic = false;

    }
}

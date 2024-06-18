using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.OpenXR;
using UnityEngine.XR.Interaction.Toolkit;

public class PaintBucket : InteractObject
{
    
    LayerMask handLayer;
    [SerializeField] GameObject prefab;


    XRHandJointsUpdatedEventArgs handPosition;

    private void OnTriggerStay(Collider collider)
    {
        if((0<<collider.gameObject.layer & handLayer) != 0)
        {
            XRHand a;
        }

        
    }


    
}

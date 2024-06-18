using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PinchGesture : Gesture
{
    PaintBucket paintBucket;

    public override void GestureEnter(XRBaseInteractor interactor)
    {
        Debug.Log("핀치 제스처");

        paintBucket = interactor.interactablesHovered.FirstOrDefault() as PaintBucket;

        if( paintBucket != null )
        {
            paintBucket.PaintPlay();
        }
    }

    public override void GestureExit(XRBaseInteractor interactor)
    {
        if (paintBucket != null)
        {
            paintBucket.PaintStop();
        }

        paintBucket = null;
    }
}

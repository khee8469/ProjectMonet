using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HumanSocket : XRSocketInteractor
{
    //모자가 씌어져있는가
    private bool onHat;
    public bool OnHat { get {  return onHat; } }

    //벚긴적이 있는가
    private bool steel;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        onHat = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        steel = true;
        onHat = false;
    }
}

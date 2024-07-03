using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class testChangeController : XRInputModalityManager
{
    XRHandSubsystem m_HandSubsystem;
    private void Start()
    {
        m_HandSubsystem = GetComponent<XRHandSubsystem>();
    }


}

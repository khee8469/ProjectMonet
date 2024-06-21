using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Swing : InteractObject
{

    [SerializeField] int swingCount;

    [SerializeField] 
    

    protected override void Awake()
    {
        base.Awake();
        
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);


    }

    // 상속하는 자식에서 다양화
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        swingCount++;
    }
}

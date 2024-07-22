using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;

public class TempItemObject : ItemObject
{
    
    void Start()
    {
        // 0 이 Nothing 1 -1 이 Everything
        // 0 은 모든 레이어비트 제외 -1은 모든 레이어 비트 포함 
        this.interactionLayers = 0;
        
    }

}

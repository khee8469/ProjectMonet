using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public enum  ButtonType
{
    None = -1,
    OnlyPress,
    PressAndRelease
    
}



public class CustomButton : XRPushButton
{
    // 버튼의 종류에 따라 
    // 1. 누르기만 하면 작동
    // 2. 누르고 떼야 작동
    // 3. 누르고 있어야 쟉동 ? 

    [Space(5)]
    [Header("------ 커스텀 컴포넌트 ------")]
    [Space(5)]
    [Header("버튼의 타입")]
    [SerializeField]
    protected ButtonType buttonType;

    public ButtonType ButtonType { get { return buttonType; } }


    

}



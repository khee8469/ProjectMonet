using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
// 상호작용할 오브젝트의 타입
public enum ObjectType
{
    None = -1,          // 단순 상호작용 오브젝트 
    CapturedScreen,      // 캡쳐된 스크린
    Button
}

namespace Jc
{
    public class InteractObject : XRGrabInteractable
    {
        [Space(5)]
        [Header("---- 컴포넌트 커스텀 ----")]
        [Space(5)]
        [Header("오브젝트의 타입 (상호작용)")]
        [SerializeField]
        protected ObjectType objectType;
        public ObjectType ObjectType {get { return objectType; } }

        protected bool isGrabbed = false;   // 오브젝트의 그랩 여부

        // 상속하는 자식에서 다양화
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
        }

        // 상속하는 자식에서 다양화
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
        }

        

    }
}

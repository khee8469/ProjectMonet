using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public interface IActivatable
{
    public void Activate();

    // CAN !! COMPONENT ?? 캐릭터 컨트롤러 기능을 붙이는 느낌? --> 할 수 있는.. 


    // STATIC TRIGGER 오브젝트의 COLLIDER를 원상 복구 시키기 위한 작업. 
    public void ResetColliderPosition();
   

  
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.Events;

namespace JJH
{
    public class DrawingCompleteManager : Singleton<DrawingCompleteManager>
    {
        // 이 Don`t Destroy를 통해서 채색 관리를 씬 간 연계 해주자. 

        // 어차피 각 씬은 baseScene을 상속 중이니까 로딩 루틴 중에 변경시키던가... 하고싶은데
        // 변경을 시키던지 아니면... 흑백으로 만들어두고 변수에 따라 켜주던지... 

        // 각 씬에서 또 다시 마테리얼을 변경하는 작업을 하지 않을 수 있도록 static + 싱글턴으로 관리할 bool 변수
        public static bool isColor_First_Completed;
        public static bool isColor_Second_Completed;
        public static bool isColor_Third_Completed;
        public static bool isColor_Fourth_Completed;

        // 그리고 로딩 루틴에서 루틴 돌릴 때 bool 변수와 스크립트들의 타입에 맞춰서
        // 흑백으로 사용할 애들만 흑백으로 전환시키기 (어차피 기본 상태는 원래 마테리얼 이기 때문 ) 

        // 아직 흑백인 상황일 때만 흑백 전이 함수를 사용하고 아닐때는 그냥 return 때려서? 해주기?


        


        

    }
}


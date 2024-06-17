using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Scene : BaseScene
{
    public override IEnumerator LoadingRoutine()
    {
        Camera mainCamera = Camera.main; // player에게 붙어있는 메인카메라 

        if (ChapterManager.is_Colored[1]==true) // true 라면 흑백효과 풀기. 
        {

        }
        else // true가 되지 않은 상태라면 흑백효과 그대로 적용 
        {

        }

        yield return null; 
    }
}

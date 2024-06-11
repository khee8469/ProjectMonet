using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Scene : BaseScene
{
    public override IEnumerator LoadingRoutine()
    {
        Debug.Log("챕터1 로딩 루틴");
        yield return null; 
    }
}

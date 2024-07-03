using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    // 진행 중인 코루틴이 있으면 진행 중인 코루틴을 중지 시키고 다른 코루틴을 진행 시킬 매니저
    // button 같은 경우는 누르다가 떼면 바로바로 다른 코루틴이 진행되어야 하기 때문이다.

    private Coroutine activeCoroutine;

    public void StartAndStopCoroutine(IEnumerator coroutine)
    {
        if(activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine =StartCoroutine(coroutine);

    }



}

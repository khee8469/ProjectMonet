using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOnOff : MonoBehaviour
{
    // 일단 로딩 루틴이 도는 동안은 이거를 계속 켜 둬야
    // start 에서 slot의 넘버가 할당이 가능하다는것. 
    // 씬을 넘어 갔을 때는 꺼져 있으면 슬롯 넘버가 할당이 안된다. 

    // 스타트에서 그냥 꺼버려도 괜찮나?
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.SetActive(false); // 그냥 끄니까 다시 킬 때 뭔가 버벅이는게 있으니까 그 부분 수정하고. 
        Debug.Log("스타트 루틴에서 캔버스 꺼버림.");
    }


}


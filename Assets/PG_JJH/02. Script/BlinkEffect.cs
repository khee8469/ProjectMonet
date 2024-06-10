using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    // 일정시간이 지나면 오브젝트가 반짝거림 (힌트)

    [SerializeField] private bool isBlink;
    [SerializeField] int itemID; // 아이디로 관리해서 이벤트 발동시켜주기. 


    private void Start()
    {
        isBlink = false;
    }

    private void Update()
    {
        
    }


    IEnumerator BlinkRoutine()
    {



        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    
}

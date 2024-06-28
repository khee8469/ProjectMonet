using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempItemAdd : MonoBehaviour
{
    int ItemID = 1;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Manager.Inventory.AddItem(ItemID); // 1번 아이템 ADD 
            Debug.Log("1번 아이템 추가 한다. ");
        }
    }
}

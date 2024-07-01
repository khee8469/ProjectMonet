using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUpdate : MonoBehaviour
{
    private void Update()
    {
        Debug.Log($"is enable 상태 체크 ->{Manager.Inventory.isEnable}");
    }
}

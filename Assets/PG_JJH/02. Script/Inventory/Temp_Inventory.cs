using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Inventory : MonoBehaviour
{

    public GameObject inventory;
    public GameObject anchor;
    public bool uiactive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            uiactive = !uiactive;
            inventory.SetActive(uiactive);
        }
        if (uiactive)
        {
            inventory.transform.position = anchor.transform.position;
            inventory.transform.eulerAngles = new Vector3
                (anchor.transform.eulerAngles.x * 15, anchor.transform.eulerAngles.y, 0);
        }
    }
}

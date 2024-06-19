using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallet : InteractObject
{
    [SerializeField]
    Paint paintPrefab;

    [SerializeField] LayerMask layerMask;

    bool isOk;


    private void OnTriggerEnter(Collider collider)
    {
        if (!layerMask.Contain(collider.gameObject.layer))
            return;

        if (!isOk)
        {
            Paint paint = Instantiate(paintPrefab, collider.transform.position, Quaternion.Euler(0, collider.transform.position.y, 0));
        }
        isOk=true;
    }


}

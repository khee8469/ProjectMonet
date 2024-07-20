using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class GearTrigger : MonoBehaviour
    {
        [SerializeField] private Collider col;

        [Tooltip("기어의 Layer-> ex ) 리턴 아이템 ")] 
        [SerializeField] private LayerMask layerMask; 

        [SerializeField] private Transform gearReturnPosition;
        private void OnTriggerExit(Collider other)
        {
            if(Extension.Contain(layerMask, other.gameObject.layer))
            {
                Debug.Log("기어 밖으로 나감 원위치 복귀");
                other.gameObject.transform.position = gearReturnPosition.transform.position;                       
            }
        }
    }
}


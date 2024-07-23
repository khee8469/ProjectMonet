using System.Collections;
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
            if (Extension.Contain(layerMask, other.gameObject.layer))
            {
                Debug.Log("기어 밖으로 나감 원위치 복귀");

                GearObject gear = other.GetComponent<GearObject>();
                if (gear != null)
                {
                    //StartCoroutine(ReturnRoutine(gear));

                    // 코루틴으로 돌리면 소켓에 들어갈 때도 out 당함 (trigger exit 이 소켓에 들어가는 순간에도 발동됨... )
                    gear.interactionLayers = 0;
                    gear.transform.position = gearReturnPosition.position;
                    gear.interactionLayers = -1; // EveryThing

                }
            }
        }


        private IEnumerator ReturnRoutine(GearObject gear)
        {
            gear.interactionLayers = 0; // Nothing 
            yield return null;
            gear.transform.position = gearReturnPosition.position;
            yield return null;
            gear.interactionLayers = -1; // EveryThing
            yield return null;

        }


    }
}


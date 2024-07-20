using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
                if(gear!=null)
                {
                    // 0 -> nothing 1 -> everything 
                    gear.interactionLayers = 0;
                    gear.transform.position = gearReturnPosition.position;
                    gear.interactionLayers = 1;
                }
            }
        }
    }
}


using JJH;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class TempLay : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public LayerMask socketLayerMask;

    void Update()
    {
        CheckRaycast();
    }

    private void CheckRaycast()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (Extension.Contain(socketLayerMask, hit.collider.gameObject.layer))
            {
                Debug.Log("레이 채크 내부가 문제네");
                var socket = hit.collider.GetComponent<InventorySlot>();
                Debug.Log("ddd");
                if (socket != null)
                {
                    Debug.Log("Raycast hit socket: " + socket.transform.name);
                    // 추가적인 처리

                }
            }
        }

        // Draw the ray in the Scene view
        Vector3 rayOrigin = rayInteractor.transform.position;
        Vector3 rayDirection = hit.point - rayOrigin;
        Debug.DrawRay(rayOrigin, rayDirection, Color.red);

    }




}


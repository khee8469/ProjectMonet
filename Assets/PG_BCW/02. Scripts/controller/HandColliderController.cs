using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliderController : MonoBehaviour
{
    [Tooltip("한손의 콜라이더 리스트")]
    [SerializeField] Collider[] handColliders;

    private void Awake()
    {
        handColliders = GetComponentsInChildren<Collider>();
    }

    //콜라이더 키기
    public void OnColliders()
    {
        foreach(Collider handCollider in handColliders)
        {
            if(handCollider != null)
            {
                handCollider.enabled = true;
            }
        }
    }

    //콜라이더 끄기
    public void OffColliders()
    {
        foreach (Collider handCollider in handColliders)
        {
            if (handCollider != null)
            {
                handCollider.enabled = false;
            }
        }
    }
}

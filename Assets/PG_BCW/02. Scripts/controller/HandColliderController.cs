using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliderController : MonoBehaviour
{
    //제스처를 취하고 안할때 콜라이더를 끄고 키기위해 제작
    [Tooltip("한손의 콜라이더 리스트")]
    [SerializeField] Collider[] handColliders;

    private void Awake()
    {
        handColliders = GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        //OffColliders();
    }

    //콜라이더 키기
    public void OnColliders()
    {
        for(int i = 0; i < handColliders.Length; i++)
        {
            handColliders[i].enabled = true;
        }
    }

    //콜라이더 끄기
    public void OffColliders()
    {
        for (int i = 0; i < handColliders.Length; i++)
        {
            handColliders[i].enabled = false;
        }
    }
}

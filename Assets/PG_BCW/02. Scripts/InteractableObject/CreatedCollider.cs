using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedCollider : MonoBehaviour
{
    //트리거시 체크하기 위해 참조
    public CreateAreaCollider createAreaCollider;
    //충돌 체크용
    [SerializeField] private LayerMask triggerLayer;
    public LayerMask TriggerLayer { get { return triggerLayer; } set { triggerLayer = value; } }
    
    //한번만 확인용
    [SerializeField] private bool IsCheak = false;


    void OnTriggerEnter(Collider collider)
    {
        if (!IsCheak && triggerLayer.Contain(collider.gameObject.layer))
        {
            //Debug.Log("청소중");
            createAreaCollider.CleaningCheck[gameObject] = true;
            createAreaCollider.CheckCleaningProgress();

            IsCheak = true;
        }
    }
}

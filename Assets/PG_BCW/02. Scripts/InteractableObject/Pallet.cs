using Jc;
using JJH;
using UnityEngine;

public class Pallet : InteractObject
{
    [Tooltip("물감 프리펩")]
    [SerializeField]
    private Paint paint;
    [SerializeField] 
    private LayerMask layer;

    [SerializeField]
    private Transform attach;

    
    private void OnCollisionEnter(Collision collision)
    {
        if (layer.Contain(collision.gameObject.layer))
        {
            //충돌체의 색타입으로 변경하여 생성
            paint.paintType = collision.gameObject.GetComponent<Paint>().paintType;
            //떨어진 위치
            Vector3 position = collision.contacts[0].point;
            //충돌체 삭제
            Destroy(collision.gameObject);
            //프리팹 생성 후 위치 고정
            Paint prefab = Instantiate(paint, position, transform.rotation, transform);
            prefab.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }





}

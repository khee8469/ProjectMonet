using Jc;
using JJH;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pallet : InteractObject
{
    [Tooltip("물감 프리펩")]
    [SerializeField]
    private Paint paint;
    [SerializeField] 
    private LayerMask layer;

    [SerializeField]
    private Transform attach;

    [SerializeField]
    [Tooltip("떨어진 오브젝트와 색깔을 판단해 줄 자신의 paint들")]
    private Paint [] paintPos;

    public Vector3 startPosition;
    public Quaternion startRotation;



    private void Start()
    {
        for(int i=0;i<paintPos.Length;i++)
        {
            paintPos[i].gameObject.SetActive(false); // 일단 씬 시작 하면 False로 꺼두기. 
        }

        startPosition = transform.position;
        startRotation = transform.rotation;


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (layer.Contain(collision.gameObject.layer))
        {
            //충돌체의 색타입으로 변경하여 생성
            //paint.paintType = collision.gameObject.GetComponent<Paint>().paintType;
            //떨어진 위치

            for(int i=0; i<paintPos.Length; i++)
            {
                if (paintPos[i].paintType == collision.gameObject.GetComponent<Paint>().paintType)
                {
                    paintPos[i].gameObject.SetActive(true);
                    break;
                }
            }

            //Vector3 position = collision.contacts[0].point;
            //충돌체 삭제
            Destroy(collision.gameObject);
            //프리팹 생성 후 위치 고정
            //Paint prefab = Instantiate(paint, position, transform.rotation, transform);
            //prefab.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        transform.position = startPosition;
        transform.rotation = startRotation;

    }




}

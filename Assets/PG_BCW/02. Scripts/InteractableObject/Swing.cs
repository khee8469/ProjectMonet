using Jc;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Swing : ItemObject
{
    //범위에 닿앗는지 확인용
    [SerializeField] LayerMask trigger;
    //일정 범위까지 당겼는지
    bool OnTrigger;
    //밀어준 횟수
    [SerializeField] int swingCount;
    //움직임 제한용
    private Rigidbody rb;

    private float rate;

    //미션 클리어
    private bool isSucess;
    public bool IsSucess { get { return isSucess; } }


    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }


    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        //두손으로 잡아야 당기기 가능
        if (args.interactableObject.interactorsSelecting.Count < 2)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;

        }
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //나머지 손도 놓기
        if (args.interactableObject.interactorsSelecting.Count != 0)
        {
            this.interactionManager.SelectExit(args.interactableObject.interactorsSelecting[0], args.interactableObject);
            //두손으로 잡고 일정 거리까지 당긴 후 놔야 카운트 증가
            if (OnTrigger)
            {
                swingCount++;
                if (swingCount > 2)
                {
                    Debug.Log("그네 3회 성공");
                    isSucess = true;
                }

            }


        }
        rb.constraints = RigidbodyConstraints.None;


    }

    //일정영역까지 당겨지는 확인용
    private void OnTriggerEnter(Collider collider)
    {
        if (trigger.Contain(collider.gameObject.layer))
        {
            OnTrigger = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (trigger.Contain(collider.gameObject.layer))
        {
            OnTrigger = false;
        }
    }
}

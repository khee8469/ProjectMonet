using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WateringCan : InteractObject
{
    [Header("현재 오브젝트 정보")]
    [Tooltip("지정된 소켓 위치")]
    [SerializeField]
    Transform specifiedSocket;

    /*[Tooltip("소켓위가아니면")]
    [SerializeField]
    LayerMask raycastPoint;*/


    protected override void OnEnable()
    {
        base.OnEnable();

        /*//아이템은 먹은적이없으면
        if (Manager.PlayableData.itemDic[itemId] == false)
        {
            gameObject.SetActive(false);
        }*/
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //책상위에 놓으면 지정된 소켓위로
        OriginalPosition();
    }

    //미니어처 위치 지정
    public void OriginalPosition()
    {
        /*RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 100, raycastPoint))*/

        if (specifiedSocket != null)
        {
            transform.position = specifiedSocket.position;
            transform.rotation = specifiedSocket.rotation;
        }
    }
}

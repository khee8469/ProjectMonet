using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WateringCan : InteractObject
{
    [Header("현재 오브젝트 정보")]
    [Tooltip("지정된 소켓 위치")]
    [SerializeField]
    Transform specifiedSocket;

    [Tooltip("")]
    [SerializeField]
    LayerMask raycastPoint;

    protected override void Awake()
    {
        base.Awake();

        if (specifiedSocket == null)
            Debug.LogError("소켓 위치를 할당 하시오");
    }




    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //책상위에 놓으면 지정된 소켓위로
        GroundCheck();
    }

    //미니어처 위치 지정
    public void GroundCheck()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 100, raycastPoint))
        {
            transform.position = specifiedSocket.position;
            transform.rotation = specifiedSocket.rotation;
        }
    }
}

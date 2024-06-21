using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectInteractor : XRDirectInteractor
{
    [Header("커스텀 세팅")]
    [Tooltip("컨트롤러 입력 콜백 함수")]
    [SerializeField]
    private PlayerControllerCallback controllerCallback;

    [Tooltip("왼손 오른손 확인")]
    /*[SerializeField]
    private bool isLeftController = false;*/
    public enum Hand {  Left, Right };
    [SerializeField]
    private Hand interactorHand;
    public Hand InteractorHand { get { return interactorHand; } }


    // 양손다 잡혓는지 확인필요 양손다 잡혓을떄 그네 이동가능?


    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();

    }


    protected override void OnEnable()
    {
        base.OnEnable();
        //왼손 입력 콜백 함수 추가
        if (interactorHand == Hand.Left)
        {

        }
        //왼손 입력 콜백 함수 추가
        else
        {

        }
    }
    protected override void OnDisable()
    {
        //왼손 입력 콜백 함수 삭제
        if (interactorHand == Hand.Left)
        {
  
        }
        //왼손 입력 콜백 함수 삭제
        else
        {

        }
        base.OnDisable();
    }

    private void Update()
    {

    }

    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        InteractObject itrObject = interactable as InteractObject;
        if (itrObject == null)
            return false;

        return base.CanHover(interactable);
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        InteractObject itrObject = interactable as InteractObject;
        if (itrObject == null)
            return false;

        return base.CanSelect(interactable);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);


    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);


    }

    
}

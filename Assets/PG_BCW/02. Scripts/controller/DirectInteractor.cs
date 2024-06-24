using Jc;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Pooling;

public class DirectInteractor : XRDirectInteractor
{
    [Header("커스텀 세팅")]
    [Tooltip("컨트롤러 입력 콜백 함수")]
    [SerializeField]
    private PlayerControllerCallback controllerCallback;

    [Tooltip("왼손 오른손 확인")]
    [SerializeField]
    private bool isLeftController = false;
    public enum Hand {  Left, Right };
    [SerializeField]
    private Hand interactorHand;
    public Hand InteractorHand { get { return interactorHand; } }

    //구현하고싶은 기능
    //1.양손다 잡혓는지 확인필요 양손다 잡혓을떄 그네 이동가능?
    //2.특정 오브젝트를 잡을떄 Attach포즈 저장
    //Dictionary<IXRSelectInteractable, Pose> m_AttachPoseOnSelect
    //Dictionary<IXRSelectInteractable, Pose> m_LocalAttachPoseOnSelect



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

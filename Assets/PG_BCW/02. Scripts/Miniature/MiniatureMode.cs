using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MiniatureMode : InteractObject
{
    [Header("미니어처 모드 정보")]

    [Tooltip("미니어처모드 On 위치")]
    [SerializeField]
    Transform modeOnPosition;
    [Tooltip("미니어처모드 Off 위치")]
    [SerializeField]
    Transform modeOffPosition;

    [Tooltip("TestPlayer 위치")]
    [SerializeField]
    Transform testPlayer;
    [Tooltip("플레이어 CharacterController 끄기용")]
    [SerializeField]
    CharacterController charactorController;
    [Tooltip("MiniatureMode Collider 끄기용")]
    [SerializeField]
    Collider miniatureModeCollider;
    [Tooltip("플레이어 움직임 막기용")]
    [SerializeField]
    DynamicMoveProvider dynamicMoveProvider;

    [Tooltip("플레이어 왼쪽 컨트롤러")]
    [SerializeField]
    ActionBasedController leftController;
    [Tooltip("플레이어 오른쪽 컨트롤러")]
    [SerializeField]
    ActionBasedController rightController;

    bool miniatureMode;

    /*[Tooltip("컨트롤러와 핸드트래킹 변경용")]
    [SerializeField]
    XRInputModalityManager modalityManager;*/




    protected override void Awake()
    {
        base.Awake();

        if (modeOnPosition == null)
            Debug.LogError("modeOnPosition을 할당하시오");
        if (modeOffPosition == null)
            Debug.LogError("modeOffPosition을 할당하시오");
        if (testPlayer == null)
            Debug.LogError("mine을 할당하시오");
        if (charactorController == null)
            Debug.LogError("charactorController을 할당하시오");
        if (miniatureModeCollider == null)
            Debug.LogError("miniatureModeCollider을 할당하시오");
        if (dynamicMoveProvider == null)
            Debug.LogError("dynamicMoveProvider을 할당하시오");
        if(leftController == null)
            Debug.LogError("leftController을 할당하시오");
        if (rightController == null)
            Debug.LogError("rightController 할당하시오");



        // 입력 이벤트 등록
        leftController.selectAction.action.performed += OnAnyButtonPressed;
        

        rightController.selectAction.action.performed += OnAnyButtonPressed;
        

    }


    protected override void OnEnable()
    {
        base.OnEnable();

        // 입력 액션 활성화
        leftController.selectAction.action.Enable();

        rightController.selectAction.action.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 입력 액션 비활성화
        leftController.selectAction.action.Disable();

        rightController.selectAction.action.Disable();
    }

    private void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        if (miniatureMode)
        {
            ExitMiniatureMode();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if(!miniatureMode)
        {
            //Debug.Log("미니어처모드 시작");
            //못움직이게
            dynamicMoveProvider.moveSpeed = 0;

            //서로 충돌하지않게 콜라이더 끄기
            miniatureModeCollider.enabled = false;
            charactorController.enabled = false;

            //지정위치로 이동
            Vector3 setPosition = modeOnPosition.position;
            Quaternion setRotation = modeOnPosition.rotation;
            charactorController.transform.position = setPosition;
            charactorController.transform.rotation = setRotation;

            //모드 체크
            miniatureMode = true;
            //StartCoroutine(ModeChangeTime());
        }
    }


    private void ExitMiniatureMode()
    {
        //Debug.Log("미니어처모드 나가기");
        // 지정 위치로 이동
        Vector3 setPosition = modeOffPosition.position;
        Quaternion setRotation = modeOffPosition.rotation;
        charactorController.transform.position = setPosition;
        charactorController.transform.rotation = setRotation;

        // 콜라이더 및 CharacterController 다시 활성화
        miniatureModeCollider.enabled = true;
        charactorController.enabled = true;

        // 움직이기 가능하게 설정
        dynamicMoveProvider.moveSpeed = 3;

        // 모드 체크 해제
        miniatureMode = false;
        //StartCoroutine(ModeChangeTime());
    }


    
    IEnumerator ModeChangeTime()
    {
        yield return new WaitForSeconds(0.1f);
        miniatureMode = !miniatureMode;
    }
}

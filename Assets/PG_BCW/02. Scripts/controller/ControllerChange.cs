using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControllerChange : MonoBehaviour
{
    [Tooltip("컨트롤러와 핸드트래킹 전환시 이벤트 추가 제거용")] 
    [SerializeField] XRInputModalityManager modalityManager;
    public XRInputModalityManager ModalityManager {  get { return modalityManager; } }

    [SerializeField] GameObject game;

    private void Awake()
    {
        if(modalityManager == null)
            modalityManager = GetComponent<XRInputModalityManager>();

        
    }

    private void OnEnable()
    {
        //modalityManager.trackedHandModeStarted.AddListener();
        //modalityManager.trackedHandModeEnded.AddListener();
        //modalityManager.motionControllerModeStarted.AddListener();
        //modalityManager.motionControllerModeEnded.AddListener();
    }

    private void OnDisable()
    {
        modalityManager.trackedHandModeStarted.RemoveAllListeners();
        modalityManager.trackedHandModeEnded.RemoveAllListeners();
    }


    private void RayCastOff()
    {

    }

    
}

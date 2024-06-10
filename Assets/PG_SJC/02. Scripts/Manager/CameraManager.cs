using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.SpatialTracking.TrackedPoseDriver;

public class CameraManager : Singleton<CameraManager>
{
    [Header("메인 카메라")]
    public Camera mainCamera;
    private CinemachineBrain cinemachineBrain;
    private TrackedPoseDriver trackedPose;
    private int originBlendStyle = 0;
    private float originBlendTime = 0f;

    [Header("플레이어 메인 카메라")]
    public CinemachineVirtualCamera playerVC;

    private CinemachineVirtualCamera currentVC;

    private void OnEnable()
    {
        MainCameraSetting();
    }

    // 메인 카메라 및 시네머신 브레인 초기세팅
    private void MainCameraSetting()
    {
        mainCamera = Camera.main;
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        if (cinemachineBrain == null) return;
        originBlendStyle = (int)cinemachineBrain.m_DefaultBlend.m_Style;
        originBlendTime = cinemachineBrain.m_DefaultBlend.m_Time;

        trackedPose = mainCamera.GetComponent<TrackedPoseDriver>();
    }

    // 플레이어 메인 카메라 세팅 (씬 변경될때마다 호출)
    public void PlayerCameraSetUp(CinemachineVirtualCamera playerVC)
    {
        this.playerVC = playerVC;

        // 기존 카메라 우선순위 적용
        if (currentVC != null)
            currentVC.Priority = 0;

        currentVC = playerVC;
        currentVC.Priority = 1;
    }

    // 카메라 우선순위 세팅
    public void SetPriority(CinemachineVirtualCamera vc = null, int style = -1, float blendTime = -1f)
    {
        if (playerVC == null || currentVC == null)
        {
            Debug.Log("메인 카메라가 존재하지 않습니다.");
            return;
        }

        if (mainCamera == null)
            MainCameraSetting();

        // 블렌드 타입 세팅
        if (style == -1)
            cinemachineBrain.m_DefaultBlend.m_Style = (CinemachineBlendDefinition.Style)originBlendStyle;
        else
            cinemachineBrain.m_DefaultBlend.m_Style = (CinemachineBlendDefinition.Style)style;

        if (blendTime == -1f)
            cinemachineBrain.m_DefaultBlend.m_Time = originBlendTime;
        else
            cinemachineBrain.m_DefaultBlend.m_Time = blendTime;

        if(vc == null)
        { 
            currentVC = playerVC;
            StartCoroutine(Extension.ActionDelay(blendTime, ()=>trackedPose.enabled = true));
        }
        else
        {
            trackedPose.enabled = false;
            currentVC = vc;
        }

        
        currentVC.Priority = 1;
    }
}
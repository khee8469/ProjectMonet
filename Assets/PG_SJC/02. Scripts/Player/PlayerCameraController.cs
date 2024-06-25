using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerVC;

    private void OnEnable()
    {
        Manager.Camera.MainCameraSetting();
        Manager.Camera.PlayerCameraSetUp(playerVC);
    }
}

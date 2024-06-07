using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    public static CameraManager Scene { get { return CameraManager.Instance; } }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        CameraManager.ReleaseInstance();


        // 싱글턴 객체생성
        CameraManager.CreateInstance();

    }
}
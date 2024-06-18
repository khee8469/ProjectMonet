using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    public static CameraManager Camera { get { return CameraManager.Instance; } }
    public static UIManager UI { get { return UIManager.Instance; } }
    public static AnimParamManager Param { get { return AnimParamManager.Instance; } }
    public static LayerManager Layer { get { return LayerManager.Instance; } }
    public static DataManager Data { get { return DataManager.Instance; } }
    public static QuestManager Quest { get { return QuestManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        CameraManager.ReleaseInstance();
        UIManager.ReleaseInstance();
        AnimParamManager.ReleaseInstance();
        LayerManager.ReleaseInstance();
        DataManager.ReleaseInstance();
        QuestManager.ReleaseInstance();

        // 싱글턴 객체생성
        CameraManager.CreateInstance();
        UIManager.CreateInstance();
        AnimParamManager.CreateInstance();
        LayerManager.CreateInstance();
        DataManager.CreateInstance();
        QuestManager.CreateInstance();
    }
}
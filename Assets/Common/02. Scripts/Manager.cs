using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JJH;

public static class Manager
{
    public static CameraManager Camera { get { return CameraManager.Instance; } }
    public static JJH.SceneManager Scene { get { return JJH.SceneManager.Instance; } }
    public static JJH.ChapterManager Chapter { get { return JJH.ChapterManager.Instance; } }
    public static Jc.UIManager UI { get { return Jc.UIManager.Instance; } }
    public static AnimParamManager Param { get { return AnimParamManager.Instance; } }
    public static LayerManager Layer { get { return LayerManager.Instance; } }
    public static Jc.DataManager Data { get { return Jc.DataManager.Instance; } }
    public static InventoryManager Inventory { get { return InventoryManager.Instance; } }
    public static QuestManager Quest { get { return QuestManager.Instance; } }
    public static PlableDataManager PlableData { get { return PlableDataManager.Instance;}}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        CameraManager.ReleaseInstance();
        Jc.UIManager.ReleaseInstance();
        JJH.SceneManager.ReleaseInstance();
        JJH.ChapterManager.ReleaseInstance();
        AnimParamManager.ReleaseInstance();
        LayerManager.ReleaseInstance();
        InventoryManager.ReleaseInstance();
        QuestManager.ReleaseInstance();
        Jc.DataManager.ReleaseInstance();
        PlableDataManager.ReleaseInstance();

        // 싱글턴 객체생성
        CameraManager.CreateInstance();
        Jc.UIManager.CreateInstance();
        JJH.SceneManager.ReleaseInstance();
        JJH.ChapterManager.CreateInstance();
        AnimParamManager.CreateInstance();
        LayerManager.CreateInstance();
        Jc.DataManager.CreateInstance();
        InventoryManager.CreateInstance();
        QuestManager.CreateInstance();
        PlableDataManager.CreateInstance();
    }
}
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
    public static QuestManager Quest { get { return QuestManager.Instance; } }
    public static PlayableDataManager PlayableData { get { return PlayableDataManager.Instance;}}
    public static SoundManager Sound { get { return SoundManager.Instance; }}
    public static ItemManager Item { get { return ItemManager.Instance;}}    


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        JJH.SceneManager.ReleaseInstance();
        AnimParamManager.ReleaseInstance();
        LayerManager.ReleaseInstance();
        QuestManager.ReleaseInstance();
        Jc.DataManager.ReleaseInstance();
        PlayableDataManager.ReleaseInstance();
        ItemManager.ReleaseInstance();
        Jc.UIManager.ReleaseInstance();
        CameraManager.ReleaseInstance();
        SoundManager.ReleaseInstance();
        JJH.ChapterManager.ReleaseInstance();


        // 싱글턴 객체생성
        JJH.SceneManager.CreateInstance();
        AnimParamManager.CreateInstance();
        LayerManager.CreateInstance();
        Jc.DataManager.CreateInstance();
        QuestManager.CreateInstance();
        PlayableDataManager.CreateInstance();
        ItemManager.CreateInstance();
        Jc.UIManager.CreateInstance();
        CameraManager.CreateInstance();
        SoundManager.CreateInstance();
        JJH.ChapterManager.CreateInstance();
    }
}
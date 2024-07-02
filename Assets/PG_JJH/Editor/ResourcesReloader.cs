#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ResourcesReloader
{
    [MenuItem("Tools/Reload Resources")]
    public static void ReloadResources()
    {
        // Resources 폴더에 있는 모든 TextAsset을 다시 불러옵니다.
        string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Resources" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log("Resources reloaded.");
    }
}
#endif
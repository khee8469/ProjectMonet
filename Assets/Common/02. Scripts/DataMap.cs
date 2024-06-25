
// 데이터 테이블 ID 매핑
using System.IO;
using UnityEngine;

/// <summary>
/// 데이터 매핑용 스크립트
/// </summary>
namespace Jc
{
    public static class DataID
    {
        // NPC ID
        public const int NPC = 1220000;
        // 퀘스트 ID
        public const int QUEST = 1510000;
        // 퀘스트 리스트 ID
        public const int QUEST_LIST = 2510000;
        // 나레이션 번들 ID
        public const int NARRATION_BUNDLE = 1600000;
        // 나레이션 ID
        public const int NARRATION = 1610000;
    }
    
    public static class SystemPath
    {
        public static string GetPath(string fileName)
        {
            string path = GetPath();
            return Path.Combine(GetPath(), fileName);
        }
        public static string GetPath()
        {
            string path = null;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = Application.persistentDataPath;
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(Application.persistentDataPath, "Resources/");
                case RuntimePlatform.WindowsEditor:
                    path = Application.dataPath;
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(path, "Assets", "Resources/");
                default:
                    path = Application.dataPath;
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(path, "Resources/");
            }
        }
    }

    public static class DataPath
    {
        // Json 플레이어블 데이터
        public const string LocalQuestData = "UserData/Quest_ListDT.csv";

        // Json 플레이어블 데이터
        public const string LocalInventoryData = "UserData/InventoryDT.csv";
    }

    public static class ResourcesPath
    {
        // 나레이션 번들 DT
        public const string NarrationBundleData = "DataTable/NarrationBundleDT";
        // 나레이션 DT
        public const string NarrationData = "DataTable/NarrationDT";
        // 퀘스트 DT
        public const string QuestData = "DataTable/QuestDT";
        // NPC DT
        public const string NPCData = "DataTable/NPCDT";
    }
}

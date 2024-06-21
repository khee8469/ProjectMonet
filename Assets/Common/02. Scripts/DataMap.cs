/// <summary>
/// 데이터 매핑용 스크립트
/// </summary>

// 데이터 테이블 ID 매핑
namespace Jc
{
    public static class DataID
    {
        // NPC ID
        public const int NPC = 1220000;
        // 퀘스트 ID
        public const int QUEST = 1510000;
        // 나레이션 번들 ID
        public const int NARRATION_BUNDLE = 1600000;
        // 나레이션 ID
        public const int NARRATION = 1610000;
    }
}

namespace Jc
{
    public static class DataPath
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

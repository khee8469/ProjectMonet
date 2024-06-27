using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class DataManager : Singleton<DataManager>
    {
        private Dictionary<int, List<int>> narrationBundleDic;  // 나레이션 번들 데이터 <나레이션 번들 ID, 나레이션 ID 리스트>
        public Dictionary<int, List<int>> NarrationBundleDic { get { return narrationBundleDic; } }

        private Dictionary<int, NarrtionData> narrationDataDic;           // <나레이션 ID, 나레이션 문자열>
        public Dictionary<int, NarrtionData> NarrationDataDic { get { return narrationDataDic; } }

        private Dictionary<int, NPCData> npcDataDic;        // NPC 데이터
        public Dictionary<int, NPCData> NPCDataDic { get { return npcDataDic; } }

        private Dictionary<int, QuestData> questDataDic;    // 퀘스트 데이터
        public Dictionary<int, QuestData> QuestDataDic { get { return questDataDic; } }

        //임시 테스트용
        private Dictionary<int, TestNPCData> testNpcDataDic;        // NPC 데이터
        public Dictionary<int, TestNPCData> TestNpcDataDic { get { return testNpcDataDic; } }


        /// <로딩순서>
        /// 1. CSV 데이터 로드
        /// 2. 로컬 데이터 로드 -> 덮어쓰기
        /// </로딩순서>
        protected override void Awake()
        {
            LoadCSVData();      // CSV 데이터 로드
        }

        #region CSV 데이터 로드
        /// <summary>
        /// CSV 데이터 로딩/캐싱
        /// </summary>
        private void LoadCSVData()
        {
            LoadNarrationData();        // 나레이션 데이터 로드
            LoadNarrationBundleData();  // 나레이션 번들 데이터 로드
            LoadQuestData();            // 퀘스트 데이터 로드
            LoadNPCData();              // NPC 데이터 로드

            LoadTestNPCData();          // Test NPC 데이터 로드
        }

        private void LoadNarrationData()
        {
            // CSVReader를 통한 csvData 읽기
            // Resources 폴더 내에 데이터 테이블이 존재해야함.
            List<Dictionary<string, object>> csvData = CSVHelper.Read(ResourcesPath.NarrationData);
            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("나레이션 데이터가 존재하지 않습니다.");
                return;
            }

            // 딕셔너리 객체화
            narrationDataDic = new Dictionary<int, NarrtionData>();

            for (int i = 0; i < csvData.Count; i++)
            {
                // 테이블 1행은 각 열이 Key 값으로 할당.
                // 매핑된 ID 값을 빼서 객체화된 딕셔너리에 할당.

                int narrationID = (int)csvData[i]["id"] - DataID.NARRATION;     // 나레이션 ID 할당

                // NarrationData 구조체 생성 후 로드한 데이터 할당.
                NarrtionData data = new NarrtionData();
                data.npcID = (int)csvData[i]["id_target"] - DataID.NPC;
                data.text = (string)csvData[i]["id_text"];

                narrationDataDic.Add(narrationID, data);
            }
        }
        private void LoadNarrationBundleData()
        {
            List<Dictionary<string, object>> csvData = CSVHelper.Read(ResourcesPath.NarrationBundleData);
            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("나레이션 번들데이터가 존재하지 않습니다.");
                return;
            }

            narrationBundleDic = new Dictionary<int, List<int>>();

            for (int i = 0; i < csvData.Count; i++)
            {
                int bundleID = (int)csvData[i]["id"] - DataID.NARRATION_BUNDLE;       // 번들 ID 할당
                List<int> narrationIDs = new List<int>();

                narrationBundleDic.Add(bundleID, narrationIDs);

                // 번들 당 나레이션의 개수는 30개
                for (int j = 1; j <= 30; j++)
                {
                    // 선형적 필드 (다음 열에 데이터가 없다면 break)
                    // 예외처리
                    if (csvData[i][$"id_nar_{j}"] is not int)
                        break;

                    int narrationID = (int)csvData[i][$"id_nar_{j}"] - DataID.NARRATION;    // 나레이션 ID 할당
                    // 리스트에 할당
                    narrationIDs.Add(narrationID);
                }
            }

        }
        private void LoadQuestData()
        {
            List<Dictionary<string, object>> csvData = CSVHelper.Read(ResourcesPath.QuestData);

            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("퀘스트 데이터가 존재하지 않습니다.");
                return;
            }

            questDataDic = new Dictionary<int, QuestData>();

            for (int i = 0; i < csvData.Count; i++)
            {
                int questID = (int)csvData[i]["id"] - DataID.QUEST;

                QuestData questData = new QuestData();

                questData.id = questID;
                questData.questName = csvData[i]["quest_name"] as string;
                questData.type = (QuestType)(int)csvData[i]["condition"];
                questData.npcID = (int)csvData[i]["quest_acc"] - DataID.NPC;
                questData.next_id = (int)csvData[i]["quest_next"] - DataID.QUEST;
                questData.receiveNarrationBundleID = (int)csvData[i]["narr_start"] - DataID.NARRATION_BUNDLE;
                questData.clearNarrationBundleID = (int)csvData[i]["narr_fin"] - DataID.NARRATION_BUNDLE;

                questDataDic.Add(questID, questData);
            }
        }
        private void LoadNPCData()
        {
            List<Dictionary<string, object>> csvData = CSVHelper.Read(ResourcesPath.NPCData);

            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("NPC 데이터가 존재하지 않습니다.");
                return;
            }

            npcDataDic = new Dictionary<int, NPCData>();

            for (int i = 0; i < csvData.Count; i++)
            {
                // 선형적 필드
                // NPC의 최대 퀘스트 갯수는 5개
                int id = (int)csvData[i]["id"] - DataID.NPC;
                NPCData data = new NPCData();
                data.id = id;
                data.npcName = (string)csvData[i]["name_npc"];
                data.questIDList = new List<int>();
                for (int j = 1; j <= 5; j++)
                {
                    if (csvData[i][$"id_quest_{j}"] is not int)
                        break;
                    data.questIDList.Add((int)csvData[i][$"id_quest_{j}"] - DataID.QUEST);
                }

                npcDataDic.Add(id, data);
            }
        }


        //테스트용
        private void LoadTestNPCData()
        {
            List<Dictionary<string, object>> csvData = CSVHelper.Read(ResourcesPath.TestNpcData);

            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("NPC 데이터가 존재하지 않습니다.");
                return;
            }

            npcDataDic = new Dictionary<int, NPCData>();

            for (int i = 0; i < csvData.Count; i++)
            {
                // 선형적 필드
                // NPC의 최대 퀘스트 갯수는 5개
                int id = (int)csvData[i]["id"] - DataID.NPC;
                NPCData data = new NPCData();
                data.id = id;
                data.npcName = (string)csvData[i]["name_npc"];
                data.questIDList = new List<int>();
                for (int j = 1; j <= 5; j++)
                {
                    if (csvData[i][$"id_quest_{j}"] is not int)
                        break;
                    data.questIDList.Add((int)csvData[i][$"id_quest_{j}"] - DataID.QUEST);
                }

                npcDataDic.Add(id, data);
            }
        }

        #endregion
    }
}

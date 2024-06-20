using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jc
{    
    // 퀘스트 상태타입
    //                      {비활성화,      활성화,  진행중, 수락대기}
    public enum QuestState { DisActive = 0, Active, Proceed, Clear}

    // 퀘스트 타입
    //                    { 기본형, 자동 클리어형, 연계형 } 
    public enum QuestType { Normal = 1, AutoClear, Link}

    public class QuestManager : Singleton<QuestManager>
    {
        // 데이터 테이블 파일경로
        private string path_narrationBundleData = "DataTable/NarrationBundleDT";
        private string path_narrationData = "DataTable/NarrationDT";
        private string path_questData = "DataTable/QuestDT";

        [SerializeField]
        private Dictionary<int, Quest> questDic;
        public Dictionary<int, Quest> QuestDic {get { return questDic; } }

        [SerializeField]
        private Dictionary<int, QuestData> questDataDic;
        public Dictionary<int, QuestData> QuestDataDic {get { return questDataDic; } }
        [SerializeField]
        private Dictionary<int, List<int>> narrationBundleDic;  // <나레이션 번들 ID, 나레이션 ID 리스트>
        public Dictionary<int, List<int>> NarrationBundleDic { get { return narrationBundleDic; } }
        [SerializeField]
        private Dictionary<int, NarrtionData> narrationDic;           // <나레이션 ID, 나레이션 문자열>
        public Dictionary<int, NarrtionData> NarrationDic {get { return narrationDic; } }
                                                                
        protected override void Awake()
        {
            base.Awake();

            LoadCSVData();
            RegistQuest();
        }

        // 퀘스트 등록
        private void RegistQuest()
        {
            questDic = new Dictionary<int, Quest>();
            Quest[] quests = Resources.LoadAll<Quest>($"Quests");

            foreach(Quest quest in quests) 
            {
                // id 예외처리
                if(quest.QuestID < 1)
                {
                    Debug.Log($"{quest} : QuestID가 할당되지 않았습니다.");
                    continue;
                }

                if (questDic.ContainsKey(quest.QuestID))
                {
                    Debug.Log($"{quest.QuestID}는 {questDic[quest.QuestID]}에 이미 할당 된 QuestID 입니다.");
                    continue;
                }

                // 데이터 id 예외처리
                if (!questDataDic.ContainsKey(quest.QuestID))
                    continue;

                // 퀘스트 생성
                Quest inst = Instantiate(quest, transform);
                QuestData data = questDataDic[quest.QuestID];
                // 퀘스트 데이터 할당
                inst.QuestData = data;

                // 수주 나레이션 할당
                inst.receiveNarrations = new List<NarrtionData>();
                if(narrationBundleDic.ContainsKey(data.receiveNarrationBundleID))
                {
                    foreach(int bundleID in narrationBundleDic[data.receiveNarrationBundleID])
                    {
                        if(!narrationDic.ContainsKey(bundleID))
                            break;

                        inst.receiveNarrations.Add(narrationDic[bundleID]);
                    }
                }

                // 클리어 나레이션 할당
                inst.clearNarrations = new List<NarrtionData>();
                if(narrationBundleDic.ContainsKey(data.clearNarrationBundleID))
                {
                    foreach (int bundleID in narrationBundleDic[data.clearNarrationBundleID])
                    {
                        if (!narrationDic.ContainsKey(bundleID))
                            break;

                        inst.clearNarrations.Add(narrationDic[bundleID]);
                    }
                }

                questDic.Add(quest.QuestID, inst);
            }
        }

        public void LoadCSVData()
        {
            LoadNarrationData();
            LoadNarrationBundleData();
            LoadQuestData();
            // 로컬에 세이브된 데이터가 있다면 다음 줄부터 로드 후 덮어쓰기 실행 
        }
        public void SaveQuestData()
        {

        }

        public Quest GetQuest(int id)
        {
            if(!questDic.ContainsKey(id))
            {
                Debug.Log($"{id}에 해당하는 퀘스트 데이터가 없습니다.");
                return null;
            }

            return questDic[id];
        }

        private void LoadNarrationData()
        {
            List<Dictionary<string, object>> csvData = CSVReader.Read(path_narrationData);
            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("나레이션 데이터가 존재하지 않습니다.");
                return;
            }

            narrationDic = new Dictionary<int, NarrtionData>();

            for(int i =0; i<csvData.Count; i++)
            {
                int narrationID = (int)csvData[i]["id"] - DataID.NARRATION;     // 나레이션 ID 할당
                NarrtionData data = new NarrtionData();
                data.npcID = (int)csvData[i]["id_target"] - DataID.NPC;
                data.text = (string)csvData[i]["id_text"];

                narrationDic.Add(narrationID, data);
            }
        }
        private void LoadNarrationBundleData()
        {
            List<Dictionary<string, object>> csvData = CSVReader.Read(path_narrationBundleData);
            if(csvData == null || csvData.Count < 1)
            {
                Debug.Log("나레이션 번들데이터가 존재하지 않습니다.");
                return;
            }

            narrationBundleDic = new Dictionary<int, List<int>>();

            for (int i =0; i<csvData.Count; i++)
            {
                int bundleID = (int)csvData[i]["id"] - DataID.NARRATION_BUNDLE;       // 번들 ID 할당
                List<int> narrationIDs = new List<int>();

                narrationBundleDic.Add(bundleID, narrationIDs);

                // 번들 당 나레이션의 개수는 30개
                for(int j=1; j<=30; j++)
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
            List<Dictionary<string, object>> csvData = CSVReader.Read(path_questData);

            if (csvData == null || csvData.Count < 1)
            {
                Debug.Log("퀘스트 데이터가 존재하지 않습니다.");
                return;
            }

            questDataDic = new Dictionary<int, QuestData>();

            for(int i =0; i<csvData.Count; i++)
            {
                int questID = (int)csvData[i]["id"] - DataID.QUEST;
                
                QuestData questData = ScriptableObject.CreateInstance<QuestData>();

                questData.id = questID;
                questData.questName = csvData[i]["quest_name"] as string;
                questData.type = (QuestType)(int)csvData[i]["condition"];
                questData.npcID = (int)csvData[i]["quest_acc"];
                questData.receiveNarrationBundleID = (int)csvData[i]["narr_start"] - DataID.NARRATION_BUNDLE;
                questData.clearNarrationBundleID = (int)csvData[i]["narr_fin"] - DataID.NARRATION_BUNDLE;

                questDataDic.Add(questID, questData);
            }
        }
    }
}
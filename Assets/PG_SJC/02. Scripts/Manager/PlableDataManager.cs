using JJH;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 게임 플레이 데이터 세이브/로드
    /// </summary>
    public class PlableDataManager : Singleton<PlableDataManager>
    {
        [SerializeField]
        private bool[] stageInfo;
        public bool[] StageInfo{ get { return stageInfo; } }

        [Header("에디터 세팅 (스테이지 정보 캐싱용)")]
        [SerializeField]
        private List<StageData> stageDatas;

        [SerializeField]
        private List<QuestListData> questStateDatas;
        [SerializeField]
        private List<SlotData> inventorySlotDatas { get; set; }
        public List<SlotData> InventorySlotDatas { get { return inventorySlotDatas; } }

        public Dictionary<int, bool> paintDataList;

        //미니어처 위치 구조체화 데이터 저장용
        private List<MiniatureData> miniatureDatas;
        public List<MiniatureData> MiniatureDatas { get { return miniatureDatas; } }
        //미니어처 위치 데이터 저장 딕셔너리, Resources에서 가져오거나 참조 지정
        [SerializeField]
        private PositionData positionData;
        public PositionData PositionData { get { return positionData; } }
        // 미니어처매니저에서 초기로드데이터 확인용
        private bool miniatureLoadData;
        public bool MiniatureLoadData { get { return miniatureLoadData; } }


        private void OnEnable()
        {
            Debug.Log(Application.persistentDataPath);

            InitSetting();
        }

        public void LoadStageData()
        {
            // 스테이지 정보 로딩 후 연계된 퀘스트 데이터 변경
            // 퀘스트 데이터 변경 -> 퍼즐 데이터 변경
            
            // 로컬 폴더가 존재하지 않을 경우
            if(!Directory.Exists(SystemPath.GetPath(DataPath.LocalDirectory)))
            {
                // 로컬 폴더 생성
                Directory.CreateDirectory(SystemPath.GetPath(DataPath.LocalDirectory));
                return;
            }

            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.StageData)))
            {
                stageInfo = new bool[4] { false, false, false, false };
                return;
            }

            // 스테이지 정보가 존재할 경우
            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.StageData));
            //stageInfo
        }
        public void SaveStageData()
        {
            string jsonData = JsonUtility.ToJson(stageInfo);

        }

        public void InitSetting()
        {
            paintDataList = new Dictionary<int, bool>();

            // 퀘스트 데이터 로드
            List<QuestListData> loadedQuestData = LoadQuestData();

            questStateDatas = loadedQuestData;
            // 로드된 데이터를 기반으로 퀘스트 상태 업데이트
            foreach (QuestListData data in questStateDatas)
            {
                Manager.Quest.QuestDic[data.id_quest].State = (QuestState)data.progress;
            }

            SaveQuestData();
        }
        // 퀘스트 데이터 저장
        public void SaveQuestData()
        {
            if (questStateDatas.Count < 1)
            {
                Debug.Log("퀘스트 데이터 리스트가 초기화되지 않았습니다.");
                return;
            }

            for (int i = 0; i < questStateDatas.Count; i++)
            {
                int id = questStateDatas[i].id;
                int id_quest = questStateDatas[i].id_quest;
                int progress = (int)Manager.Quest.QuestDic[id].State;

                if (progress == questStateDatas[i].progress)    // 데이터가 변경되지 않았다면 continue
                    continue;

                questStateDatas[i] = new QuestListData(id, id_quest, progress);
            }

            // 저장소가 없다면 생성
            if (!Directory.Exists(SystemPath.GetPath("UserData")))
                Directory.CreateDirectory(SystemPath.GetPath("UserData"));

            CSVHelper.Write(SystemPath.GetPath(DataPath.LocalQuestData), questStateDatas);
        }
        // 퀘스트 데이터 불러오기
        public List<QuestListData> LoadQuestData()
        {
            List<QuestListData> loadedData = new List<QuestListData>();

            // 초기화 
            foreach (int key in Manager.Quest.QuestDic.Keys)
            {
                loadedData.Add(new QuestListData(key, key, (int)Manager.Quest.QuestDic[key].State));
            }

            if (File.Exists(SystemPath.GetPath(DataPath.LocalQuestData)))
            {
                List<Dictionary<string, object>> csvData = CSVHelper.Read(SystemPath.GetPath(DataPath.LocalQuestData), true);

                // 불러온 데이터가 있다면 덮어쓰기 진행
                if (csvData != null && csvData.Count >= 1)
                {
                    for (int i = 0; i < csvData.Count; i++)
                    {
                        loadedData[i] = new QuestListData((int)csvData[i]["id"], (int)csvData[i]["id_quest"], (int)csvData[i]["progress"]);
                    }
                }
            }

            return loadedData;
        }

    }
}

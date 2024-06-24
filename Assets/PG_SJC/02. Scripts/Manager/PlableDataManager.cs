using JetBrains.Annotations;
using JJH;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Jc
{
    /// <summary>
    /// 게임 플레이 데이터 세이브/로드
    /// </summary>
    public class PlableDataManager : Singleton<PlableDataManager>
    {
        [SerializeField]
        private List<QuestListData> questStateDatas;
        [SerializeField]
        private List<InventorySlotStateData> inventorySlotStateDatas;

        [Header("테스트모드 (false : 새로 시작) (true : 불러오기)")]
        public bool isLoadMode = false;

        [SerializeField]
        private string questJson;
        [SerializeField]
        private string inventoryJson;

        private void OnEnable()
        {
            InitSetting();
        }

        public void InitSetting()
        {
            // 퀘스트 데이터 로드
            List<QuestListData> loadedQuestData = LoadQuestData();

            questStateDatas = loadedQuestData;
            // 로드된 데이터를 기반으로 퀘스트 상태 업데이트
            foreach (QuestListData data in questStateDatas)
            {
                Manager.Quest.QuestDic[data.id_quest].State = (QuestState)data.progress;
            }


            // 인벤토리 슬롯 데이터 로드
            List<InventorySlotStateData> loadedInventoryData = LoadSlotData();
            if (isLoadMode || loadedInventoryData == null || loadedInventoryData.Count < 1)
            {
                loadedInventoryData = new List<InventorySlotStateData>();
                // *****추후 데이터 테이블 제작후 구현예정
            }

            inventorySlotStateDatas = loadedInventoryData;
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

            CSVHelper.Write(Path.Combine("Assets/PG_SJC/Resources/", DataPath.LocalQuestData), questStateDatas);
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

            List<Dictionary<string, object>> csvData = CSVHelper.Read(DataPath.LocalQuestData);

            // 불러온 데이터가 있다면 덮어쓰기 진행
            if (csvData != null && csvData.Count >= 1)
            {
                for (int i = 0; i < csvData.Count; i++)
                {
                    loadedData[i] = new QuestListData((int)csvData[i]["id"], (int)csvData[i]["id_quest"], (int)csvData[i]["progress"]);
                }
            }


            return loadedData;
        }

        // 인벤토리 슬롯 데이터 저장
        public void SaveSlotData()
        {

        }

        public List<InventorySlotStateData> LoadSlotData()
        {
            List<InventorySlotStateData> loadedData = new List<InventorySlotStateData>();

            return loadedData;
        }
    }

}

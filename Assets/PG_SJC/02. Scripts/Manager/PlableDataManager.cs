using JetBrains.Annotations;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 게임 플레이 데이터 세이브/로드
    /// </summary>
    public class PlableDataManager : Singleton<PlableDataManager>
    {
        private List<QuestStateData> questStateDatas;
        private List<InventorySlotStateData> inventorySlotStateDatas;

        [Header("테스트모드 (false : 데이터로드) (true : 새로 시작)")]
        public bool isLoadMode = false;

        public void InitSetting()
        {
            // 퀘스트 데이터 로드
            List<QuestStateData> loadedQuestData = LoadQuestData();
            if (isLoadMode || loadedQuestData == null || loadedQuestData.Count < 1)
            {
                loadedQuestData = new List<QuestStateData>();

                foreach(int key in Manager.Quest.QuestDic.Keys)
                {
                    loadedQuestData.Add(new QuestStateData(key, (int)Manager.Quest.QuestDic[key].State));
                }
            }

            questStateDatas = loadedQuestData;

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
            string jsonData = JsonUtility.ToJson(questStateDatas);
            // **********작성중
        }

        public List<QuestStateData> LoadQuestData()
        {
            List<QuestStateData> loadedData = new List<QuestStateData>();

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

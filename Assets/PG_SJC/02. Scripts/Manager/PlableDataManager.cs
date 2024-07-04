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
        private List<QuestListData> questStateDatas;
        [SerializeField]
        private List<SlotData> inventorySlotDatas { get; set; }
        public List<SlotData> InventorySlotDatas { get { return inventorySlotDatas; } }

        public Dictionary<int, bool> paintDataList;

        [Header("테스트모드 (false : 새로 시작) (true : 불러오기)")]
        public bool isLoadMode = false;

        [SerializeField]
        private string questJson;
        [SerializeField]
        private string inventoryJson;



        //미니어처 위치 구조체화 데이터 저장용
        private List<MiniatureData> miniatureDatas;
        public List<MiniatureData> MiniatureDatas { get { return miniatureDatas; } }
        //미니어처 위치 데이터 저장 딕셔너리, Resources에서 가져오거나 참조 지정
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
        //public void InitSlot()
        //{
        //    // 인벤토리 슬롯 데이터 로드
        //    List<SlotData> loadedInventoryData = LoadSlotData();
        //    inventorySlotDatas = loadedInventoryData;
        //}
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
        // 인벤토리 슬롯 데이터 저장
        //        public void SaveSlotData()
        //        {
        //            if (inventorySlotDatas.Count < 1)
        //            {
        //                Debug.Log("슬롯 데이터 리스트가 초기화되지 않았습니다.");
        //                return;
        //            }

        //            foreach (var key in Manager.Inventory.inventorySlots.Keys)
        //            {
        //                InventorySlot slot = Manager.Inventory.inventorySlots[key];
        //                inventorySlotDatas[key - 1] = new SlotData(slot.slotID, slot.ItemID, slot.ItemCount);
        //            }

        //            CSVHelper.Write(SystemPath.GetPath(DataPath.LocalInventoryData), inventorySlotDatas);

        //#if UNITY_EDITOR
        //            AssetDatabase.Refresh();
        //#endif
        //        }
        //public List<SlotData> LoadSlotData()
        //{
        //    List<SlotData> loadedData = new List<SlotData>();
        //    // 초기화 
        //    foreach (int key in Manager.Inventory.inventorySlots.Keys)
        //    {
        //        SlotData data = new SlotData(0, -1, 0);
        //        loadedData.Add(data);
        //    }

        //    if (File.Exists(SystemPath.GetPath(DataPath.LocalInventoryData)))
        //    {
        //        List<Dictionary<string, object>> csvData = CSVHelper.Read(SystemPath.GetPath(DataPath.LocalInventoryData), true);

        //        // 불러온 데이터가 있다면 덮어쓰기 진행
        //        if (csvData != null && csvData.Count >= 1)
        //        {
        //            for (int i = 0; i < csvData.Count; i++)
        //            {

        //                SlotData loadSlot = new SlotData();
        //                loadSlot.id_slot = (int)csvData[i]["id_slot"];
        //                if (csvData[i]["id_item"] is int)
        //                    loadSlot.id_item = (int)csvData[i]["id_item"];
        //                if (csvData[i]["count"] is int)
        //                    loadSlot.count = (int)csvData[i]["count"];

        //                loadedData[loadSlot.id_slot - 1] = loadSlot;

        //            }
        //        }
        //    }

        //    return loadedData;
        //}




        /// <summary>
        /// 미니어처 데이터 관리
        /// </summary>

        // 포지션 데이터 구조체로 변환해서 저장
        public void SaveMiniatureData()
        {
            if (miniatureDatas.Count < 1)
            {
                Debug.Log("미니어처 데이터 리스트가 초기화되지 않았습니다.");
                return;
            }
            //딕셔너리데이터를 구조체에 저장
            foreach (int key in positionData.SavePosition_3.Keys)
            {
                //Debug.Log(key);
                int miniatureId = key;
                float x = positionData.SavePosition_3[key].x;
                float y = positionData.SavePosition_3[key].y;
                float z = positionData.SavePosition_3[key].z;

                miniatureDatas[key] = new MiniatureData(miniatureId, new Vector3(x, y, z));
            }
            // 저장소가 없다면 생성
            if (!Directory.Exists(SystemPath.GetPath("UserData")))
                Directory.CreateDirectory(SystemPath.GetPath("UserData"));
            //miniatureDatas 데이터 csv파일로 세이브
            CSVHelper.Write(SystemPath.GetPath(DataPath.LocalMiniatureData), miniatureDatas);
        }

        // 미니어처 위치 데이터 불러오기
        public void LoadMiniatureData()
        {
            List<MiniatureData> loadedData = new List<MiniatureData>();

            //데이터를 로드
            if (File.Exists(SystemPath.GetPath(DataPath.LocalMiniatureData)))
            {
                List<Dictionary<string, object>> csvData = CSVHelper.Read(SystemPath.GetPath(DataPath.LocalMiniatureData), true);

                // 불러온 데이터가 있다면 덮어쓰기 진행
                if (csvData != null && csvData.Count >= 1)
                {
                    for (int i = 0; i < csvData.Count; i++)
                    {
                        int id = Convert.ToInt32(csvData[i]["miniatureId"]);
                        float x = Convert.ToSingle(csvData[i]["xPosition"]);
                        float y = Convert.ToSingle(csvData[i]["yPosition"]);
                        float z = Convert.ToSingle(csvData[i]["zPosition"]);
                        loadedData.Add(new MiniatureData(id, new Vector3(x, y, z)));
                    }
                }

                //csv화용 구조체에 저장
                miniatureDatas = loadedData;
            }
            else
            {
                Debug.Log("미니어처 초기 데이터 없음");
            }

            // miniatureDatas 데이터를 딕셔너리에 저장
            LoadPositionData();
        }

        //로드된 데이터를 딕셔너리에 저장
        public void LoadPositionData()
        {
            if(miniatureDatas.Count == 0)
            {
                Debug.Log("세팅 할 데이터가 없음");
                return;
            }

            foreach (MiniatureData data in miniatureDatas)
            {
                if (!positionData.SavePosition_3.ContainsKey(data.miniatureId))
                {
                    positionData.SavePosition_3[data.miniatureId] = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                }
            }
        }
        //딕셔너리를 구조체 데이터로 변환
        public void SavePositionData()
        {
            miniatureDatas.Clear();
            // 초기화 
            foreach (int key in positionData.SavePosition_3.Keys)
            {
                Debug.Log("구조체 초기데이터 입력");
                miniatureDatas.Add(new MiniatureData(key, new Vector3(positionData.SavePosition_3[key].x, positionData.SavePosition_3[key].y, positionData.SavePosition_3[key].z)));
            }
        }
    }
}

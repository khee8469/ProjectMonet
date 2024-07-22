using JJH;
using Newtonsoft.Json;
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
    public class PlayableDataManager : Singleton<PlayableDataManager>
    {
        // 물감 수령 데이터 딕셔너리
        // 추후 아이템 데이터 딕셔너리로 통합 예정
        public Dictionary<int, bool> paintDataList;
        // 퍼즐 데이터 딕셔너리
        public Dictionary<int, PuzzleState> puzzleDataDic;
        // 슬롯 데이터 리스트
        public Dictionary<int, SlotData> slotDataDic;
        // 아이템 사용정보 데이터 딕셔너리
        public Dictionary<int, ItemInfoData> itemInfoDataDic;

        [Header("인벤토리 슬롯 총 개수")]
        public int slotCount;

        protected override void Awake()
        {
            base.Awake();
            RegistSlot();
        }

        private void OnEnable()
        {
            InitSetting();
        }

        public void InitSetting()
        {
            paintDataList = new Dictionary<int, bool>();
            itemInfoDataDic = new Dictionary<int, ItemInfoData>();

            LocalDirectoryInit();
            // 아이템 무결성을 위해
            // 아이템 -> 퍼즐 -> 퀘스트 -> 슬롯 단위로 데이터 로드
            LoadItemData();
            LoadPuzzleData();
            LoadQuestData();
        }
        private void RegistSlot()
        {
            if (slotDataDic == null)
                slotDataDic = new Dictionary<int, SlotData>();

            for(int i=1; i<=slotCount; i++)
            {
                slotDataDic.Add(i,new SlotData(i, -1, 0));
            }
        }
        // 로컬 폴더 초기세팅
        private void LocalDirectoryInit()
        {
            // 로컬 폴더가 존재하지 않을 경우
            if (!Directory.Exists(SystemPath.GetPath(DataPath.LocalDirectory)))
            {
                // 로컬 폴더 생성
                Directory.CreateDirectory(SystemPath.GetPath(DataPath.LocalDirectory));
            }
        }

        public void SaveItemData()
        {
            List<ItemInfoData> itemInfoDatas = new List<ItemInfoData>();
            // Dictionary to List
            foreach (int key in itemInfoDataDic.Keys)
            {
                if(itemInfoDataDic[key].isInventoryItem)
                    itemInfoDatas.Add(new ItemInfoData(key, Manager.Item.ItemDataDic[key].itemName, itemInfoDataDic[key].isAccepted, itemInfoDataDic[key].isClear, true));
                else
                    itemInfoDatas.Add(new ItemInfoData(key, $"\"로비 : {key}\"", itemInfoDataDic[key].isAccepted, itemInfoDataDic[key].isClear, false));
            }

            // 직렬화한 데이터 쓰기
            string jsonData = JsonConvert.SerializeObject(itemInfoDatas);
            File.WriteAllText(SystemPath.GetPath(DataPath.LocalItemInfoData), jsonData);
        }
        public void LoadItemData()
        {
            itemInfoDataDic = new Dictionary<int, ItemInfoData>();
            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.LocalItemInfoData)))
            {
                Debug.Log("로컬 폴더에 퍼즐 데이터가 존재하지 않습니다.");
                return;
            }

            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.LocalItemInfoData));
            List<ItemInfoData> itemInfoDatas = JsonConvert.DeserializeObject<List<ItemInfoData>>(jsonData);

            // 예외처리
            if (itemInfoDatas == null || itemInfoDatas.Count < 1)
                return;

            // 구조체 데이터 딕셔너리로 변환
            foreach (ItemInfoData data in itemInfoDatas)
            {
                if (itemInfoDataDic.ContainsKey(data.itemID))
                {
                    Debug.Log($"동일한 Key({data.itemID})값의 아이템 데이터가 이미 존재합니다.");
                    break;
                }
                itemInfoDataDic.Add(data.itemID, data);

                // 수령했지만 사용완료하지않은 아이템이라면 슬롯에 그대로 할당
                if(data.isAccepted && !data.isClear && data.isInventoryItem)
                {
                    int emptySlotID = -1;
                    // 빈 슬롯을 찾아 할당
                    foreach(int key in slotDataDic.Keys)
                    {
                        if (emptySlotID == -1 && slotDataDic[key].slotItemID == -1)
                            emptySlotID = key;
                            
                        if (slotDataDic[key].slotItemID == data.itemID)
                        {
                            emptySlotID = -1;
                            break;
                        }
                    }

                    if (emptySlotID != -1)
                        slotDataDic[emptySlotID] = new SlotData(emptySlotID, data.itemID, 1);
                }
            }
        }

        // 아이템이 슬롯에 존재하는지 확인
        public bool CheckItemInInventory(int itemID)
        {
            foreach(SlotData slot in slotDataDic.Values)
            {
                if (slot.slotItemID == itemID)
                    return true;
            }
            return false;
        }
        // 비어있는 슬롯 리턴
        public int FindEmptySlot()
        {
            foreach(int key in slotDataDic.Keys)
            {
                if (slotDataDic[key].slotItemID == -1)
                    return key;
            }
            return -1;
        }


        public void LoadPuzzleData()
        {
            puzzleDataDic = new Dictionary<int, PuzzleState>();
            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.LocalPuzzleData)))
            {
                Debug.Log("로컬 폴더에 퍼즐 데이터가 존재하지 않습니다.");
                return;
            }

            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.LocalPuzzleData));
            List<PuzzleData> puzzleDatas = JsonConvert.DeserializeObject<List<PuzzleData>>(jsonData);
            // 예외처리
            if (puzzleDatas == null || puzzleDatas.Count < 1)
                return;
            
            // 구조체 데이터 딕셔너리로 변환
            foreach(PuzzleData data in puzzleDatas)
            {
                if(puzzleDataDic.ContainsKey(data.puzzleID))
                {
                    Debug.Log($"동일한 Key({data.puzzleID})값의 퍼즐 데이터가 이미 존재합니다.");
                    break;
                }
                puzzleDataDic.Add(data.puzzleID, (PuzzleState)data.puzzleState);
            }
        }
        public void SavePuzzleData()
        {
            List<PuzzleData> puzzleData = new List<PuzzleData>();
            // Dictionary to List
            foreach (int key in puzzleDataDic.Keys)
            {
                puzzleData.Add(new PuzzleData(key, (int)puzzleDataDic[key]));
            }

            // 직렬화한 데이터 쓰기
            string jsonData = JsonConvert.SerializeObject(puzzleData); 
            File.WriteAllText(SystemPath.GetPath(DataPath.LocalPuzzleData), jsonData);
        }
  
        public void LoadQuestData()
        {
            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.LocalQuestData)))
            {
                Debug.Log("로컬 폴더에 퍼즐 데이터가 존재하지 않습니다.");
                return;
            }

            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.LocalQuestData));
            List<QuestListData> questListData = JsonConvert.DeserializeObject<List<QuestListData>>(jsonData);
            // 예외처리
            if (questListData == null || questListData.Count < 1)
            {
                Debug.Log("로드할 퀘스트 데이터가 존재하지 않습니다.");
                return;
            }

            // 구조체 데이터 딕셔너리로 변환
            foreach (QuestListData data in questListData)
            {
                if (!Manager.Quest.QuestDic.ContainsKey(data.id_quest))
                {
                    Debug.LogError($"Key({data.id_quest})값의 퀘스트가 존재하지 않습니다.");
                    break;
                }
                Manager.Quest.QuestDic[data.id_quest].State = (QuestState)data.progress;
                if ((QuestState)data.progress == QuestState.Proceed || (QuestState)data.progress == QuestState.Clear)
                    StartCoroutine(Extension.ActionDelay(0.5f,()=>Manager.UI.CreateEntry(Manager.Quest.QuestDic[data.id_quest])));
            }

        }
        public void SaveQuestData()
        {
            List<QuestListData> questListData = new List<QuestListData>();
            // Dictionary to List
            foreach (int key in Manager.Quest.QuestDic.Keys)
            {
                questListData.Add(new QuestListData(key, key, (int)Manager.Quest.QuestDic[key].State));
            }

            // 직렬화한 데이터 쓰기
            string jsonData = JsonConvert.SerializeObject(questListData);
            File.WriteAllText(SystemPath.GetPath(DataPath.LocalQuestData), jsonData);
        }







        // 채색 데이터 저장 

        private CanvasData canvasData; // 채색 관련 데이터가 저장된 data 스크립트 
        public CanvasData CanvasData { get { return canvasData; } }


        public void SaveCanvasData()
        {
            if(Directory.Exists(DataPath.LocalCanvasData) ==false)
            {
                Debug.Log("디렉토리가 존재하지 않음 -> 캔버스 데이터");
                Directory.CreateDirectory(DataPath.LocalCanvasData); // 디렉토리 생성? 
            }
            string json = JsonUtility.ToJson(canvasData , true);
            File.WriteAllText(SystemPath.GetPath(DataPath.LocalCanvasData), json);
        }
        public void LoadCanvasData()
        {
            if(!File.Exists(SystemPath.GetPath(DataPath.LocalCanvasData)))
            {               
                NewData();
                return;
            }

            string json = File.ReadAllText(SystemPath.GetPath(DataPath.LocalCanvasData));
            try
            {
                canvasData = JsonUtility.FromJson<CanvasData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Load data fail : {ex.Message}");
                NewData(); 
            }
        }
        public void NewData()
        {
            canvasData = new CanvasData(); // 새로운 데이터 생성. --> 데이터가 없을 시 . 
        }

    }
}

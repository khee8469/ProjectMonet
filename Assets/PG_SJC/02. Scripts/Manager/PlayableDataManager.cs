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
        // 슬롯 데이터 딕셔너리
        public Dictionary<int, SlotData> slotDataDic;
        // 아이템 사용정보 데이터 딕셔너리
        public Dictionary<int, ItemInfoData> itemSaveDataDic;

        private void OnEnable()
        {
            InitSetting();
        }

        public void InitSetting()
        {
            paintDataList = new Dictionary<int, bool>();
            slotDataDic = new Dictionary<int, SlotData>();
            // 퍼즐 -> 퀘스트 -> 스테이지 단위로 데이터 로드
            LoadPuzzleData();
            LoadQuestData();
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

        public void LoadPuzzleData()
        {
            LocalDirectoryInit();

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
            LocalDirectoryInit();

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
            LocalDirectoryInit();

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
            }

        }
        public void SaveQuestData()
        {
            LocalDirectoryInit();

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
    }
}

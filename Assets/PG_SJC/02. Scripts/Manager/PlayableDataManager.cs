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
        // -1 : 진행중
        // 3 : Clear - 완료 대기 (채색만 완료)
        // 4 : Complete - 완료
        [SerializeField]
        private int[] stageInfo;
        public int[] StageInfo{ get { return stageInfo; } }

        [Header("에디터 세팅 (스테이지 정보 캐싱용)")]
        [SerializeField]
        private List<StageData> stageDatas;

        [SerializeField]
        private List<SlotData> inventorySlotDatas { get; set; }
        public List<SlotData> InventorySlotDatas { get { return inventorySlotDatas; } }

        // 물감 수령 데이터 딕셔너리
        // 추후 아이템 데이터 딕셔너리로 통합 예정
        public Dictionary<int, bool> paintDataList;

        // 퍼즐 데이터 딕셔너리
        public Dictionary<int, PuzzleState> puzzleDataDic;


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

        [SerializeField]
        private List<PuzzleData> puzzleData;

        private void OnEnable()
        {
            Debug.Log(Application.persistentDataPath);

            InitSetting();
        }

        public void InitSetting()
        {
            paintDataList = new Dictionary<int, bool>();
            // 퍼즐 -> 퀘스트 -> 스테이지 단위로 데이터 로드
            LoadStageData();
            LoadPuzzleData();
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
                stageInfo = new int[4] { -1, -1, -1, -1 };
                return;
            }

            // 스테이지 정보가 존재할 경우
            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.StageData));
            // 데이터 불러오기
            stageInfo = JsonUtility.FromJson<int[]>(jsonData);

            // 불러온 데이터를 기반으로 상태 업데이트
            for(int i = 0; i<stageInfo.Length; i++)
            {
                // 갱신할 정보가 없다면 (false일 경우)
                // break;
                if (stageInfo[i] == -1 || stageDatas == null 
                    || stageDatas[i].linkedQuestID == null || stageDatas[i].linkedQuestID.Count < 1)
                {
                    Debug.Log($"{i + 1} 스테이지 정보는 갱신되지 않습니다.");
                    break;
                }

                #region 퀘스트 관련 불러오기
                // 스테이지에 해당하는 퀘스트 상태 업데이트 
                foreach (int id in stageDatas[i].linkedQuestID)
                {
                    int questID = id - DataID.QUEST;
                    if(!Manager.Quest.QuestDic.ContainsKey(questID))
                    {
                        Debug.Log($"{questID}에 해당하는 퀘스트가 존재하지 않습니다.");
                        break;
                    }
                    // 연관된 퀘스트는 모두 Complete 상태로 할당
                    Manager.Quest.QuestDic[questID].State = QuestState.Complete;
                }
                // 색칠하기 퀘스트는 저장된 퀘스트의 상태에 따라 업데이트 방식을 변경
                Manager.Quest.QuestDic[stageDatas[i].baseQuestID - DataID.QUEST].State = (QuestState)stageInfo[i];
                #endregion
            }

            // 불러온 데이터를 기반으로 연결된 캔버스 / 채색 상태 업데이트
        }
        public void SaveStageData()
        {
            // 로컬 폴더가 존재하지 않을 경우
            if (!Directory.Exists(SystemPath.GetPath(DataPath.LocalDirectory)))
            {
                // 로컬 폴더 생성
                Directory.CreateDirectory(SystemPath.GetPath(DataPath.LocalDirectory));
                return;
            }

            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.StageData)))
            {
                stageInfo = new int[4] { -1, -1, -1, -1 };
                return;
            }

            // 직렬화한 데이터 쓰기
            string jsonData = JsonUtility.ToJson(stageInfo);
            File.WriteAllText(SystemPath.GetPath(DataPath.StageData), jsonData);
        }

        public void LoadPuzzleData()
        {
            // 로컬 폴더가 존재하지 않을 경우
            if (!Directory.Exists(SystemPath.GetPath(DataPath.LocalDirectory)))
            {
                // 로컬 폴더 생성
                Directory.CreateDirectory(SystemPath.GetPath(DataPath.LocalDirectory));
                return;
            }

            puzzleDataDic = new Dictionary<int, PuzzleState>();
            // 스테이지 데이터 정보가 존재하지 않을 경우
            if (!File.Exists(SystemPath.GetPath(DataPath.LocalPuzzleData)))
            {
                Debug.Log("로컬 폴더에 퍼즐 데이터가 존재하지 않습니다.");
                return;
            }

            string jsonData = File.ReadAllText(SystemPath.GetPath(DataPath.LocalPuzzleData));
            PuzzleData[] puzzleDatas = JsonUtility.FromJson<PuzzleData[]>(jsonData);
            // 예외처리
            if (puzzleDatas == null || puzzleDatas.Length < 1)
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
            // 로컬 폴더가 존재하지 않을 경우
            if (!Directory.Exists(SystemPath.GetPath(DataPath.LocalDirectory)))
            {
                // 로컬 폴더 생성
                Directory.CreateDirectory(SystemPath.GetPath(DataPath.LocalDirectory));
                return;
            }

            puzzleData = new List<PuzzleData>();
            // Dictionary to List
            foreach (int key in puzzleDataDic.Keys)
            {
                Debug.Log($"ID : {key} 퍼즐 저장");
                puzzleData.Add(new PuzzleData(key, (int)puzzleDataDic[key]));
            }

            // 직렬화한 데이터 쓰기
            string jsonData = JsonUtility.ToJson(puzzleData.ToArray()); 
            File.WriteAllText(SystemPath.GetPath(DataPath.LocalPuzzleData), jsonData);
        }
    }


}

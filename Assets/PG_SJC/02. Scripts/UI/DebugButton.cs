using Jc;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Timeline;
namespace Jc
{
    [Serializable]
    public struct DebugItem
    {
        public List<ItemInfoData> infoDatas;
    }
    [Serializable]
    public struct DebugCanvas
    {
        public int partCount;
        public int clearCount;
        public int unLockCount;

        public DebugCanvas(int partCount, int clearCount, int unLockCount)
        {
            this.partCount = partCount;
            this.clearCount = clearCount;
            this.unLockCount = unLockCount;
        }
    }

    public class DebugButton : MonoBehaviour
    {
        public List<int> puzzleClearCount = new List<int>();
        public List<int> questClearCount = new List<int>();
        public List<DebugCanvas> canvasData = new List<DebugCanvas>();
        public List<DebugItem> itemInfoDatas = new List<DebugItem>();

        // 임시
        // 저장된 데이터 삭제 후 다시 로드
        public void OnClickResetButton()
        {
            RemoveData();
            Application.Quit();
        }

        public void OnClickStageButton(int stageIndex)
        {
            RemoveData();
            // 인덱스는 0부터 할당
            int curStageIndex = stageIndex - 1;

            // 슬롯 비우기
            for (int i = 1; i <= 6; i++)
            {
                Manager.PlayableData.slotDataDic[i] = new SlotData(i, -1, 0);
            }

            // 아이템 데이터 재할당
            Manager.PlayableData.itemInfoDataDic.Clear();
            for (int i = 0; i < curStageIndex + 1; i++)
            {
                List<ItemInfoData> itemInfos = itemInfoDatas[i].infoDatas;
                foreach (ItemInfoData data in itemInfos)
                {
                    if (Manager.PlayableData.itemInfoDataDic.ContainsKey(data.itemID))
                        Manager.PlayableData.itemInfoDataDic[data.itemID] = data;
                    else
                        Manager.PlayableData.itemInfoDataDic.Add(data.itemID, data);
                }
            }
            Manager.PlayableData.SaveItemData();

            // 퍼즐 데이터 재할당
            Manager.PlayableData.puzzleDataDic.Clear();
            for (int i = 0; i < puzzleClearCount[curStageIndex]; i++)
            {
                Manager.PlayableData.puzzleDataDic.Add(i + 1, PuzzleState.Clear);
            }
            Manager.PlayableData.SavePuzzleData();

            // 퀘스트 데이터 재할당
            int clearCount = questClearCount[curStageIndex];
            for (int i = 1; i <= Manager.Quest.QuestDic.Count; i++)
            {
                int questID = i + DataID.QUEST;
                Manager.Quest.QuestDic[questID].State = QuestState.Complete;
                // 다음 퀘스트 활성화
                if (clearCount > 0)
                    Manager.Quest.QuestDic[questID].State = QuestState.Complete;
                else if (clearCount == 0)
                    Manager.Quest.QuestDic[questID].State = QuestState.Active;
                else
                    Manager.Quest.QuestDic[questID].State = QuestState.DisActive;

                clearCount--;
            }
            Manager.PlayableData.SaveQuestData();

            // 캔버스 데이터 재할당
            Manager.PlayableData.CanvasData = new JJH.CanvasData();
            DebugCanvas debugCanvas = canvasData[curStageIndex];

            int checkCount = debugCanvas.partCount;
            for (int j = 0; j < Manager.PlayableData.CanvasData.myDrawPartCheckArr.Length; j++)
            {
                Manager.PlayableData.CanvasData.myDrawPartCheckArr[j] = checkCount > 0 ? true : false;
                checkCount--;
            }
            checkCount = debugCanvas.clearCount;
            for (int j = 0; j < Manager.PlayableData.CanvasData.isColoredCheckArr.Length; j++)
            {
                Manager.PlayableData.CanvasData.isColoredCheckArr[j] = checkCount > 0 ? true : false;
                checkCount--;
            }
            checkCount = debugCanvas.unLockCount;
            for (int j = 0; j < Manager.PlayableData.CanvasData.stageUnlockStatus.Length; j++)
            {
                Manager.PlayableData.CanvasData.stageUnlockStatus[j] = checkCount > 0 ? true : false;
                checkCount--;
            }
            Manager.PlayableData.SaveCanvasData();
            Manager.Chapter.DebugInitSetting();
            Manager.Scene.LoadScene("Lobby");
        }

        private void RemoveData()
        {
            //// 세이브 폴더 내 파일 삭제
            CSVHelper.Remove(SystemPath.GetPath(DataPath.LocalCanvasData)); // 캔버스 json 데이터 삭제 
            CSVHelper.Remove(SystemPath.GetPath(DataPath.LocalItemInfoData));
            CSVHelper.Remove(SystemPath.GetPath(DataPath.LocalPuzzleData));
            CSVHelper.Remove(SystemPath.GetPath(DataPath.LocalQuestData));
        }
    }
}
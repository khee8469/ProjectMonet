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
        [SerializeField]
        private Dictionary<int, Quest> questDic;    // 퀘스트 리소스 (ID 매핑)
        public Dictionary<int, Quest> QuestDic { get { return questDic; } }

        protected override void Awake()
        {
            base.Awake();
            InitSetting();
        }

        // 데이터 매니저의 데이터 할당 종료 시 
        #region 퀘스트 할당

        private void InitSetting()
        {
            RegistQuest();   // 퀘스트 리소스 등록
            LoadQuestData(); // 퀘스트 로컬 데이터 불러오기 및 덮어쓰기
        }

        /// <summary>
        /// 퀘스트 리소스 등록 및 생성 (데이터 할당)
        /// </summary>
        private void RegistQuest()
        {
            questDic = new Dictionary<int, Quest>();
            Quest[] quests = Resources.LoadAll<Quest>($"Quests");

            foreach (Quest quest in quests)
            {
                // id 예외처리
                if (quest.QuestID < 1)
                {
                    Debug.Log($"{quest} : QuestID가 할당되지 않았습니다.");
                    continue;
                }

                // 딕셔너리 예외처리
                if (questDic.ContainsKey(quest.QuestID))
                {
                    Debug.Log($"{quest.QuestID}는 {questDic[quest.QuestID]}에 이미 할당 된 QuestID 입니다.");
                    continue;
                }

                // 데이터 id 예외처리
                if (!Manager.Data.QuestDataDic.ContainsKey(quest.QuestID))
                    continue;

                // 퀘스트 생성
                Quest inst = Instantiate(quest, transform);
                QuestData data = Manager.Data.QuestDataDic[quest.QuestID];
                // 퀘스트 데이터 할당
                inst.QuestData = data;

                // 수주 나레이션 할당
                inst.receiveNarrations = new List<NarrtionData>();
                if (Manager.Data.NarrationBundleDic.ContainsKey(data.receiveNarrationBundleID))
                {
                    foreach (int bundleID in Manager.Data.NarrationBundleDic[data.receiveNarrationBundleID])
                    {
                        if (!Manager.Data.NarrationDataDic.ContainsKey(bundleID))
                            break;

                        inst.receiveNarrations.Add(Manager.Data.NarrationDataDic[bundleID]);
                    }
                }

                // 클리어 나레이션 할당
                inst.clearNarrations = new List<NarrtionData>();
                if (Manager.Data.NarrationBundleDic.ContainsKey(data.clearNarrationBundleID))
                {
                    foreach (int bundleID in Manager.Data.NarrationBundleDic[data.clearNarrationBundleID])
                    {
                        if (!Manager.Data.NarrationDataDic.ContainsKey(bundleID))
                            break;

                        inst.clearNarrations.Add(Manager.Data.NarrationDataDic[bundleID]);
                    }
                }

                questDic.Add(quest.QuestID, inst);
            }
        }
        private void LoadQuestData()
        {

        }

        #endregion

        public Quest GetQuest(int id)
        {
            if(!questDic.ContainsKey(id))
            {
                Debug.Log($"{id}에 해당하는 퀘스트 데이터가 없습니다.");
                return null;
            }

            return questDic[id];
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

namespace Jc
{    
    // 퀘스트 상태타입
    //                      {비활성화,      활성화,  진행중, 수락대기, 완료됨}
    public enum QuestState { DisActive = -1, Active = 1, Proceed, Clear, Complete}

    // 퀘스트 타입
    //                    { 기본형, 자동 클리어형, 연계형 } 
    public enum QuestType { Normal = 1, AutoClear, Link}

    public class QuestManager : Singleton<QuestManager>
    {
        [SerializeField]
        private Dictionary<int, Quest> questDic;    // 퀘스트 리소스 (ID 매핑)
        public Dictionary<int, Quest> QuestDic { get { return questDic; } }

        [Header("퀘스트 프리팹")]
        [SerializeField]
        private Quest questPrefab;

        [Header("퀘스트 개수")]
        [SerializeField]
        private int questSize;

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
        }

        /// <summary>
        /// 퀘스트 리소스 등록 및 생성 (데이터 할당)
        /// </summary>
        private void RegistQuest()
        {
            questDic = new Dictionary<int, Quest>();

            for (int i = 1; i<=questSize; i++)
            {
                int questID = i + DataID.QUEST;

                // 딕셔너리 예외처리
                if (questDic.ContainsKey(questID))
                {
                    Debug.Log($"{questID}는 {questDic[questID]}에 이미 할당 된 QuestID 입니다.");
                    continue;
                }

                // 데이터 id 예외처리
                if (!Manager.Data.QuestDataDic.ContainsKey(questID))
                    continue;

                // 퀘스트 생성
                Quest inst = Instantiate(questPrefab, transform);
                QuestData data = Manager.Data.QuestDataDic[questID];
                // 퀘스트 데이터 할당
                inst.QuestID = questID;
                inst.QuestData = data;

                // 최초 퀘스트는 활성화 상태로 변경
                if (inst.QuestID == 1)
                    inst.State = QuestState.Active;

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

                questDic.Add(questID, inst);
            }
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
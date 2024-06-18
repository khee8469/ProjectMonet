using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 퀘스트 ID (데이터 테이블 연동)
    public enum QuestID { }
    
    // 퀘스트 상태타입
    //                      {비활성화,      활성화,  진행중, 수락대기}
    public enum QuestState { DisActive = 0, Active, Proceed, Clear}

    public enum QuestType { Main = 0, Sub}

    public class QuestManager : Singleton<QuestManager>
    {
        [SerializeField]
        private Dictionary<QuestID, Quest> questDic;

        protected override void Awake()
        {
            base.Awake();

            RegistQuest();
        }

        // 퀘스트 등록
        private void RegistQuest()
        {
            questDic = new Dictionary<QuestID, Quest>();
            Quest[] quests = Resources.LoadAll<Quest>($"Quests");

            foreach(Quest quest in quests)
            {
                // id 예외처리
                if(questDic.ContainsKey(quest.QuestID))
                {
                    Debug.Log($"{quest.QuestID}는 {questDic[quest.QuestID]}에 이미 할당 된 QuestID 입니다.");
                    continue;
                }

                // 퀘스트 생성 및 할당
                Quest inst = Instantiate(quest, transform);
                questDic.Add(quest.QuestID, inst);
            }
        }

        public void LoadQuestData()
        {

        }
        public void SaveQuestData()
        {

        }

        public Quest GetQuest(QuestID id)
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
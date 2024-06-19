using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerQuestController : MonoBehaviour
    {
        // 수락한 퀘스트 목록
        [SerializeField]
        private List<Quest> acceptQuests = new List<Quest>(); 
        public List<Quest> AcceptQuests { get { return acceptQuests; } }

        // 퀘스트 엔트리 프리팹
        [SerializeField]
        private QuestEntry questEntryPrefab;
        public QuestEntry QuestEntryPrefab { get { return questEntryPrefab;}}

        // 퀘스트 엔트리 그룹
        [SerializeField]
        private RectTransform questEntryTr;

        // 퀘스트 클리어 시
        public void OnClearQuest(int questID)
        {

        }

        // 퀘스트 수락 시 
        public void OnReceiveQuest(Quest quest)
        {
            acceptQuests.Add(quest);

            // 퀘스트 엔트리 생성
            QuestEntry entry = Instantiate(questEntryPrefab, questEntryTr);
            entry.InitSetting();
        }
    }
}

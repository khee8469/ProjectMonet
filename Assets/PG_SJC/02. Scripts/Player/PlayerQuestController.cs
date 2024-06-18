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

        public void OnClearQuest(QuestID id)
        {

        }

        public void OnGetQuest(Quest quest)
        {
            acceptQuests.Add(quest);
        }
    }
}

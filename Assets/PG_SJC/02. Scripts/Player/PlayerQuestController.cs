using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerQuestController : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<QuestID, Quest> acceptsQuest = new Dictionary<QuestID, Quest>();

        public void ClearQuest(QuestID id)
        {
            if (!acceptsQuest.ContainsKey(id))
                return;

            // 할당 해제
            acceptsQuest.Remove(id);
        }
    }
}

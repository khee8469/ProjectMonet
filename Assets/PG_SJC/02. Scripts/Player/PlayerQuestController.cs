using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerQuestController : MonoBehaviour
    {
        // 퀘스트 엔트리 프리팹
        [SerializeField]
        private QuestEntry questEntryPrefab;
        public QuestEntry QuestEntryPrefab { get { return questEntryPrefab;}}

        // 퀘스트 엔트리 그룹
        [SerializeField]
        private RectTransform questEntryTr;

        // 퀘스트 클리어 콜백
        public void OnClearQuest(Quest quest)
        {
            // 아이템 획득 (추후 매개변수가 아이템으로 변경)

            // 콜백 등록해제
            quest.OnClearQuest -= OnClearQuest;
        }

        // 퀘스트 수락 시 
        public void ReceiveQuest(Quest quest)
        {
            // 퀘스트 엔트리 생성
            QuestEntry entry = Instantiate(questEntryPrefab, questEntryTr);
            entry.OwnerQuest = quest;

            entry.InitSetting();

            // 퀘스트 클리어 콜백 등록
            quest.OnClearQuest += OnClearQuest;
        }
    }
}

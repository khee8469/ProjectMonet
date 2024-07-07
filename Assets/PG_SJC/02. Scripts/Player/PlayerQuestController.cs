using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerQuestController : MonoBehaviour
    {
        private void Start()
        {
            InitSetting();
        }
        private void InitSetting()
        {
            // 퀘스트 데이터 로드 후 엔트리 등록
            foreach(int key in Manager.Quest.QuestDic.Keys)
            {
                if (Manager.Quest.QuestDic[key].State == QuestState.Proceed)
                    ReceiveQuest(Manager.Quest.QuestDic[key]);
            }
        }

        // 퀘스트 클리어 콜백
        public void OnClearQuest(Quest quest)
        {
            // 아이템 획득 (추후 매개변수가 아이템으로 변경)

            // 퀘스트 엔트리 삭제
            Manager.UI.RemoveEntry(quest);
            // 콜백 등록해제
            quest.OnClearQuest -= OnClearQuest;
        }

        // 퀘스트 수락 시 
        public void ReceiveQuest(Quest quest)
        {
            // 퀘스트 엔트리 생성
            Manager.UI.CreateEntry(quest);

            // 퀘스트 클리어 콜백 등록
            quest.OnClearQuest += OnClearQuest;
        }
    }
}

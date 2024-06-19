using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class QuestNormal : Quest
    {
        protected override void Awake()
        {
            base.Awake();
            // 퀘스트 타입 세팅
            questType = QuestType.Normal;
        }

        // 퀘스트 활성화 시
        protected override void OnActiveQuest()
        {

        }

        // 퀘스트 수주 시
        protected override void OnProceedQuest()
        {

        }

        // 퀘스트 클리어 시 (수락 대기)
        protected override void OnClearQuest()
        {

        }

        // 퀘스트 수락 이후 비활성화
        protected override void OnDisActiveQuest()
        {
            ActiveNextQuest();
        }
    }
}
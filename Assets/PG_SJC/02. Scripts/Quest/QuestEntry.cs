using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class QuestEntry : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI npcNameText;
        [SerializeField]
        private TextMeshProUGUI stateText;

        [SerializeField]
        private Quest ownerQuest; 
        public Quest OwnerQuest { get { return ownerQuest; } set { ownerQuest = value; }}

        private string proceedText = "(진행중)";
        private string clearText = "(완료)";

        private void OnEnable()
        {
            UpdateUI();
        }

        public void InitSetting()
        {
            if(ownerQuest == null)
            {
                Debug.Log("엔트리에 해당하는 퀘스트가 할당되지 않았습니다.");
                return;
            }

            // 퀘스트 제목 할당
            titleText.text = ownerQuest.QuestData.questName;
            // NPC 이름 할당
            npcNameText.text = Manager.Data.NPCDataDic[ownerQuest.QuestID].npcName;
        }

        public void UpdateUI()
        {
            // 진행상태 텍스트 업데이트
            switch(ownerQuest.State)
            {
                case QuestState.Proceed:
                    stateText.text = proceedText;
                    break;
                case QuestState.Clear:
                    stateText.text = clearText;
                    break;
            }
        }

        public void OnClickQuestButton()
        {

        }

    }
}
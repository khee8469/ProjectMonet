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
        private TextMeshProUGUI typeText;
        [SerializeField]
        private TextMeshProUGUI npcNameText;

        [SerializeField]
        private Quest ownerQuest; 
        public Quest OwnerQuest { get { return ownerQuest; } set { ownerQuest = value; }}

        private void OnEnable()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {

        }

        public void OnClickQuestButton()
        {

        }

    }
}
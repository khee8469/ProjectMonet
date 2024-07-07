using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Jc
{
    public class NPC : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]
        protected int id;
        public int ID { get { return id; } }

        [SerializeField]
        protected NPCData npcData;
        public NPCData NPCData { get { return npcData; } }

        [SerializeField]
        protected List<int> questIDList = new List<int>();
        public List<int> QuestIDList { get { return questIDList; } }

        [SerializeField]
        protected List<int> basicNarrations = new List<int>();       // 기본 나레이션 ID
        public List<int> BasicNarrations {get { return basicNarrations; } }

        [SerializeField]
        protected TextMeshProUGUI dialogText;     // 다이얼로그 텍스트
        [SerializeField]
        protected Animator floatingAnim;      // 다이얼로그 텍스트 플로팅 애니메이터

        [Space(5)]
        [Header("밸런싱")]
        [Space(5)]
        [SerializeField]
        protected bool isInteracted = true;    // 상호작용 여부
        public bool IsInteracted { get { return isInteracted; } }

        [SerializeField]
        protected Quest currentQuest;

        [SerializeField]
        private int basicNarrationIndex = 0;     // 기본 나레이션 인덱스

        [SerializeField]
        private int curQuestDialogIndex = 0;     // 퀘스트 대화 진행 인덱스
        [SerializeField]
        private int curBasicDialogIndex = 0;     // 기본 대화 진행 인덱스 

        protected virtual void Start()
        {
            LoadData();
        }

        /// <summary>
        /// NPC 데이터 로드
        /// </summary>
        private void LoadData()
        {
            // NPC 데이터 할당
            if (!Manager.Data.NPCDataDic.ContainsKey(id))
            {
                Debug.Log($"{id}에 해당하는 NPC 데이터가 존재하지 않습니다.");
                return;
            }

            npcData = Manager.Data.NPCDataDic[id];

            // 데이터에 따른 퀘스트 할당
            questIDList = npcData.questIDList;

            // 기본 나레이션 리스트 할당
            if (npcData.narrationBundleID == null || npcData.narrationBundleID.Count < 1
                || !Manager.Data.NarrationBundleDic.ContainsKey(npcData.narrationBundleID[basicNarrationIndex])
                || Manager.Data.NarrationBundleDic[npcData.narrationBundleID[basicNarrationIndex]].Count < 1)
            {
                Debug.Log($"{id}NPC의 {basicNarrationIndex}번째 기본 대사가 존재하지 않습니다.");
                return;
            }
            basicNarrations = Manager.Data.NarrationBundleDic[npcData.narrationBundleID[basicNarrationIndex]];
        }

        // 퍼즐/퀘스트를 통한 기본 나레이션 변경
        public void NextBasicNarration()
        {
            basicNarrationIndex++;
            if(!Manager.Data.NarrationBundleDic.ContainsKey(basicNarrationIndex) 
                || Manager.Data.NarrationBundleDic[basicNarrationIndex].Count < 1)
            {
                Debug.Log($"{id}NPC의 {basicNarrationIndex}번째 기본 대사가 존재하지 않습니다.");
                return;
            }
            basicNarrations = Manager.Data.NarrationBundleDic[npcData.narrationBundleID[basicNarrationIndex]];
        }

        // 상호작용 시 
        public virtual bool OnInteract(PlayerQuestController questController)
        {
            // 최초 상호작용 처리
            // 현재 진행할 퀘스트 할당
            currentQuest = GetQuest();
      
            // 진행할 퀘스트가 없다면 기본 대사, 특수 대사 출력
            UpdateDialog(questController);
            return true;
        }
        // 상호작용 도중 이탈 시
        public virtual void OnExitInteract()
        {
            dialogText.enabled = false;

            // 다이얼로그 인덱스 수정
            curQuestDialogIndex = 0;
            curBasicDialogIndex = 0;
        }

        // 활성화되어있는 퀘스트 반환
        protected Quest GetQuest()
        {
            foreach (int id in questIDList)
            {
                QuestState state = Manager.Quest.GetQuest(id).State;

                // 비활성화 상태가 아닌 퀘스트를 반환
                if (state != QuestState.DisActive
                    && state != QuestState.Complete)
                    return Manager.Quest.GetQuest(id);
            }
            return null;
        }

        protected virtual void UpdateDialog(PlayerQuestController questController)
        {
            dialogText.enabled = true;

            // 현재 할당된 퀘스트가 없는 경우
            if(currentQuest == null)
            {
                // 플로팅 애니메이션
                floatingAnim.SetTrigger(Manager.Param.OnFloating);

                if (curBasicDialogIndex >= basicNarrations.Count)
                    dialogText.text = currentQuest.receiveNarrations[basicNarrations.Count-1].text;
                else
                    dialogText.text = currentQuest.receiveNarrations[curBasicDialogIndex++].text;
                return;
            }

            // 할당된 퀘스트가 있는 경우
            // 퀘스트 상태에 따른 대화 출력
            switch (currentQuest.State)
            {
                // 퀘스트 수주
                case QuestState.Active:
                    // 대화 종료 체크
                    if (curQuestDialogIndex >= currentQuest.receiveNarrations.Count)
                    {
                        dialogText.enabled = false;
                        // 최초 등록 (수주 시에만 최초로 등록)
                        // 플레이어에 퀘스트 등록
                        questController.ReceiveQuest(currentQuest);
                        // 퀘스트 진행중 상태로 변경
                        currentQuest.ChangeState(QuestState.Proceed);
                        return;
                    }
                    // 플로팅 애니메이션
                    floatingAnim.SetTrigger(Manager.Param.OnFloating);
                    // 대화 진행
                    dialogText.text = currentQuest.receiveNarrations[curQuestDialogIndex++].text;
                    break;
                // 퀘스트 진행중
                case QuestState.Proceed:
                    // 플로팅 애니메이션
                    floatingAnim.SetTrigger(Manager.Param.OnFloating);
                    dialogText.text = currentQuest.receiveNarrations[currentQuest.receiveNarrations.Count - 1].text;
                    break;
                // 퀘스트 완료
                case QuestState.Clear:
                    // 대화 종료 체크
                    if (curQuestDialogIndex >= currentQuest.clearNarrations.Count)
                    {
                        dialogText.enabled = false;
                        // 퀘스트 완료 상태로 변경
                        currentQuest.ChangeState(QuestState.Complete);
                        // 리워드 지급은 퀘스트 자체에서 진행
                        // NPC 상태 변경
                        return;
                    }
                    // 대화 진행
                    // 플로팅 애니메이션
                    floatingAnim.SetTrigger(Manager.Param.OnFloating);
                    dialogText.text = currentQuest.clearNarrations[curQuestDialogIndex++].text;
                    break;
                default:
                    break;
            }
        }
    }
}


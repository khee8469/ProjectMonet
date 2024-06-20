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
    public abstract class NPC : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]    
        protected int id;
        public int ID { get { return id; } }

        [SerializeField]
        protected NPCData npcData;
        public NPCData NPCData { get { return npcData; } }

        [SerializeField]
        private TextMeshProUGUI debugNameText;

        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        protected Animator anim;
        public Animator Anim { get { return anim; } }

        [SerializeField]
        protected List<int> questIDList = new List<int>();
        public List<int> QuestIDList { get { return questIDList; } }

        [SerializeField]
        protected float moveSpeed;  // 이동속도 설정
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set
            {
                moveSpeed = value;
                agent.speed = value;
            }
        }

        [SerializeField]
        private float idleTime;     // 대기시간 설정
        public float IdleTime { get { return idleTime; } }

        [SerializeField]
        private TextMeshProUGUI dialogText;     // 다이얼로그 텍스트

        protected StateMachine<NPC, NPCStateType> fsm;
        public StateMachine<NPC, NPCStateType> FSM { get { return fsm; } }

        [SerializeField]
        private string basicDialog = "NULL";

        [Space(5)]
        [Header("밸런싱")]
        [Space(5)]
        [SerializeField]
        protected NPCStateType curState;        // 현재 상태

        [SerializeField]
        protected bool isInteracted = true;    // 상호작용 여부
        public bool IsInteracted { get { return isInteracted; } }

        public Vector3 playerPos;       // 상호작용 한 플레이어 위치

        [SerializeField]
        private Quest currentQuest;

        [SerializeField]
        private int curDialogIndex = 0;     // 대화 진행 인덱스
        [SerializeField]
        private int maxDialogIndex = 0;     // 대화 진행 최대 인덱스

        private void Start()
        {
            LoadData();
        }

        /// <summary>
        /// NPC 데이터 로드
        /// </summary>
        private void LoadData()
        {
            // NPC 데이터 할당
            if (!Manager.Quest.NPCDataDic.ContainsKey(id))
            {
                Debug.Log($"{id}에 해당하는 NPC 데이터가 존재하지 않습니다.");
                return;
            }
            npcData = Manager.Quest.NPCDataDic[id];

            // 데이터에 따른 퀘스트 할당
            questIDList = npcData.questIDList;
        }

        // 목적지 계산
        public abstract Vector3 CalculateDestination();
        // 상호작용 시 
        public virtual void OnInteract(PlayerQuestController questController)
        {
            // 최초 상호작용 처리
            if (fsm.CurState != NPCStateType.Interact)
            {
                // 현재 진행할 퀘스트 할당
                if (GetQuest() != null)
                    currentQuest = GetQuest();
                // 진행할 퀘스트가 없다면 일정시간 딜레이 후 다시 순찰루틴 진행
                else
                {
                    StartCoroutine(Extension.ActionDelay(5.0f, () => dialogText.enabled = false));
                    StartCoroutine(Extension.ActionDelay(5.5f, () => fsm.ChangeState(NPCStateType.Patrol)));
                }
                // 상호작용 상태로 전이
                fsm.ChangeState(NPCStateType.Interact);
                anim.SetBool(Manager.Param.IsInteract, true);
            }

            UpdateDialog(questController);
        }

        // 상호작용 도중 이탈 시
        public void OnExitInteract()
        {
            if (fsm.CurState == NPCStateType.Interact)
            {
                // 다이얼로그 인덱스 수정
                curDialogIndex = 0;
                fsm.ChangeState(NPCStateType.Patrol);
            }
        }

        private Quest GetQuest()
        {
            foreach (int id in questIDList)
            {
                // 비활성화 상태가 아닌 퀘스트를 반환
                if (Manager.Quest.GetQuest(id).State != QuestState.DisActive)
                    return Manager.Quest.GetQuest(id);
            }
            return null;
        }

        private void UpdateDialog(PlayerQuestController questController)
        {
            dialogText.gameObject.SetActive(true);

            // 현재 할당중인 퀘스트가 없는 경우
            if (currentQuest == null)
            {
                dialogText.text = basicDialog;
                return;
            }

            // 할당된 퀘스트가 있는 경우
            // 퀘스트 상태에 따른 대화 출력
            switch (currentQuest.State)
            {
                // 퀘스트 수주
                case QuestState.Active:
                    // 대화 종료 체크
                    if (curDialogIndex >= currentQuest.receiveNarrations.Count)
                    {
                        dialogText.enabled = false;
                        // 최초 등록 (수주 시에만 최초로 등록)
                        // 플레이어에 퀘스트 등록
                        questController.ReceiveQuest(currentQuest);
                        // 퀘스트 진행중 상태로 변경
                        currentQuest.ChangeState(QuestState.Proceed);
                        // NPC 상태 변경
                        fsm.ChangeState(NPCStateType.Patrol);
                        anim.SetBool(Manager.Param.IsInteract, false);
                        return;
                    }
                    // 대화 진행
                    dialogText.text = currentQuest.receiveNarrations[curDialogIndex++].text;
                    break;
                // 퀘스트 진행중
                case QuestState.Proceed:
                    dialogText.text = currentQuest.receiveNarrations[currentQuest.receiveNarrations.Count - 1].text;
                    break;
                // 퀘스트 완료
                case QuestState.Clear:
                    // 대화 종료 체크
                    if (curDialogIndex >= currentQuest.clearNarrations.Count)
                    {
                        dialogText.enabled = false;
                        // 퀘스트 비활성화 상태로 변경
                        currentQuest.ChangeState(QuestState.DisActive);
                        // 리워드 지급은 퀘스트 자체에서 진행
                        // NPC 상태 변경
                        fsm.ChangeState(NPCStateType.Patrol);
                        anim.SetBool(Manager.Param.IsInteract, false);
                        return;
                    }
                    // 대화 진행
                    dialogText.text = currentQuest.clearNarrations[curDialogIndex++].text;
                    break;
                default:
                    dialogText.text = basicDialog;
                    break;
            }

            anim.SetTrigger(Manager.Param.OnInteract);
            // 플레이어 방향으로 전환
            Vector3 dir = (questController.transform.position - transform.position).normalized;
            transform.forward = dir;
            dialogText.enabled = true;
        }



        protected abstract void OnDrawGizmosSelected();

        protected virtual void Update()
        {
            if (fsm.CurState != curState)
                curState = fsm.CurState;

            fsm.Update();
        }
        protected virtual void LateUpdate()
        {
            fsm.LateUpdate();
        }
        protected virtual void FixedUpdate()
        {
            fsm.FixedUpdate();
        }
    }
}
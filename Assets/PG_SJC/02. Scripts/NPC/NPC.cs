using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Jc
{
    public abstract class NPC : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField]
        protected string npcName;
        public string NpcName { get { return npcName; } }

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
        private int curDialogIndex = -1;     // 대화 진행 인덱스
        [SerializeField]
        private int maxDialogIndex = 0;     // 대화 진행 최대 인덱스

        private void Awake()
        {
            debugNameText.text = npcName;
        }

        // 목적지 계산
        public abstract Vector3 CalculateDestination();
        // 상호작용 시 
        public virtual void OnInteract(Vector3 targetPos)
        {
            // 최초 상호작용 처리
            if (fsm.CurState != NPCStateType.Interact)
            {
                // 현재 진행할 퀘스트 할당
                if (currentQuest != GetQuest())
                    currentQuest = GetQuest();
                // 진행할 퀘스트가 없다면 일정시간 딜레이 후 다시 순찰루틴 진행
                else
                    StartCoroutine(Extension.ActionDelay(5.5f, () => fsm.ChangeState(NPCStateType.Patrol)));
                // 상호작용 상태로 전이
                fsm.ChangeState(NPCStateType.Interact);
            }

            UpdateDialog();
            anim.SetTrigger(Manager.Param.OnInteract);
            // 플레이어 방향으로 전환
            Vector3 dir = (targetPos - transform.position).normalized;
            transform.forward = dir;
        }

        // 상호작용 도중 이탈 시
        public void OnExitInteract()
        {
            if (fsm.CurState == NPCStateType.Interact)
            {
                // 다이얼로그 인덱스 수정
                curDialogIndex = -1;
                fsm.ChangeState(NPCStateType.Patrol);
            }
        }

        private Quest GetQuest()
        {
            foreach (int id in questIDList)
            {
                if (Manager.Quest.GetQuest(id).State == QuestState.Active)
                    return Manager.Quest.GetQuest(id);
            }
            return null;
        }

        private void UpdateDialog()
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
                case QuestState.Active:

                    break;
                case QuestState.Clear:

                    break;
                default:
                    dialogText.text = basicDialog;
                    break;
            }
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
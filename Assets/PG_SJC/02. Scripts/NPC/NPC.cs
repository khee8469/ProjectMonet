using System.Collections;
using System.Collections.Generic;
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
        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } } 

        [SerializeField]
        protected Animator anim;
        public Animator Anim { get { return anim; } }

        [SerializeField]
        protected Quest npcQuest;
        public Quest NPCQuest { get { return npcQuest; } }  

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

        protected StateMachine<NPC, NPCStateType> fsm;
        public StateMachine<NPC, NPCStateType> FSM { get { return fsm; } }

        [Space(5)]
        [Header("밸런싱")]
        [Space(5)]
        [SerializeField]
        protected NPCStateType curState;        // 현재 상태

        [SerializeField]
        protected bool isInteracted = true;    // 상호작용 여부
        public bool IsInteracted { get { return isInteracted; }}

        public Vector3 playerPos;       // 상호작용 한 플레이어 위치
       

        // 목적지 계산
        public abstract Vector3 CalculateDestination();
        // 상호작용 시 
        public virtual void OnInteract(Vector3 targetPos)
        {
            // 추후 조건추가 : NPC가 상호작용할 수 있는 상태인지?
            // 해당 조건에 따른 다른 상호작용 적용 예정

            playerPos = targetPos;
            // 상호작용 상태로 전이
            fsm.ChangeState(NPCStateType.Interact);
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
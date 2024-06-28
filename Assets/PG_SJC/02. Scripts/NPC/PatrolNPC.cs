using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jc.NPCStates
{
    public class PatrolNPC : NPC
    {
        [Space(5)]
        [Header("순찰 스펙")]
        [SerializeField]
        private Transform startTr;   // 순찰 시작지점
        public Transform StartTr { get { return startTr; }}

        [SerializeField]
        private Transform endTr;     // 순찰 종료지점
        public Transform EndTr { get { return endTr; }}

        private void Awake()
        {
            agent.speed = moveSpeed;

            // 상태머신 셋업
            fsm = new StateMachine<NPC, NPCStateType>(this);
            fsm.AddState(NPCStateType.Idle, new Idle(this));            // 대기상태
            fsm.AddState(NPCStateType.Patrol, new Patrol(this));        // 순찰상태
            fsm.AddState(NPCStateType.Interact, new Interact(this));    // 상호작용 상태
            fsm.Init(NPCStateType.Idle);
        }

        // 애니메이터 초기세팅 (파라미터 아이디 캐싱)
        private void AnimInit()
        {
            //animID_isMoving = Animator.StringToHash("isMoving");
        }

        // 목적지 계산
        public override Vector3 CalculateDestination()
        {
            // Patrol 행동
            // 정해진 시작 지점과 도착 지점을 왕복
            Vector3 startPos = startTr.transform.position;
            Vector3 endPos = endTr.transform.position;

            if ((transform.position - startPos).sqrMagnitude < 1f)        // 시작지점에서 출발할 경우
                return endPos;
            else if ((transform.position - endPos).sqrMagnitude < 1f)     // 도착지점에서 출발할 경우
                return startPos;
            else                                                            // 아닐경우 출발지점으로 다시 이동
                return startPos;        
        }
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(startTr.transform.position +Vector3.up, Vector3.one);
            Gizmos.DrawLine(startTr.transform.position + Vector3.up, endTr.transform.position + Vector3.up);
            Gizmos.DrawWireCube(endTr.transform.position + Vector3.up, Vector3.one);
        }
    }
}

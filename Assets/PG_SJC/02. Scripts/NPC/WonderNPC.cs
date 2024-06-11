using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jc.NPCStates
{
    public class WonderNPC : NPC
    {
        [Space(5)]
        [Header("배회 스펙")]
        [SerializeField]
        private float maxRadius;     // 배회 최대거리 설정 (원점 기준)
        public float MaxRadius { get { return maxRadius; } }

        private Vector3 originPos;   // 배회 중심 설정
        public Vector3 OriginPos { get { return originPos; } }

        private void Awake()
        {
            originPos = transform.position;
            agent.speed = moveSpeed;

            // 상태머신 셋업
            fsm = new StateMachine<NPC, NPCStateType>(this);
            fsm.AddState(NPCStateType.Idle, new Idle(this));            // 대기상태
            fsm.AddState(NPCStateType.Patrol, new Patrol(this));        // 순찰상태
            fsm.AddState(NPCStateType.Interact, new Interact(this));    // 상호작용 상태
            fsm.Init(NPCStateType.Idle);
        }
        public override Vector3 CalculateDestination()
        {
            // maxRadius의 반지름을 가지는 원 내부의 임의의 점을 도출
            Vector2 randPos = Random.insideUnitCircle * maxRadius;
            // 에이전트의 최초 위치에 더해 목적지 세팅
            return originPos + new Vector3(randPos.x, 0, randPos.y);
        }
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(originPos, maxRadius);
        }
    }
}

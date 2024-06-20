using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Jc
{
    public enum NPCStateType { Idle = 0, Patrol, Interact }

    public class NPCBaseState : BaseState
    {
        protected NPC baseOwner;
    }

    namespace NPCStates
    {
        // 대기
        public class Idle : NPCBaseState
        {
            private Coroutine idleRoutine;
            public Idle(NPC owner)
            {
                baseOwner = owner;
            }

            public override void Enter()
            {
                baseOwner.Agent.isStopped = true;

                // 일정시간 딜레이 후 순찰 루틴으로 변경 
                idleRoutine = baseOwner.StartCoroutine(
                    Extension.ActionDelay(
                        baseOwner.IdleTime
                        ,
                        () => baseOwner.FSM.ChangeState(NPCStateType.Patrol)));
            }
            public override void Exit()
            {
                if (idleRoutine != null)
                {
                    baseOwner.StopCoroutine(idleRoutine);
                    idleRoutine = null;
                }
            }
        }
        // 순찰
        public class Patrol : NPCBaseState
        {
            private Vector3 curDestination;     // 설정된 목적지
            private Coroutine checkRoutine;     // 도착지점 확인 코루틴
            public Patrol(NPC owner)
            {
                baseOwner = owner;
            }

            public override void Enter()
            {
                baseOwner.Agent.isStopped = false;
                // 해당 지점으로 이동
                curDestination = baseOwner.CalculateDestination();
                baseOwner.Agent.SetDestination(curDestination);

                // 목적지 확인루틴 실행
                checkRoutine = baseOwner.StartCoroutine(CheckArrivalRoutine());
            }
            public override void LateUpdate()
            {
                baseOwner.Anim.SetFloat(Manager.Param.MoveSpeed, baseOwner.Agent.velocity.sqrMagnitude);
            }
            public override void Exit()
            {
                if (checkRoutine != null)
                {
                    baseOwner.StopCoroutine(checkRoutine);
                    checkRoutine = null;
                }

                curDestination = baseOwner.transform.position;
                baseOwner.Anim.SetFloat(Manager.Param.MoveSpeed, 0f);
            }

            IEnumerator CheckArrivalRoutine()
            {
                while (!CheckArrival())
                {
                    yield return new WaitForSeconds(1f);
                }

                checkRoutine = null;
                // 대기상태로 전환
                baseOwner.FSM.ChangeState(NPCStateType.Idle);
                yield return null;
            }

            private bool CheckArrival()
            {
                // 목적지 확인
                return (baseOwner.transform.position - curDestination).sqrMagnitude < 1f;
            }
        }
        // 상호작용
        public class Interact : NPCBaseState
        {
            public Interact(NPC owner)
            {
                baseOwner = owner;
            }

            public override void Enter()
            {
                baseOwner.Agent.isStopped = true;
            }
        }
    }
}
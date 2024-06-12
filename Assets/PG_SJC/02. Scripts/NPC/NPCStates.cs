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
            private Coroutine interactRoutine;
            public Interact(NPC owner)
            {
                baseOwner = owner;
            }

            public override void Enter()
            {
                baseOwner.Agent.isStopped = true;
                interactRoutine = baseOwner.StartCoroutine(InteractRoutine());
            }

            public override void Exit()
            {
                baseOwner.Anim.SetBool(Manager.Param.IsInteract, false);

                if (interactRoutine != null)
                {
                    baseOwner.StopCoroutine(interactRoutine);
                    interactRoutine = null;
                }
            }

            IEnumerator InteractRoutine()
            {
                //yield return RotateRoutine();
                baseOwner.Anim.SetBool(Manager.Param.IsInteract, true);
                baseOwner.transform.forward = (baseOwner.playerPos - baseOwner.transform.position).normalized;
                // 테스트 모드 : 일정시간 뒤 순찰상태로 전이
                yield return new WaitForSeconds(5f);
                interactRoutine = null;
                baseOwner.FSM.ChangeState(NPCStateType.Patrol);
            }

            // 플레이어 위치로 자연스러운 회전
            IEnumerator RotateRoutine()
            {
                float rate = 0f;
                float rotTime = 2f;
                Quaternion startRot = baseOwner.transform.rotation;
                Quaternion endRot = Quaternion.Euler((baseOwner.playerPos - baseOwner.transform.position).normalized);

                while (rate < 1f)
                {
                    baseOwner.transform.rotation = Quaternion.Lerp(startRot, endRot, rate);
                    rate += Time.deltaTime * rotTime;
                    yield return null;
                }

                baseOwner.transform.rotation = endRot;
            }
        }
    }
}
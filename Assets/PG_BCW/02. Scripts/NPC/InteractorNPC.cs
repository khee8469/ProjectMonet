using Jc;
using Jc.NPCStates;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

public class InteractorNPC : NPC
{
    private void Awake()
    {
        // 상태머신 셋업
        fsm = new StateMachine<NPC, NPCStateType>(this);
        fsm.AddState(NPCStateType.Idle, new Idle(this));            // 대기상태
        fsm.AddState(NPCStateType.Patrol, new Patrol(this));        // 순찰상태
        fsm.AddState(NPCStateType.Interact, new Interact(this));    // 상호작용 상태
        fsm.Init(NPCStateType.Idle);

        
    }



    public void OnTalkInteractor()
    {
        //완료 전 대사, 완료 후 대사
        /*if( 미완료)
        else if(완료)*/
        
    //}

    public override Vector3 CalculateDestination()
    {
        return transform.position;
    }

    protected override void OnDrawGizmosSelected()
    {
        //throw new System.NotImplementedException();
    }
}

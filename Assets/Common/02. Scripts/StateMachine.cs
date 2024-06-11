using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TOwner, TState>
{
    // 상태머신 소유자
    private TOwner owner;
    public TOwner Owner { get { return owner; } }

    // 현재 상태
    private TState curState;
    public TState CurState { get { return curState; } }

    // 딕셔너리를 통한 상태 검색
    private Dictionary<TState, BaseState> stateDic;
    public Dictionary<TState, BaseState> StateDic { get { return stateDic; } }

    public StateMachine(TOwner owner)
    {
        this.owner = owner;
        stateDic = new Dictionary<TState, BaseState>();
    }

    public void AddState(TState key, BaseState value)
    {
        stateDic.Add(key, value);
    }

    // 초기 상태 지정
    public void Init(TState entry)
    {
        curState = entry;
        stateDic[entry].Enter();
    }

    // 상태 전이
    public void ChangeState(TState nextState)
    {
        // 현재 상태 탈출 -> 다음상태 진입
        // 이전 상태의 Exit이 먼저 호출되고 다음 상태의 Enter호출
        stateDic[curState].Exit();
        curState = nextState;
        stateDic[curState].Enter();
    }

    public void Update()
    {
        stateDic[curState].Update();
    }
    public void LateUpdate()
    {
        stateDic[curState].LateUpdate();
    }
    public void FixedUpdate()
    {
        stateDic[curState].FixedUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 파라미터 ID 세팅, 캐싱
/// </summary>
public class AnimParamManager : Singleton<AnimParamManager>
{
    private int id_MoveSpeed;
    public int MoveSpeed {get { return id_MoveSpeed; } }

    private int id_IsMoving;
    public int IsMoving{get { return id_IsMoving; } }

    protected override void Awake()
    {
        base.Awake();
        InitParameters();
    }

    private void InitParameters()
    {
        id_MoveSpeed = Animator.StringToHash("MoveSpeed");
    }

}

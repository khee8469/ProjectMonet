using Jc;
using JJH;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPatternController : MonoBehaviour, IPuzzleable
{
    // 원반 한 세트를 총 관리해 줄 controller 

    [Header("원반 관리")]
    [SerializeField] private List<HiddenPatternObject> patterns; // 1-1 은 어차피 못움직이는 0 0 0 이므로 관리 할 필요없음.

    [Tooltip("자신의 ID ")]
    [SerializeField] int controllerID;

    [Tooltip("퍼즐 매니저 가져오기.")]
    [SerializeField] HiddenPatternPuzzle puzzle;

    [Tooltip("약간 보정값 둬서 정답 맞추기 쉽게 하기")]
    [SerializeField] private float tolerance = 1f; //이게 1이면 1도일듯?

    [Tooltip("오브젝트 들의 정답 targetRotation")]
    [SerializeField] private Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

    [Tooltip("자신의 update 해 줄 퍼즐인덱스")]
    [SerializeField] private int puzzleIndex; // 0 부터 시작 


    // 여기서 각 원반 list의 rotation 값 체크하고 추가로 update 돌리고 
    // 완료되면 자기 자식들 콜라이더 꺼주고 상태 저장해주고 등등 필요함. 

    // 정답체크를 만약에 각 오브젝트들이 같은 그림이니까 같은 localRotation 이라면?
    // but 그냥 0 0 0 이 정상적이기는 할 듯? 

    private void Awake()
    {
        RegistObject(puzzle); // 퍼즐 등록 
    }

    private void Start()
    {

    }


    public void CheckRotation()
    {
        // 자기의 자식들의 localRotaion이 0 0 0 과 가까우면 -- > update 해주기.
        // 해주고 콜라이더 끄기 
        Debug.Log("로테이션 값 체크");

        bool isAllTrue = true;
        foreach (var pattern in patterns)  // xr knob의 handle의 transform을 가져와야 한다. 
        {
            if(Quaternion.Angle(pattern.handle.transform.localRotation, targetRotation) < Mathf.Abs( tolerance))
            {
                Debug.Log($"쿼터니언 앵글값->{Quaternion.Angle(pattern.gameObject.transform.localRotation, targetRotation)}");
            }
            else //각도가 맞지 않아서 break 되면
            {
                isAllTrue = false;
                Debug.Log($"쿼터니언 앵글값->{Quaternion.Angle(pattern.handle.transform.localRotation, targetRotation)}, " +
                    $"아직 패턴이 일치 하지 않음.");
                break;
            }
        }

        Debug.Log($"지금 bool 값의 값 ->{isAllTrue}");

        if(isAllTrue) // 모두 값이 자신의 안 이니까.
        {
            puzzle.UpdateCondition(puzzleIndex);
        }
    }


    private void ColliderOff()
    {
        foreach(var pattern  in patterns)
        {
            pattern.collider.enabled = false;  // 자신의 모든 콜라이더를 꺼준다.
        }
    }


    public void RegistObject(PuzzleManager puzzle)
    {
        puzzle.puzzleObjects.Add(this);
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
    {
        puzzle.UpdateCondition(index); //일단 기본이 true 인데 어차피 완성되서 날라올 거니까 그냥 true로 하면 된다.
        ColliderOff(); // 자신의 모든 자식들의 콜라이더를 off 해서 건들지 못하도록 한다. 
    }

    public void ActiveSetting()
    {

    }

    public void DisActiveSetting()
    {

    }

    public void CompleteSetting()
    {
        // 자기 자식 콜라이더 다 꺼줘서 못 만지게 하기. or 스크립트를 꺼버리기

        foreach (var pattern in patterns)
        {
            pattern.gameObject.layer = 0; // 빛을 받을 수 있도록 Defalut layer로 바꿔주기.
            pattern.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0); // 모든 오브젝트를 0 0 0 으로 변경
        }

        puzzle.UpdateCondition(puzzleIndex);

    }







}

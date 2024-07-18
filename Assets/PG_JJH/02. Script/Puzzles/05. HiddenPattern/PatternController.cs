using Jc;
using JJH;
using System;
using System.Collections;
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

    [Tooltip("정답 보정값")]
    [SerializeField] private float tolerance = 10f; //이게 1이면 1도일듯?

    [Tooltip("오브젝트 들의 정답 targetRotation")]
    [SerializeField] private Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

    [Tooltip("자신의 update 해 줄 퍼즐인덱스")]
    [SerializeField] private int puzzleIndex; // 0 부터 시작 

    private bool isChangingValue = false;

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
        if (isChangingValue) return;

        isChangingValue = true;

        bool isAllTrue = true;

        foreach (var pattern in patterns)  // xr knob의 handle의 transform을 가져와야 한다. 
        {
            float y = pattern.handle.transform.localRotation.eulerAngles.y;
            Debug.Log($"y의 값 ->{y}");
            if ((y >= 0 && y <= 0 + tolerance) || (y <= 360 && y >= 360 - tolerance)) // 보정값 나중에 수정하기.
            {
                Debug.Log("정답을 맞췄다.");
            }
            else
            {
                isAllTrue = false;
            }
        }

        Debug.Log($"지금 bool 값의 값 ->{isAllTrue}");

        if (isAllTrue) // 모두 값이 자신의 안 이니까. --> 그니까 결국 모두 정답이면 여기서 update해주고.
        {
            puzzle.UpdateCondition(puzzleIndex); // bool 값 업데이트 
            UpdatePattern();
            
        }

        isChangingValue = false;
    }

    private void UpdatePattern()
    {
        foreach (var pattern in patterns)
        {
            pattern.collider.enabled = false;  // 자신의 모든 콜라이더를 꺼준다.
            pattern.interactionLayers = 0; // 0이 아마 nothing임. 

            StartCoroutine(IdentityRoutine(pattern));
        }
    }


    float duration = 1f; 
    private IEnumerator IdentityRoutine(HiddenPatternObject pattern) // foreach 의 pattern을 받음. 
    {

        Quaternion startRotation = pattern.handle.transform.localRotation;
        Quaternion endRotation = Quaternion.identity;

        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            pattern.handle.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            yield return null;
        }
        pattern.handle.transform.localRotation = endRotation;

    }



    public void RegistObject(PuzzleManager puzzle)
    {
        Debug.Log($"{controllerID} : 컨트롤러 등록");
        puzzle.puzzleObjects.Add(this);
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
    {
        puzzle.UpdateCondition(index); //일단 기본이 true 인데 어차피 완성되서 날라올 거니까 그냥 true로 하면 된다.
        
    }

    public void ActiveSetting()
    {
        // 문을 안열면 어차피 못 들어온다. 기본 상태가 만질 수 있는 상태. 
    }

    public void DisActiveSetting()
    {

    }

    public void CompleteSetting()
    {
        // 자기 자식 콜라이더 다 꺼줘서 못 만지게 하기. or 스크립트를 꺼버리기

        foreach (HiddenPatternObject pattern in patterns)
        {
            Debug.Log($"{pattern.PatternID} : 컴플릿");
            pattern.handle.gameObject.layer = 0; // 빛을 받을 수 있도록 Defalut layer로 바꿔주기. 바꿔 줄 필요 있나?
            pattern.interactionLayers = 0 ; //nothing 으로 못만지도록  
            pattern.handle.transform.localRotation = Quaternion.identity;   // 0 0 0 으로 초기화. 
        }
        UpdatePuzzleManager(puzzle, puzzleIndex);
    }
}

using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("퍼즐 클리어 조건 번호")]
    [SerializeField]
    int puzzleIndex;


    
    


    public void ActiveSetting()
    {
        throw new System.NotImplementedException();
    }

    public void CompleteSetting()
    {
        throw new System.NotImplementedException();
    }

    public void DisActiveSetting()
    {
        throw new System.NotImplementedException();
    }

    public void RegistObject(PuzzleManager puzzle)
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
    {
        throw new System.NotImplementedException();
    }
}

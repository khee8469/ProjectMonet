using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClockPuzzle : InteractObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("퍼즐 클리어 조건 번호")]
    [SerializeField]
    int puzzleIndex;
    [Tooltip("boxcollider")]
    [SerializeField]
    Collider clockCollider;


    [Tooltip("minuteCollider")]
    [SerializeField]
    Collider minuteCollider;



    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        
    }



    public void RegistObject(PuzzleManager puzzle)
    {
        puzzle.puzzleObjects.Add(this);
    }
    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        puzzle.UpdateCondition(index);
    }
    public void ActiveSetting()
    {
        clockCollider.enabled = true;
    }
    public void DisActiveSetting()
    {
        clockCollider.enabled = false;
    }
    public void CompleteSetting()
    {
        if (puzzleManager != null)
        {
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
        }
        Destroy(gameObject);
    }
}

using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WateringCan : ItemObject
{
    [Tooltip("완료를 확인할 퀘스트 ID")]
    [SerializeField]
    int flowerQuestID;

    protected override void OnEnable()
    {
        base.OnEnable();

        //퀘스트 완료시 제거
        if (Manager.PlayableData.puzzleDataDic[flowerQuestID] == PuzzleState.Clear)
        {
            gameObject.SetActive(false);
        }
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //퀘스트 완료시 제거
        if (Manager.PlayableData.puzzleDataDic[flowerQuestID] == PuzzleState.Clear)
        {
            gameObject.SetActive(false);
        }
    }

    /*public void GroundCheck()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 100, raycastPoint))
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
        // hit.point 
        else
        {
            transform.position = hit.point;
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
            if (umblleraCollider != null) umblleraCollider.enabled = false;
        }
    }*/

}
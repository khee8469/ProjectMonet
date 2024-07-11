using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WateringCan : ItemObject
{
    [Header("현재 오브젝트 정보")]
    [Tooltip("지정된 소켓 위치")]
    [SerializeField]
    Transform specifiedSocket;

    [Tooltip("완료를 확인할 퀘스트 ID")]
    [SerializeField]
    int flowerQuestID;


    protected override void OnEnable()
    {
        base.OnEnable();

        /*if (Manager.PlayableData.puzzleDataDic[flowerQuestID] == PuzzleState.Clear)
        {
            gameObject.SetActive(false);
        }*/
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //Debug.Log(Manager.PlayableData.puzzleDataDic[flowerQuestID]);
        /*if (Manager.PlayableData.puzzleDataDic[flowerQuestID] == PuzzleState.Clear)
        {
            gameObject.SetActive(false);
        }*/
    }
}
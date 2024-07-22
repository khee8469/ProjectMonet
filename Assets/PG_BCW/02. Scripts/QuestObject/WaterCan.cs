using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterCan : ItemObject
{
    [Header("미니어처 아이템")]

    [Tooltip("클리어 확인 할 퍼즐")]
    [SerializeField]
    int puzzleID;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleID)) return;

        //퍼즐을 클리어 했으면 비활성화
        if (Manager.PlayableData.puzzleDataDic[puzzleID] == PuzzleState.Clear)
        {
            gameObject.SetActive(false);
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        StopCoroutine(coroutine);
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        coroutine = StartCoroutine(PuzzleCheck());
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        StopCoroutine(coroutine);
        coroutine = null;
    }


    Coroutine coroutine;
    IEnumerator PuzzleCheck()
    {
        while(Manager.PlayableData.puzzleDataDic[puzzleID] != PuzzleState.Clear)
        {
            yield return new WaitForSeconds(0.1F);
        }

        Destroy(gameObject);
    }
}

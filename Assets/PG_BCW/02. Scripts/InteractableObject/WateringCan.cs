using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WateringCan : InteractObject //, IPuzzleable
{
    // 소켓에 넣을때 좌표 조정용
    /*[SerializeField]
    MiniatureManager miniatureManager;
    [SerializeField]
    PuzzleManager puzzleManager;

    protected override void Awake()
    {
        base.Awake();

        miniatureManager = GetComponentInParent<MiniatureManager>();
        puzzleManager = miniatureManager.GetComponent<PuzzleManager>();
        RegistObject(puzzleManager);
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //씬3에서 물뿌리게를 인베토리에 넣고 로비로 갈때로 변경해야할듯
        if (miniatureManager.SceneNumber==3)
        {
            //
            gameObject.SetActive(false);
        }
    }


    public void RegistObject(PuzzleManager puzzle)
    {
        puzzle.puzzleObjects.Add(this);
    }

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

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        throw new System.NotImplementedException();
    }*/
}

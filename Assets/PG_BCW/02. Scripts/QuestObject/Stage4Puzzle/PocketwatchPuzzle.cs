using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PocketwatchPuzzle: ItemObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("퍼즐 클리어 조건 번호")]
    [SerializeField]
    int puzzleIndex;
    [Tooltip("분침 콜라이더")]
    [SerializeField]
    Collider minuteCollider;

    [Tooltip("회중시계")]
    [SerializeField]
    InteractObject pocketWatch;


    protected override void Awake()
    {
        base.Awake();

        //퍼즐매니저에 등록
        if (puzzleManager != null)
            RegistObject(puzzleManager);

        //puzzleDataDic에 키값이 없으면
        if (puzzleManager != null)
        {
            if (!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleManager.PuzzleID))
                Manager.PlayableData.puzzleDataDic.Add(puzzleManager.PuzzleID, PuzzleState.DisActive);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (pocketWatch != null)
            pocketWatch.selectEntered.AddListener(MinuteColliderOn);

        //이미 획득햇으면 회중시계 비활성화
        if (puzzleManager != null || Manager.PlayableData.CheckItemInInventory(ItemID))
        {
            pocketWatch.gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (pocketWatch != null)
            pocketWatch.selectEntered.RemoveListener(MinuteColliderOn);
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);


    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (puzzleManager != null)
        {
            if (Manager.PlayableData.CheckItemInInventory(ItemID))
            {
                UpdatePuzzleManager(puzzleManager, puzzleIndex);
            }
        }
    }


    //시계를 잡아야 분침을 잡을수있게
    private void MinuteColliderOn(SelectEnterEventArgs args)
    {
        minuteCollider.enabled = true;
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

    }
    public void DisActiveSetting()
    {

    }
    public void CompleteSetting()
    {

    }
}

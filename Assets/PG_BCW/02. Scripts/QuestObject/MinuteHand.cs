using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MinuteHand : ItemObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("퍼즐 클리어 조건 번호")]
    [SerializeField]
    int puzzleIndex;
    [SerializeField]
    Rigidbody rb;
    [Tooltip("시계 콜라이더")]
    [SerializeField]
    Collider clockCollider;
    [Tooltip("분침 콜라이더")]
    [SerializeField]
    Collider minuteCollider;
    [SerializeField]
    InteractObject interactObject;


    protected override void Awake()
    {
        base.Awake();

        //퍼즐매니저에 등록
        if (puzzleManager != null)
            RegistObject(puzzleManager);

        //puzzleDataDic에 키값이 없으면 할당
        if (!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleManager.PuzzleID))
            Manager.PlayableData.puzzleDataDic.Add(puzzleManager.PuzzleID, PuzzleState.DisActive);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //상태에 따른 세팅 
        puzzleManager.PuzzleSetting(Manager.Quest.QuestDic[puzzleManager.activeQuestID].State);


        if (interactObject != null)
            interactObject.selectEntered.AddListener(MinuteColliderOn);
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log(123);
        //인벤토리에 생성
        Manager.Item.GetItem(ItemID, true);
        //퍼즐 클리어
        if (puzzleManager != null)
        {
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
        }
    }


    /*protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        ////인벤토리에 넣엇으면 클리어
        if (Manager.PlayableData.CheckItemInInventory(ItemID))
        {
            if (puzzleManager != null)
                UpdatePuzzleManager(puzzleManager, puzzleIndex);

            Destroy(gameObject);
        }

        //원위치 시켜야함
    }*/



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
        /*if (clockCollider != null)
            clockCollider.enabled = true;*/
    }
    public void DisActiveSetting()
    {
        /*if (clockCollider != null)
            clockCollider.enabled = false;*/
    }
    public void CompleteSetting()
    {
        if (clockCollider != null)
        {
            Debug.Log(3);
            Destroy(clockCollider.gameObject);
        }
    }
}

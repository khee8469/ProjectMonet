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
      
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (pocketWatch != null)
            pocketWatch.selectEntered.AddListener(MinuteColliderOn);

        //이미 획득햇으면 회중시계 비활성화
        if (Manager.PlayableData.CheckItemInInventory(ItemID) && puzzleManager != null)
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

        Debug.Log(123123);

        //인벤토리에 생성
        if (!Manager.PlayableData.CheckItemInInventory(ItemID))
        {
            Manager.Item.GetItem(ItemID, true);

            //퍼즐 클리어
            if (puzzleManager != null)
            {
                UpdatePuzzleManager(puzzleManager, puzzleIndex);
            }
        }

        //this.interactionManager.SelectExit(args.interactorObject, this);
        //this.gameObject.SetActive(false);
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

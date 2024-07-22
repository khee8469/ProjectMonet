using Jc;
using System.Collections;
using System.Collections.Generic;
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

        if (puzzleManager == null) Debug.LogError($"MinuteHand에 puzzleManager 할당하시오");
        if (clockCollider == null) Debug.LogError($"MinuteHand에 clockCollider 할당하시오");
        if (rb == null) Debug.LogError($"MinuteHand에 rb 할당하시오");
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        if (Manager.PlayableData.CheckItemInInventory(ItemID))
        {
            Destroy(clockCollider.gameObject);
        }
        else
        {
            if (interactObject != null)
                interactObject.selectEntered.AddListener(MinuteColliderOn);
        }
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //인벤토리에 생성
        Manager.Item.GetItem(ItemID, true);
        //퍼즐 클리어
        if (puzzleManager != null)
        {
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
        }

        Destroy(gameObject);
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

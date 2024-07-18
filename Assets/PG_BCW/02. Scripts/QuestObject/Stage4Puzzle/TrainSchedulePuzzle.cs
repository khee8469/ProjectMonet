using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrainSchedulePuzzle : XRSocketInteractor, IPuzzleable
{
    [Header("에디터 세팅")]

    [SerializeField]
    private PuzzleManager puzzle;
    [Tooltip("퍼즐 인덱스")]
    [SerializeField]
    private int puzzleIndex;
    [Tooltip("클리어용 아이템 ID")]
    [SerializeField]
    private int itemID;
    [Tooltip("보간 시간")]
    [SerializeField]
    private float trackingTime = 0.5f;
    [Tooltip("충돌확인 콜라이더")]
    [SerializeField]
    private Collider triggerCollider;
    [Tooltip("클리어 활성화 렌더러 위치")]
    [SerializeField]
    private MeshRenderer clearPoint;





    protected override void Awake()
    {
        RegistObject(puzzle);

        if (puzzle == null) Debug.LogError($"TrainSchedulePuzzle 에 PuzzleManager를 참조하시오");
        if (triggerCollider == null) Debug.LogError($"TrainSchedulePuzzle 에 Collider 참조하시오");
        if(clearPoint == null) Debug.LogError($"TrainSchedulePuzzle 에 MeshRenderer 참조하시오");
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //아이디값이 같은거만 셀렉트
        if(args.interactableObject.transform.GetComponent<ItemObject>().ItemID != itemID)
        {
            this.interactionManager.SelectExit(this, args.interactableObject);
            return;
        }

        Transform selectObject = args.interactableObject.transform;
        selectObject.parent = transform;
        StartCoroutine(SuccessRoutine(selectObject));
    }


    // 오브젝트 소켓 트래킹 루틴 
    IEnumerator SuccessRoutine(Transform selectObject)
    {
        Debug.Log("코르틴시작");
        float rate = 0f;
        Vector3 startPos = selectObject.position;
        Quaternion startRot = selectObject.rotation;
        Vector3 endPos = clearPoint.transform.position;
        Quaternion endRot = clearPoint.transform.rotation;

        while (rate < 1f)
        {
            rate += Time.deltaTime / trackingTime;
            selectObject.position = Vector3.Lerp(startPos, endPos, rate);
            selectObject.rotation = Quaternion.Lerp(startRot, endRot, rate);
            yield return null;
        }

        selectObject.localPosition = Vector3.zero;
        selectObject.localRotation = Quaternion.identity;
        // 퍼즐매니저 업데이트
        UpdatePuzzleManager(puzzle, puzzleIndex);
    }

    // 인터페이스 재정의 
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
        triggerCollider.enabled = true;
    }
    public void DisActiveSetting()
    {
        //triggerCollider.enabled = false;
    }
    public void CompleteSetting()
    {
        Debug.Log("컴플리트");
        //MeshRenderer On
        clearPoint.enabled = false;
        //Instantiate(Manager.Item.ItemDataDic[itemID].itemPrefab, prefabPoint.position, prefabPoint.rotation);
        puzzle.UpdateCondition(puzzleIndex);
    }
}

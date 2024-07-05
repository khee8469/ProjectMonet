using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WindMill : InteractObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("성공조건 : 회전 속도")]
    [SerializeField]
    float rotationSpeed;
    [Tooltip("퍼즐 번호")]
    [SerializeField]
    int puzzlleIndex;

    Rigidbody rb;

    //미션 클리어 체크
    private bool isSucess;
    public bool IsSucess { get { return isSucess; } }
    //어느 방향으로 돌아갓는지
    private bool leftRotation;


    protected override void Awake()
    {
        base.Awake();

        //퍼즐매니저에 등록
        puzzleManager = GetComponentInParent<PuzzleManager>();
        RegistObject(puzzleManager);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 이 오브젝트에 없습니다!");
        }
    }

    private void Update()
    {
        //회전속도가 일정속도가 되면
        if(Mathf.Abs(rb.angularVelocity.z) > rotationSpeed && !IsSucess)
        {
            //더이상 조작 못하게
            DisActiveSetting();
            //조건 완료여부 확인 및 클리어
            puzzleManager.UpdateCondition();

            //어느방향으로 회전중인지
            if(rb.angularVelocity.z >= 0)
            {
                leftRotation = true; 
            }
            else if(rb.angularVelocity.z < 0)
            {
                leftRotation = false;
            }

            isSucess = true;
        }

        //완료했으면 계속 회전
        if (isSucess)
        {
            if (leftRotation)
            {
                transform.Rotate(Vector3.forward, -20 * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward, 20 * Time.deltaTime);
            }
        }
    }

    //활성화 상태에서
    public void ActiveSetting()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        isSucess = false;
    }

    //비활성화 상태에서
    public void DisActiveSetting()
    {
        //완료후 잡지못하게 레이어 설정
        interactionLayers = 0;
        //더이상 손과 충돌하지않게 충돌체 끄기
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    //퍼즐 성공
    public void CompleteSetting()
    {
        //풍차 미션 클리어 저장
    }

    //UpdatePuzzleManager에서 불러오는
    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        
    }






    //퍼즐 매니저에 등록시
    public void RegistObject(PuzzleManager puzzle)
    {
        puzzle.puzzleObjects.Add(this);
    }
}

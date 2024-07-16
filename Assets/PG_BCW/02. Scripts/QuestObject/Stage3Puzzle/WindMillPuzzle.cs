using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WindMillPuzzle : InteractObject, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("몇도 돌려야 하는지")]
    [SerializeField]
    public float requiredRotation = 360f; // 필요한 회전 각도 (도 단위)
    [Tooltip("성공보상 : 자동 회전 속도")]
    [SerializeField]
    float rotationSpeed;
    [Tooltip("퍼즐 클리어 조건 번호")]
    [SerializeField]
    int puzzleIndex;
    [Tooltip("회전속도 체크용")]
    [SerializeField]
    Rigidbody rb;
    [Tooltip("성공 후 콜라이더 끄기용")]
    [SerializeField]
    Collider leverCollider;

    //레버를잡앗는지
    bool leverSelect;
    //어느 방향으로 돌아갓는지
    private bool leftRotation;

    float startRotation;
    float endRotation;
    private float totalRotation = 0f;
    //레버 돌리기 성공
    bool success;




    protected override void Awake()
    {
        base.Awake();

        if (puzzleManager == null)
            Debug.LogError("puzzleManager 컴포넌트가 이 오브젝트에 없습니다!");
        if (rotationSpeed == 0)
            Debug.LogError("rotationSpeed 가 0 입니다.!");
        if (rb == null)
            Debug.LogError("Rigidbody 컴포넌트가 이 오브젝트에 없습니다!");

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

        //스테이지3의 상태에 따라
        if (Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] == PuzzleState.Clear)
        {
            //CompleteSetting();
        }
    }


    //상태 초기화
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("상태초기화");
            Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] = PuzzleState.DisActive;
            Manager.PlayableData.SavePuzzleData();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("진행중");
            Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] = PuzzleState.Proceed;
            Manager.PlayableData.SavePuzzleData();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("클리어");
            Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] = PuzzleState.Clear;
            Manager.PlayableData.SavePuzzleData();
        }
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        startRotation = transform.eulerAngles.z;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        endRotation = transform.eulerAngles.z;

        LeverRotation(startRotation, endRotation);

        //속도 0으로만든후 회전 시작
        rb.angularVelocity = Vector3.zero;
    }

    /*IEnumerator LeverRotation()
    {
        startRotation = transform.rotation;
        previousRotation = transform.rotation;

        while (leverSelect)
        {
            yield return new WaitForSeconds(0.1f);

            Quaternion currentRotation = transform.rotation;
            float rotationThisFrame = Quaternion.Angle(previousRotation, currentRotation);
            Debug.Log($"rotationThisFrame : {rotationThisFrame}");
            totalRotation += rotationThisFrame;
            previousRotation = currentRotation;
            //Debug.Log($"totalRotation {totalRotation}");
            if (Mathf.Abs(totalRotation) >= requiredRotation)
            {
                success = true;

                if (rb.angularVelocity.z >= 0)
                {
                    leftRotation = true;
                }
                else if (rb.angularVelocity.z < 0)
                {
                    leftRotation = false;
                }
                break;
            }
        }
    }*/

    private void LeverRotation(float startRotation, float endRotation)
    {
        // 이전 프레임과 현재 프레임 사이의 회전 각도 차이를 계산
        float rotationDelta = Mathf.DeltaAngle(startRotation, endRotation);

        Debug.Log(rotationDelta);
        // 회전 각도를 총 회전 각도에 누적 또는 차감
        totalRotation += rotationDelta;

        if (Mathf.Abs(totalRotation) >= requiredRotation)
        {
            if (totalRotation < 0)
            {
                leftRotation = true;
            }
            else if (totalRotation > 0)
            {
                leftRotation = false;
            }


            //퍼즐성공 저장
            UpdatePuzzleManager(puzzleManager, puzzleIndex);
            //회전 코르틴 시작
            StartCoroutine(AngularVelocity());
            //더이상 조작 못하게
            if (leverCollider != null) leverCollider.enabled = false;
            rb.isKinematic = false;
        }
    }



    IEnumerator AngularVelocity()
    {
        while (true)
        {
            if (leftRotation)
            {
                transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }


    public void ActiveSetting()
    {
        //순서인 퀘스트 오브젝트 활성화
        Debug.Log("풍차 퀘스트 진행중");
        if (leverCollider != null) leverCollider.enabled = true;
    }

    public void DisActiveSetting()
    {
        //순서아닌 퀘스트 오브젝트 비활성화
        if (leverCollider != null) leverCollider.enabled = false;
    }


    //퍼즐클리어 조건중 하나 성공으로 변경
    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        puzzle.UpdateCondition(index); //아니면 정재훈잘못
    }


    //퍼즐 매니저에 등록시
    public void RegistObject(PuzzleManager puzzle)
    {
        if (puzzle == null)
            return;
        puzzle.puzzleObjects.Add(this);
    }

    //퍼즐 성공
    public void CompleteSetting()
    {
        UpdatePuzzleManager(puzzleManager, puzzleIndex);
        if (leverCollider != null) leverCollider.enabled = false;
        StartCoroutine(AngularVelocity());
    }
}

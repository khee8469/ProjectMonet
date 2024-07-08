using Jc;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR.Interaction.Toolkit;

public class WindMill : MonoBehaviour, IPuzzleable
{
    [Header("현재 오브젝트 정보")]
    [SerializeField]
    PuzzleManager puzzleManager;
    [Tooltip("성공조건 : 회전 속도")]
    [SerializeField]
    float successSpeed;
    [Tooltip("성공보상 : 풍차회전 속도")]
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
    Collider fanCollider;

    //어느 방향으로 돌아갓는지
    private bool leftRotation;

    //미션 클리어 체크
    private bool isSucess;
    public bool IsSucess { get { return isSucess; } }
    


    private void Awake()
    {
        if(puzzleManager == null)
            Debug.LogError("puzzleManager 컴포넌트가 이 오브젝트에 없습니다!");
        if (rotationSpeed == 0)
            Debug.LogError("rotationSpeed 가 0 입니다.!");
        if (rb == null)
            Debug.LogError("Rigidbody 컴포넌트가 이 오브젝트에 없습니다!");
        if(fanCollider == null)
            Debug.LogError("Collider컴포넌트가 이  오브젝트에 없습니다!");

        //퍼즐매니저에 등록
        if (puzzleManager != null)
            RegistObject(puzzleManager);

        //puzzleDataDic에 키값이 없으면 할당
        if (!Manager.PlayableData.puzzleDataDic.ContainsKey(puzzleManager.PuzzleID))
            Manager.PlayableData.puzzleDataDic.Add(puzzleManager.PuzzleID, PuzzleState.DisActive);

    }

    private void OnEnable()
    {
        //회전속도 체크 코르틴
        StartCoroutine(AngularVelocity());
        //스테이지3의 상태에 따라
        if (Manager.PlayableData.puzzleDataDic[puzzleManager.PuzzleID] == PuzzleState.Clear)
        {
            CompleteSetting();
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator AngularVelocity()
    {
        while (!isSucess)
        {
            //회전속도가 일정속도가 되면
            if (Mathf.Abs(rb.angularVelocity.z) > successSpeed)
            {
                //어느방향으로 회전중인지
                if (rb.angularVelocity.z >= 0)
                {
                    leftRotation = true;
                }
                else if (rb.angularVelocity.z < 0)
                {
                    leftRotation = false;
                }

                CompleteSetting();
                fanCollider.enabled = false;
                isSucess = true;
            }

            yield return null;
        }
        

        //완료했으면 계속 회전
        while (isSucess)
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
    }

    public void DisActiveSetting()
    {
        //순서아닌 퀘스트 오브젝트 비활성화
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
        fanCollider.enabled = false;
        isSucess = true;
    }
}

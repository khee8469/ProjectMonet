using Jc;
using UnityEngine;
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
    [Tooltip("퍼즐 번호")]
    [SerializeField]
    int puzzleIndex;
    [Tooltip("회전속도 체크용")]
    [SerializeField]
    Rigidbody rb;
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

        //퍼즐매니저에 등록
        if (puzzleManager != null)
            RegistObject(puzzleManager);
    }


    private void Update()
    {
        //회전속도가 일정속도가 되면
        if (Mathf.Abs(rb.angularVelocity.z) > successSpeed && !IsSucess)
        {
            //더이상 조작 못하게
            DisActiveSetting();

            //조건 및 클리어 확인
            UpdatePuzzleManager(puzzleManager, puzzleIndex);

            //어느방향으로 회전중인지
            if (rb.angularVelocity.z >= 0)
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
                transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    //활성화 상태에서
    public void ActiveSetting()
    {
        /*for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = true;
        }*/

        isSucess = false;
    }

    //비활성화 상태에서
    public void DisActiveSetting()
    {
        /*//완료후 잡지못하게 레이어 설정
        interactionLayers = 0;
        //더이상 손과 충돌하지않게 충돌체 끄기
        for(int i = 0; i < colliders.Count; i++) 
        {
            colliders[i].enabled = false;
        }*/
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
        //Debug.Log(111);
        //풍차 미션 클리어 저장
    }
}

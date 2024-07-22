using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
using UnityEditor.ShaderGraph.Internal;

namespace JJH
{

    public class LightHouseHead : MonoBehaviour, IPuzzleable
    {

        /*[Tooltip("퍼즐 매니저")]
        [SerializeField] private Chapter2SunHole puzzleSun; // 얘 한테 퍼즐매니저 붙어 있음. ㅠㅠ */

        [Header("퍼즐 매니저 sunhole")]
        [Tooltip("체크 해야 할 검은 구멍")]
        [SerializeField]
        private Chapter2SunHole sunHole;

        [Tooltip("레이 캐스트 발사 위치")]
        [SerializeField]
        private Transform rayStartPos;

        [Tooltip("퍼즐의 On OFF 상태 체크 --> 임시 (나중에 Manager와 연계 할 것")]
        [SerializeField] private bool puzzleOn;

        [Tooltip("닿아야 할 태양구멍의 layer --> Puzzle")]
        [SerializeField] LayerMask layerMask;

        [Tooltip("보여 줄 빛 기둥")]
        [SerializeField] public GameObject pillar_Of_Light;

        [Tooltip("레이캐스트 발사 거리")]
        [SerializeField] private float distance = 4000f;

        [Header("클리어를 위한 등대의 각도")]
        // raycast를 대신하여 등대의 각도를 파악 한 후 그 각도가 올바르면 퍼즐 클리어 하도록 체크 
        [SerializeField] private float XminValue;
        [SerializeField] private float XmaxValue;
        [SerializeField] private float YminValue;
        [SerializeField] private float YmaxValue;
        [SerializeField] private float ZmaxValue;
        [SerializeField] private float ZminValue;


        [Tooltip("클리어 시 창문으로 비칠 햇빛")]
        [SerializeField]
        private Light clearLight;

        

        [Tooltip("버튼이 꺼지는 거는 버튼이 함수로 가지고 있고 그거를 여기서 빌려쓰는 식으로 하자.")]
        [SerializeField] private LightPanelButton[] buttons;
        

        // 빛 기둥 같은 경우는 이제 그냥 켜주기만 하면 되는 느낌이겠지. 
        // ray 보다는 패널 에서 자기 위치 체크를 하는게 낫지 않나? 

        // Ray 안 쓰고 그냥 각도로 하는 중이니까 각도로 하는게 나을 수도 


        private void Awake()
        {
            PillarChange(false); // 일단 어웨이크 에서 꺼주자.
            RegistObject(sunHole);
        }


        private void PillarChange(bool boolean)
        {
            pillar_Of_Light.SetActive(boolean);
        }


        // 등대의 각도와 태양의 각도 

        [Tooltip("오차 범위 설정")]
        [SerializeField] private float tolerance = 3f;
        
        public void MyCheckRotation(Quaternion endRotation)
        {
            float angleX = endRotation.eulerAngles.x;
            float angleY = endRotation.eulerAngles.y;
            float angleZ = endRotation.eulerAngles.z;

            Debug.Log($"앵글 들의 값 x y z {angleX} {angleY} {angleZ} ");

            // 각각의 angle 값이 0~360도 중에 어느 정도 값 사이에 들어가 있어야 체크 할지 파악하면된다.
            // 지금 임의적으로 숫자 넣어준거임
            if ((angleX >= 20 - tolerance && angleX <= 20 + tolerance)&&(angleZ >= 355 - tolerance && angleZ <= 355 + tolerance))
            {
                sunHole.OnClearPuzzle();
                Debug.Log("정답 체크");
                DeAactiveLight_Button();
                ChangeRoutine(); // 등대 빛 일단 끄자 이상해 보임.. 
                MyCompleteRotation();

            }
        }

        public void PuzzleOn()
        {
            puzzleOn = true;
            Debug.Log("등대 퍼즐 켜짐");
        }

        public void DeAactiveLight_Button() 
        {
            for(int i=0;i < buttons.Length;i++)
            {
                buttons[i].MyEnable(false);
            }
        }


        private void ChangeRoutine()
        {
            StartCoroutine(ChangeCoRoutine());  //--> 등대 빛 없애기 기능 
        }

        private IEnumerator ChangeCoRoutine() 
        {

            // 등대 빛 점점 약하게.
            // 햇빛 emission 값 점점 밝게 하다가.
            // 루틴 끝나면 스카이박스도 바꿔줘야 할듯? 어차피 흑백이니까 그렇게 크게 티 나지는 않을듯함. 
            LineRenderer line = pillar_Of_Light.GetComponent<LineRenderer>();
            float during = 2f;
            float elapse = 0f;
            if (line == null)
            {
                yield break;
            }

            while (elapse < during)
            {
                elapse += Time.deltaTime;
                line.startWidth =  Mathf.Lerp(line.startWidth, 0, elapse / during);
                line.endWidth = Mathf.Lerp(line.endWidth, 0, elapse / during);
                yield return null;
            }

            line.enabled = false; //완료되면 꺼주기. 
        }

        public void MyCompleteRotation() //클리어 했을 때의 등대 위치 저장. 
        {
            Debug.Log("등대 로테이션 저장... but 저장해야 하는게 등대 머리랑 바닥이랑 두개라서 거의 불가능 할 것 같음...");
        }


        #region 퍼즐 인터페이스 오버라이드 
        public void ActiveSetting()  // 진행 가능한 상태의 세팅 
        {
            // 등대의 불은 나오고 있어야 함. 
            PillarChange(true);
            Debug.Log("등대의 액티브세팅");
        }

        public void CompleteSetting() // 완성되 있는 상태 세팅 ++ 스카이박스 변경 필요.
        {
            Debug.Log("등대의 컴플리트 세팅");  // 스카이박스 변경 부르기 
            DeAactiveLight_Button();
            sunHole.OnClearPuzzle();
            PillarChange(true);
            MyCompleteRotation(); // 클리어 세팅에서 자신의 등대 저장 
        }

        public void DisActiveSetting()  // 진행 불가능 한 상태의 세팅 --> 퍼즐 진입 전 상태 
        {
            DeAactiveLight_Button(); // 버튼 꺼두기 
            PillarChange(false); // 불이 아직 들어오지 않음 
            Debug.Log("등대의 디스액티브세팅");

        }

        public void RegistObject(PuzzleManager puzzle)  // Awake 에서 발동. 
        {
            // 퍼즐 매니저에 자신을 등록 
            puzzle.puzzleObjects.Add(this);
            Debug.Log("등대의 레지스터");
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            // 얘는 어차피 완료 조건이 하나 니까 그냥 OnClear 부르는 방식으로 진행 할 것. 
            sunHole.OnClearPuzzle();
            Debug.Log("등대의 업데이트 퍼즐");
        }
        #endregion
    }
}



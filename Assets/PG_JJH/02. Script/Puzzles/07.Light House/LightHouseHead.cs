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


        [Tooltip("버튼 배열 가지고 있기")]
        [SerializeField]
        private LightPanelButton[] buttons;

        [Tooltip("클리어 시 창문으로 비칠 햇빛")]
        [SerializeField]
        private Light clearLight;

        // 빛 기둥 같은 경우는 이제 그냥 켜주기만 하면 되는 느낌이겠지. 
        // ray 보다는 패널 에서 자기 위치 체크를 하는게 낫지 않나? 

        private void Start()
        {
            //pillar_Of_Light.gameObject.SetActive(false);
        }

        private void Update()
        {
            // 퍼즐이 On 된 상태에서만 RayCast 및 빛 기둥 발사를 해야한다.
            if(puzzleOn ==true)
            {
                // 다른 함수로 Pillar 를 Active 하는 부분을 CallBack 으로 불러서 (이전 퍼즐에서)
                // 켜줘야 한다. Ray도 마찬가지고 
                //RayOn();
                PillarChange(true);
            }
        }
        private void RayOn() //Ray 든 뭐 빛 기둥이던 어쨋든 이전 퍼즐을 깨야 발동이 가능하다. 
        {
            RaycastHit hit;
            Debug.DrawRay(rayStartPos.position, rayStartPos.forward * distance, Color.red);
            if (Physics.Raycast(rayStartPos.position , rayStartPos.forward , out hit , distance, layerMask))
            {
                // 이미 레이어 마스크 체크로 들어갔기 때문에 그냥 레이가 부딪혔으면 부딪힌 거임.
                sunHole = hit.collider.GetComponent<Chapter2SunHole>();
                if(sunHole != null)
                {
                    Debug.Log("구멍 체크 완료");
                }
            }
        }

        private void PillarChange(bool boolean)
        {
            pillar_Of_Light.SetActive(boolean);
        }


        // 등대의 각도와 태양의 각도 

        [Tooltip("오차 범위 설정")]
        [SerializeField] private float tolerance = 0.2f;
        
        public void MyCheckRotation(Quaternion endRotation)
        {
            float angleX = endRotation.eulerAngles.x;
            float angleY = endRotation.eulerAngles.y;
            float angleZ = endRotation.eulerAngles.z;

            Debug.Log($"앵글 들의 값 x y z {angleX} {angleY} {angleZ} ");

            // 각각의 angle 값이 0~360도 중에 어느 정도 값 사이에 들어가 있어야 체크 할지 파악하면된다.
            // 지금 임의적으로 숫자 넣어준거임
            if ((angleY >= 340 - tolerance && angleY <= 345 + tolerance)&&(angleZ >= 320 - tolerance && angleZ <= 325 + tolerance))
            {
                sunHole.OnClearPuzzle();
                DeAactiveLight_Button();
                ChangeRoutine();
                sunHole.ItemAdd(sunHole.ID);

            }
        }

        public void PuzzleOn()
        {
            puzzleOn = true;
            Debug.Log("등대 퍼즐 켜짐");
        }

        public void DeAactiveLight_Button() // 아 이거 인스펙터창에서 달았었나보다.. 
        {
            for(int i=0;i<buttons.Length;i++)
            {
                buttons[i].enabled = false; // 스크립트 꺼주기
            }
        }


        private void ChangeRoutine()
        {
            //StartCoroutine(ChangeCoRoutine());  --> 등대 빛 없애기 기능 
        }

        private IEnumerator ChangeCoRoutine() 
        {

            // 등대 빛 점점 약하게.
            // 햇빛 emission 값 점점 밝게 하다가.
            // 루틴 끝나면 스카이박스도 바꿔줘야 할듯? 어차피 흑백이니까 그렇게 크게 티 나지는 않을듯함. 
            LineRenderer line = pillar_Of_Light.GetComponent<LineRenderer>();
            float during = 2f;
            float elapse = 0f;

            while (elapse < during)
            {
                elapse += Time.deltaTime;
                line.startWidth =  Mathf.Lerp(line.startWidth, 0, elapse / during);
                line.endWidth = Mathf.Lerp(line.endWidth, 0, elapse / during);
                yield return null;
            }

            line.enabled = false; //완료되면 꺼주기. 
        }

        #region 퍼즐 인터페이스 오버라이드 
        public void ActiveSetting()  // 진행 가능한 상태의 세팅 
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].enabled = true; // 스크립트 켜주기
            }

            // 등대의 불은 나오고 있어야 함. 
            PillarChange(true);

        }

        public void CompleteSetting() // 완성되 있는 상태 세팅 ++ 스카이박스 변경 필요.
        {
            DeAactiveLight_Button();
            sunHole.OnClearPuzzle();
            PillarChange(true);
            //sunHole.ChangeSkyBox(sunHole.afternoonSkybox); // 낮 상태로 스카이박스 및 마테리얼을 빛 상태로 --> 온 클리어에 같이 있음. 

        }

        public void DisActiveSetting()  // 진행 불가능 한 상태의 세팅 --> 퍼즐 진입 전 상태 
        {
            DeAactiveLight_Button(); // 버튼 꺼두기 
            PillarChange(false); // 불이 아직 들어오지 않음 

        }

        public void RegistObject(PuzzleManager puzzle)  // Awake 에서 발동. 
        {
            // 퍼즐 매니저에 자신을 등록 
            puzzle.puzzleObjects.Add(this); 
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            // 얘는 어차피 완료 조건이 하나 니까 그냥 OnClear 부르는 방식으로 진행 할 것. 
        }
        #endregion
    }
}



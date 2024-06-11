using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class Pen : MonoBehaviour
    {
        // 라인렌더러를 이용해서 그림을 그리기. --> 캔버스로 지정된 곳에만 그림을 그릴 수 있어야 한다. 
        [Header("펜의 속성")]
        [Tooltip("펜의 펜촉 (그려지는 부분)")]
        public Transform tip;
        [Tooltip("Defalut-Line 으로 설정할 것")]
        public Material drawingMaterial;
        [Tooltip("펜촉 부분의 마테리얼")]
        public Material tipMaterial;
        [Tooltip("펜의 크기 조절 기능")]
        [Range(0.01f, 0.1f)] public float penWidth = 0.01f;
        [Tooltip("펜의 색상 배열")] //추후에 이 리스트를 이용해서 아이템과 연계로 색 변화 발생시키기? 이 부분은 나중에 추가로 생각해보기. 
        public List<Color> penColors = new List<Color>();

        [Header("렌더러와 컬러 관리")]
        [Tooltip("그려줄 라인 렌더러")]
        [SerializeField] private LineRenderer currentDrawing;
        [Tooltip("컬러 리스트의 인덱스")] // 이거 리스트 말고 딕셔너리로 해야하나? 컬러 색깔 구분해 줄 때 뭐가 편할지 생각해보자. 
        [SerializeField] private int index;
        [Tooltip("현재 컬러 인덱스")]
        [SerializeField] int currentColorIndex;

        [Tooltip("그리기 상태 관리")]
        [SerializeField] private bool isDrawing = false;

        [Header("상호작용 오브젝트 관리")]
        [Tooltip("그리기를 허용할 레이어 마스크")]
        [SerializeField] private LayerMask drawingLayer;
        [Tooltip("레이어 체크 거리")]
        [SerializeField] private float distance = 3f;


        private void Start()
        {
            currentColorIndex = 0;
            tipMaterial.color = penColors[currentColorIndex]; //현재 팁의 마테리얼은 펜컬러의 인덱스와 같다. 
            drawingLayer = LayerMask.GetMask("DrawBoard");

        }


        // 지금 update 없이 xrBase의 update 용 콜백을 받아도 제대로 동작이 안해서 이 부분 나중에 시간나면 수정하기. 
        // 레이캐스트를 계속 체크해야 하기 때문에 update 밖에 없나? 어떻게 해야할지... 
        private void Update()
        {
            if (isDrawing)
            {
                Draw();
            }
        }

        public void Draw()
        {
            if (!isDrawing) return; // 그리기 상태가 아니면 리턴 

            RaycastHit hit;

            if (Physics.Raycast(tip.position, tip.forward, out hit, distance, drawingLayer))
            {
                Debug.Log("레이캐스트 도착"); // 지금 도착이 안나옴... 문제가 뭔지 생각할 것. 
            }
            else
            {
                //DrawingStop(); // 레이어 밖이면 드로우 중지. 
                Debug.Log("레이 밖");
            }

            if (currentDrawing == null) //이 부분에서 현재 물감에 알맞는 색상으로 만들어줘야 할 것 같아. 
            {
                Debug.Log("Draw함수 내부 진입 ");


                index = 0;

                GameObject lineObj = new GameObject("Line");
                lineObj.transform.position = tip.position;
                currentDrawing = lineObj.AddComponent<LineRenderer>();

                currentDrawing.material = drawingMaterial; // 현재 마테리얼 

                // 이 부분 일단 추가하기는 했는데... 이거 lineRenderer랑 연계가 되면 없어도 되는디... 
                currentDrawing.material.color = penColors[currentColorIndex];

                currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex]; //현재 색깔

                currentDrawing.startWidth = currentDrawing.endWidth = penWidth; //일정한 굵기. 
                currentDrawing.positionCount = 1; // 시작 포지션 카운트 1
                currentDrawing.SetPosition(0, tip.position); // 0번째 인덱스의 포지션은 펜촉(tip)의 위치.
                Debug.Log("LineRenderer 생성됨: " + tip.position);
            }
            else // 즉 이미 생성된 경우. 
            {
                var currentPos = currentDrawing.GetPosition(index);
                Debug.Log("현재 위치: " + currentPos);
                currentDrawing.material.color = penColors[currentColorIndex];

                if (Vector3.Distance(currentPos, tip.position) > 0.01f)
                {
                    index++;
                    currentDrawing.positionCount = index + 1;
                    currentDrawing.SetPosition(index, tip.position);
                    Debug.Log("새 위치 추가됨: " + tip.position);
                }
            }
        }

        public void StartDrawing()
        {
            isDrawing = true; // 그리기 상태로 전환
            Debug.Log("LineRenderer 시작됨");
        }

        public void DrawingStop()
        {
            isDrawing = false; //그리기 상태 중지로 설정
            Debug.Log("LineRenderer 중지됨");
        }

        // 물감과의 연계가 필요하기 때문에 
        public void SwitchColor()  // 색상 전환은 일단 나중에.
        {
            if (currentColorIndex == penColors.Count - 1)
            {
                currentColorIndex = 0;
            }
            else
            {
                currentColorIndex++;
            }

            tipMaterial.color = penColors[currentColorIndex];

        }
    }
}



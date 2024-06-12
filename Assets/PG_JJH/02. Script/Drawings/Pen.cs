using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class Pen : MonoBehaviour
    {
        // 어차피 한 번에 하나의 색 밖에 안되니까 크게 문제 없을것 같기는함. 
        // 새로운 line을 생성해주는거니까. 나중에 문제 생기면 마테리얼이 같이 바뀌는거는 그때 해결해주자. 

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
        /*[SerializeField]*/
        private float distance = 0.4f;

        [Header("삭제 및 이미지 연계")]
        [Tooltip("생성된 라인렌더러를 저장 해 줄 리스트")]
        [SerializeField] List<GameObject> lineList = new List<GameObject>();

        [SerializeField]
        [Tooltip("해당 캔버스와 관련된 참조")]
        private DrawObjectManager drawManager;
        [Tooltip("Noraml 벡터 크기")]
        private float NormalDis = 0.01f;

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
                Debug.DrawRay(tip.position, tip.forward * distance, Color.red);

                Vector3 drawPosition = hit.point + hit.normal * NormalDis;
                drawManager = hit.collider?.GetComponent<DrawObjectManager>();

                if (!CheckColorType(drawManager))
                {
                    DrawingStop();
                    return;
                }

                if (currentDrawing == null) //이 부분에서 현재 물감에 알맞는 색상으로 만들어줘야 할 것 같아. 
                {
                    index = 0;

                    GameObject lineObj = new GameObject("Line");

                    lineObj.transform.position = tip.position;
                    currentDrawing = lineObj.AddComponent<LineRenderer>();

                    currentDrawing.material = drawingMaterial; // 현재 마테리얼 

                    currentDrawing.material.color = penColors[currentColorIndex];

                    currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex]; //현재 색깔

                    currentDrawing.startWidth = currentDrawing.endWidth = penWidth; //일정한 굵기. 
                    currentDrawing.positionCount = 1; // 시작 포지션 카운트 1

                    // hit를 이용하여 hit 포지션에 드로잉을 하기
                    
                    currentDrawing.SetPosition(0, drawPosition);

                    drawManager.AddLineRenderer(currentDrawing, penWidth);

                    lineList.Add(lineObj);

                    Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);  
                }
                else // 즉 이미 생성된 경우. 
                {
                    
                    var currentPos = currentDrawing.GetPosition(index);
                    currentDrawing.material.color = penColors[currentColorIndex];

                    if (Vector3.Distance(currentPos, drawPosition) > 0.01f)
                    {
                        index++;
                        currentDrawing.positionCount = index + 1;

                        currentDrawing.SetPosition(index, drawPosition);

                        Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
                    }
                }
            }
            else
            {
                DrawingStop(); 
            }
        }

        private bool CheckColorType(DrawObjectManager drawObjectManager)
        {
            if (drawObjectManager == null)
            {
                return false;
            }

            return drawObjectManager.ObjectMyColor == penColors[currentColorIndex];

        }

        public void StartDrawing()
        {
            isDrawing = true; // 그리기 상태로 전환

        }

        public void DrawingStop()
        {
            isDrawing = false; //그리기 상태 중지로 설정

            if (currentDrawing != null)
            {
                currentDrawing = null;
            }
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


        //한 라인 씩 Undo 할 필요는 없을 듯 함. --> 한 번에 라인 삭제 가능한 함수. 
        public void RemoveALLLine() // 삭제가 지금 한 번에 안되니까 생각해보자. 
        {
            if (lineList.Count == 0) return;

            for (int i = lineList.Count - 1; i >= 0; i--)
            {
                LineRenderer lineObj = lineList[i]?.GetComponent<LineRenderer>();
                drawManager?.RemoveLineRenderer(lineObj);

                Destroy(lineObj.gameObject);
                lineList.RemoveAt(i);
            }

        }

       
    }
}



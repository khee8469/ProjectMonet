using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class DrawingController : MonoBehaviour
    {
        public GameObject drawingCanvas;
        public Material blackMaterial;
        public LineRenderer lineRendererPrefab;  // 나중에 라인렌더러 프리팹이 변할 수도 있지 --> ex ) 물감색 
        private Renderer canvasRenderer; //캔버스의 renderer 상황에 따라 mesh 일 수도 skinned 일 수도 있음. 
        private float totalArea; // 도화지의 총 면적 
        private float drawnArea = 0; // 현재 그려진 영역 
        private bool isDrawingEnabled = true; // 더 이상 그릴 수 있는지 없는지를 확인 하기 위한 bool 변수 
        private List<Vector3> drawnPoints = new List<Vector3>(); // 그려진 점들의 리스트 --> 어떤 식으로 이용?? 

        private void Start()
        {
            canvasRenderer = drawingCanvas.GetComponent<Renderer>();
            totalArea = drawingCanvas.transform.localScale.x * drawingCanvas.transform.localScale.z;

        }

        private void Update()
        {
            // 그릴 수 있는 상황인지 확인. 
            if (isDrawingEnabled && Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                RaycastHit hit;

                // mouse point를 ray로 detect 

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == drawingCanvas)
                    {
                        Vector3 localHitPoint = drawingCanvas.transform.InverseTransformPoint(hit.point);

                        if(IsPointInCanvas(localHitPoint)) // 캔버스 내부에서만 드로우 시도. 
                        {
                            drawnPoints.Add(localHitPoint);
                            DrawLine(); 
                        }


                    }
                }
            }
        }

        private void DrawLine()
        {
            if (drawnPoints.Count < 2) return;

            // 새로운 라인렌더러 생성 --> 지정된 포인트에 
            LineRenderer line = Instantiate(lineRendererPrefab, drawingCanvas.transform);
            line.positionCount = drawnPoints.Count; //라인렌더러 컴포넌트에 있는 꼭지점(position) 들. 
            line.SetPositions(drawnPoints.ToArray());

            drawnArea += CalculateDrawArea(); // 그려진 범위를 새롭게 계산함. 

            if (drawnArea / totalArea >= 0.5f) // 50퍼 이상 그려지면 
            {
                canvasRenderer.material = blackMaterial; // 다른 마테리얼로 바꿔버리고
                // 이 경우 --> 캔버스의 색을 채운다는 느낌 ++ 실제 월드에 위치의 마테리얼도 같이 변경해줘야함 
                isDrawingEnabled = false; // 더 이상 그림 못 그리도록 해줌. 
            }



        }

        // 그리는 point가 캔버스 내부에 있는 확인함. --> 캔버스 바깥이면 return 때려서 그려지지 않도록 
        private bool IsPointInCanvas(Vector3 point)
        {
            return point.x >= -0.5f && point.x <= 0.5f && point.y >= -0.5f && point.y <= 0.5f;
        }

        private float CalculateDrawArea()
        {
            if (drawnPoints.Count < 2) return 0;

            // 마지막 두 점 사이의 거리를 계산함
            Vector3 lastPoint = drawnPoints[drawnPoints.Count - 2];
            Vector3 currentPoint = drawnPoints[drawnPoints.Count - 1];
            return Vector3.Distance(currentPoint, lastPoint);

        }


    }
}


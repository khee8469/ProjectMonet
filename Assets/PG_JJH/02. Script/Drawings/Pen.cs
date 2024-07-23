using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class Pen : InteractObject
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

        [Header("렌더러와 컬러 관리")]
        [Tooltip("그려줄 라인 렌더러")]
        [SerializeField] public LineRenderer currentDrawing; // 드로우 오브젝트들과 비교해줄 펜의 현재 라인렌더러


        [Header("스크립터블 오브젝트 관련")]
        [Tooltip("현재 컬러 타입")]
        [SerializeField] private PaintTypeEnum currentPaintType;

        [Tooltip("색상 데이터를 관리하는 스크립터블 오브젝트")]
        public PaintTypeManager paintTypeManager;

        [Tooltip("그리기 상태 관리")]
        [SerializeField] private bool isDrawing = false;

        [Header("상호작용 오브젝트 관리")]
        [Tooltip("그리기를 허용할 레이어 마스크")]

        [SerializeField] private LayerMask drawingLayer;

        [Tooltip("레이어 체크 거리")]
        [SerializeField] private float distance = 2.5f;

        [Header("삭제 및 이미지 연계")]
        [Tooltip("생성된 라인렌더러를 저장 해 줄 리스트")]
        [SerializeField] List<GameObject> lineList = new List<GameObject>();

        [Tooltip("해당 캔버스와 관련된 참조")]
        private DrawObjectManager drawManager;

        [Tooltip("Noraml 벡터 크기")]
        private float NormalDis = 0.007f;

        [Tooltip("원하는 완료 퍼센트")]
        [SerializeField] private float percent = 5;

        [Tooltip("라인렌더러의 포지션 위한 인덱스")]
        [SerializeField] private int index;

       

        [Tooltip("update 여러번 진입 방지를 위한 bool 변수")]
        [SerializeField] private bool isNotEntered;


        [Tooltip("자신의 시작 시의 위치")]
        public Vector3 startPosition;

        [Tooltip("자신의 로테이션 위치")]
        public Quaternion startRotation;


        private void Start() // 시작 시에는 무조건 하얀색. 
        {
            currentPaintType = PaintTypeEnum.None; //기본 색상으로 지정해주기. 

            if (paintTypeManager != null)
            {
                // 스크립터블 오브젝트의 타입에 따라 tip의 컬러를 조정해 줄 예정 
                tipMaterial.color = paintTypeManager.GetColorByType(currentPaintType);
            }
            drawingLayer = LayerMask.GetMask("DrawBoard");

            startPosition = transform.position;
            startRotation = transform.rotation;

        }

        private void Update()
        {
            
            if (isSelected == true)
            {
                StartDrawing(); // 오브젝트가 잡혀 있는 상황이라면 true로 지속 
            }

            if (isDrawing && currentPaintType != PaintTypeEnum.None) // 잡고 있을 때만 
            {
                Draw();
            }
        }

        public void Draw()
        {

            RaycastHit hit;

            if (Physics.Raycast(tip.position, tip.forward, out hit, distance, drawingLayer))
            {
                Debug.DrawRay(tip.position, tip.forward * distance, Color.red, 0.5f);

                Vector3 drawPosition = hit.point + hit.normal * NormalDis;
                drawManager = hit.collider?.GetComponent<DrawObjectManager>();

                if (!CheckColorType(drawManager)) // 컬러 타입이 같을 때만 그릴 수 있게 컬러타입을 체크 해줘야한다.
                {
                    DrawingStop();
                    return;
                }
                if (currentDrawing == null) //이 부분에서 현재 물감에 알맞는 색상으로 만들어줘야 할 것 같아. 
                {
                    index = 0;

                    // 여기부분을 미리 준비해둔 LineRenderer 붙인 프리팹을 pooling 해둔다음에
                    // material 이나 color나 이런것들 세팅 해주고
                    // 생성 위치 같은 경우는 어차피 SetPosition 그대로 하고 있으니까 pooling 으로 바꿔보자. 

                    GameObject lineObj = new GameObject("Line");

                    lineObj.transform.position = tip.position;
                    currentDrawing = lineObj.AddComponent<LineRenderer>();

                    currentDrawing.material = new Material(drawingMaterial);

                    // 현재 색상 설정
                    currentDrawing.material.color = paintTypeManager.GetColorByType(currentPaintType); 

                    // 현재 색상 설정
                    currentDrawing.startColor = currentDrawing.endColor = paintTypeManager.GetColorByType(currentPaintType);

                    currentDrawing.startWidth = currentDrawing.endWidth = penWidth; //일정한 굵기. 

                    currentDrawing.positionCount = 1; // 시작 포지션 카운트 1

                    currentDrawing.SetPosition(0, drawPosition);

                    drawManager.AddLineRenderer(currentDrawing, penWidth);

                    lineList.Add(lineObj);

                    //Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
                }
                else // 즉 이미 생성된 경우. 
                {
                    var currentPos = currentDrawing.GetPosition(index);
                    currentDrawing.material.color = paintTypeManager.GetColorByType(currentPaintType);

                    if (Vector3.Distance(currentPos, drawPosition) > 0.01f)
                    {
                        index++;
                        currentDrawing.positionCount = index + 1;

                        currentDrawing.SetPosition(index, drawPosition);
                        drawManager.AddLineRenderer(currentDrawing, penWidth);

                        //Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
                    }
                }
                // 이 부분이 완성된 상태니까. 여기서 추가 함수를 불러서 실제 이미지를 On 해주고 
                // 더이상 그려지지 않는 작업을 추가해주고
                // 
                if (drawManager.GetFillPercentage() >= percent) // 다 찬 상황일 때 플레이어를 해방시켜 줘야함. (어차피 한 부분만 해결해도 탈출 가능함 ) 
                {
                    if (isNotEntered)
                    {
                        return;
                    }

                    CompleteDrawing();
                }
            }
            else  // RayCast가 닿지 않을 때도 stop 이후에 다시 그리게 되면 너무 line Renderer가 많이 생성되는 문제가 발생한다. 
            {
                DrawingStop();
                
            }
        }
        private void CompleteDrawing()
        {
            isNotEntered = true;
            DrawingStop();
            drawManager.ImageAlphaUp();
            RemoveALLLine();
            drawManager.DrawFinished();
            StartCoroutine(blockRoutine());
        }

        private IEnumerator blockRoutine()
        {

            yield return new WaitForSeconds(1f);
            isNotEntered = false;
            yield return null;

        }

        private bool CheckColorType(DrawObjectManager drawObjectManager)
        {
            if (drawObjectManager == null)
            {
                return false;
            }

            return drawObjectManager.ObjectMyColor == paintTypeManager.GetColorByType(currentPaintType);

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

        // 이 부분은 그냥 잘 바뀌나 확인용으로 둔 함수 --> 실제 사용 x 
        /*public void SwitchColor()  // 색상 전환은 일단 나중에.
        {
            // PaintTypeEnum의 모든 값을 배열로 가져옵니다.
            PaintTypeEnum[] paintTypes = (PaintTypeEnum[])System.Enum.GetValues(typeof(PaintTypeEnum));

            // 현재 색상 타입의 인덱스를 배열에서 찾습니다.
            int currentIndex = System.Array.IndexOf(paintTypes, currentPaintType);

            // 인덱스를 하나 증가시킵니다. 배열의 끝에 도달하면 다시 처음으로 순환합니다.
            currentIndex = (currentIndex + 1) % paintTypes.Length;

            // 증가된 인덱스를 사용하여 새로운 색상 타입을 설정합니다.
            currentPaintType = paintTypes[currentIndex];

            // 새로운 색상 타입에 해당하는 색상을 가져와 펜촉의 마테리얼에 적용합니다.
            tipMaterial.color = paintTypeManager.GetColorByType(currentPaintType);

            DrawObjectManager.colorChangeEvent?.Invoke(currentPaintType);

        }*/

        // 실제로 색깔 변경을 위해 사용 할 함수
        public void ChangeColor(PaintTypeEnum _paintTypeEnum)
        {
            ChangeColorAllStop(); // 일단 이전 버전을 다 지우고 시작하는게 맞을 듯 ? 
            currentPaintType = _paintTypeEnum;
            tipMaterial.color = paintTypeManager.GetColorByType(currentPaintType);

            DrawObjectManager.colorChangeEvent.Invoke(currentPaintType);
            
            // 이게 컬러가 체인지 될 때 이미 있는 라인 렌더러가 색이 변해버리는데 그거를 해결하려면 어떻게 해야할지 생각해보자. 

        }

        //한 라인 씩 Undo 할 필요는 없을 듯 함. --> 한 번에 라인 삭제 가능한 함수. 
        public void RemoveALLLine()
        {
            if (lineList.Count == 0) return;

            for (int i = lineList.Count - 1; i >= 0; i--)
            {
                LineRenderer lineObj = lineList[i]?.GetComponent<LineRenderer>();
                drawManager?.RemoveLineRenderer(lineObj);

                Destroy(lineObj.gameObject);  // 이 Remove All 도 Destroy 대신에 pooling 적용하기. 
                //lineList.RemoveAt(i);
            }
            lineList.Clear();
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            StartDrawing();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {

            base.OnSelectExited(args);
            Debug.Log("펜 셀렉트 엑시트");
            transform.position = startPosition;
            transform.rotation = startRotation;

            DrawingStop();
            RemoveALLLine(); //라인 다 지우고
                             // 해당하고 있는 DrawObject의 퍼센트를 초기화한다. --> 라인이 그려져 있는 DrawManager가 뭔지를 알고 있어야 하는데... 

            // 채워진 양 초기화 시키기. --> 어차피 놓는 순간까지는 잡고 있을 거니까.. 아마도 그냥 빼면 될 듯 
            // ray를 소던 마지막 drawmanager의 함수 발동
            if (drawManager != null)
            {
                drawManager.PercentReset();
            }
            
        }

        // ChangeColor 할 때 불러줄 함수 
        public void ChangeColorAllStop() // 어차피 지금 같은 색깔 일 때만 drawmanager에 저장이 되니까 이거를 
        {
            DrawingStop();
            RemoveALLLine();
            if(drawManager!=null)
            {
                drawManager.PercentReset(); 
            }
        }
    }
}



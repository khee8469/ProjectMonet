using System.Collections;
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

        /*[Tooltip("펜의 색상 배열")] //추후에 이 리스트를 이용해서 아이템과 연계로 색 변화 발생시키기? 이 부분은 나중에 추가로 생각해보기. 
        public List<Color> penColors = new List<Color>();*/

        [Header("렌더러와 컬러 관리")]
        [Tooltip("그려줄 라인 렌더러")]
        [SerializeField] private LineRenderer currentDrawing;

        /* [Tooltip("컬러 리스트의 인덱스")] // 이거 리스트 말고 딕셔너리로 해야하나? 컬러 색깔 구분해 줄 때 뭐가 편할지 생각해보자. 
         [SerializeField] private int index;

         [Tooltip("현재 컬러 인덱스")]
         [SerializeField] int currentColorIndex;*/

        [Header("스크립터블 오브젝트 관련")]
        [Tooltip("현재 컬러 타입")]
        [SerializeField] private PaintTypeEnum currentPaintType;

        [Tooltip("색상 데이터를 관리하는 스크립터블 오브젝트")]
        public PaintTypeManager paintTypeManager;


        [Tooltip("그리기 상태 관리")]
        [SerializeField] private bool isDrawing = false;

        [Header("플레이어의 움직임 방지(그림그리는 중)")]
        [SerializeField] private bool isNotMove = false;

        [Header("상호작용 오브젝트 관리")]
        [Tooltip("그리기를 허용할 레이어 마스크")]

        [SerializeField] private LayerMask drawingLayer;
        [Tooltip("레이어 체크 거리")]
        private float distance = 3f;

        [Header("삭제 및 이미지 연계")]
        [Tooltip("생성된 라인렌더러를 저장 해 줄 리스트")]
        [SerializeField] List<GameObject> lineList = new List<GameObject>();

        [Tooltip("해당 캔버스와 관련된 참조")]
        private DrawObjectManager drawManager;

        [Tooltip("Noraml 벡터 크기")]
        private float NormalDis = 0.01f;

        [Tooltip("원하는 완료 퍼센트")]
        [SerializeField] private float percent = 5;

        [Tooltip("라인렌더러의 포지션 위한 인덱스")]
        [SerializeField] private int index;


        [Header("레이캐스트 박스 설정")]
        [Tooltip("박스의 크기")]
        public Vector3 boxSize = new Vector3(0.4f, 0.4f, 0.4f);
        [Tooltip("박스의 방향")]
        public Quaternion boxOrientation = Quaternion.identity;

        [Tooltip("update 여러번 진입 방지를 위한 bool 변수")]
        [SerializeField] private bool isNotEntered;



        private void Start() // 시작 시에는 무조건 하얀색. 
        {
            currentPaintType = PaintTypeEnum.None; //기본 색상으로 지정해주기. 

            if (paintTypeManager != null)
            {
                // 스크립터블 오브젝트의 타입에 따라 tip의 컬러를 조정해 줄 예정 
                tipMaterial.color = paintTypeManager.GetColorByType(currentPaintType);
            }
            drawingLayer = LayerMask.GetMask("DrawBoard");

        }

        // 지금 update 없이 xrBase의 update 용 콜백을 받아도 제대로 동작이 안해서 이 부분 나중에 시간나면 수정하기. 
        // 레이캐스트를 계속 체크해야 하기 때문에 update 밖에 없나? 어떻게 해야할지... 
        private void Update()
        {
            // 컬러의 타입이 None이 아니고 동시에 isDrawing 상태면 그리기 가능. 


            if (isDrawing && currentPaintType != PaintTypeEnum.None)
            {
                Draw();
                
            }
        }

        public void Draw()
        {
            if (!isDrawing || currentPaintType == PaintTypeEnum.None) return; // 그리기 상태가 아니면 리턴 

            RaycastHit hit;

            // 레이 캐스트 박스의 센터 
            Vector3 boxCenter = tip.position;

            if (Physics.BoxCast(boxCenter, boxSize, tip.forward, out hit, boxOrientation, distance, drawingLayer))
            {
                Debug.DrawRay(tip.position, tip.forward * distance, Color.red);

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

                    Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
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

                        Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
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
                    Debug.Log("퍼센트 완료");
                    isNotEntered = true;
                    DrawingStop();
                    drawManager.ImageAlphaUp();
                    RemoveALLLine();
                    isNotMove = false;
                    PlayerNotMove(isNotMove);
                    drawManager.DrawFinished(); // 이거는 지금 완전히 완성된 상태를 체크하게 되고 있는 것 같은대.. 

                    StartCoroutine(blockRoutine());
                    
                }
            }
            else
            {
                DrawingStop();
            }
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
            isNotMove = true;
            PlayerNotMove(isNotMove);

            


            // not move 와 함께 --> 플레이어의 움직임 막아버리는 함수 발동 

        }

        private void PlayerNotMove(bool isNotMove)
        {
            if (isNotMove)  // true면 움직임 방지 
            {
                
            }
            else
            {
                
            }
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
        public void SwitchColor()  // 색상 전환은 일단 나중에.
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

        }

        public void ChangeColor(PaintTypeEnum _paintTypeEnum)
        {
            currentPaintType = _paintTypeEnum;
            Debug.Log($"색깔 변경 +{_paintTypeEnum} ");
            tipMaterial.color = paintTypeManager.GetColorByType(currentPaintType);
        }

        //한 라인 씩 Undo 할 필요는 없을 듯 함. --> 한 번에 라인 삭제 가능한 함수. 
        public void RemoveALLLine()
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



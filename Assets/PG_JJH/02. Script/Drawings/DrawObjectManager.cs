using System.Collections.Generic;
using UnityEngine;

namespace JJH  // 캔버스(그림 그려지는 곳 ) 에 붙을 스크립트. --> 차라리 진짜 이미지에 붙이는 방법으로 가보자. 
{
    public enum DrawBoardNumber
    {
        Stage1, Stage2, Stage3, Stage4
    }


    public enum ColorDrawType
    {
        Color1, Color2, Color3, Color4, END
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class DrawObjectManager : MonoBehaviour
    {

        public List<LineRenderer> lineRenderers = new List<LineRenderer>();

        [Header("캔버스 구분 열겨형 변수")]
        [Tooltip("스테이지 별 캔버스 구분")]
        [SerializeField] private DrawBoardNumber drawBoardNumber;

        [Tooltip("그림판의 타입-- 색상 정하기.")]
        [SerializeField] private ColorDrawType colorDrawType;

        [Header("각 드로우판의 컬러타입 지정")]
        [Tooltip("각 컬러타입에 맞는 마테리얼 color만 색칠 할 수 있도록")]
        [SerializeField]
        public List<Color> boardColorTypeList = new List<Color>();

        [Tooltip("자신의 컬러타입")]
        [SerializeField]
        private Color myColor;

        public Color ObjectMyColor { get { return myColor; } private set { myColor = value; } }

        [Header("스프라이트 이미지 관련")]
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Texture2D texture;
        [Tooltip("스프라이트의 실제 월드 크기의 가로")]
        [SerializeField] private float worldWidth;
        [Tooltip("스프라이트 실제 월드 크기의 세로")]
        [SerializeField] private float worldHeight;

        [Tooltip("채워진 부분")]
        [SerializeField] private float filledArea = 0f;
        [Tooltip("전체 구역의 크기")]
        [SerializeField] private float totalArea = 0f;



        private void Start()
        {
            // 임시로 start에서 색깔 지정해주자. 
            boardColorTypeList.Add(new Color32(255, 0, 0, 255));
            boardColorTypeList.Add(new Color32(0, 255, 0, 255));
            boardColorTypeList.Add(new Color32(0, 0, 255, 255));
            boardColorTypeList.Add(new Color32(255, 255, 0, 255));

            switch (colorDrawType)
            {
                case ColorDrawType.Color1:
                    myColor = boardColorTypeList[0];
                    break;
                case ColorDrawType.Color2:
                    myColor = boardColorTypeList[1];
                    break;
                case ColorDrawType.Color3:
                    myColor = boardColorTypeList[2];
                    break;
                case ColorDrawType.Color4:
                    myColor = boardColorTypeList[3];
                    break;
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                texture = spriteRenderer.sprite.texture;
            }

            InitializeSpriteSize(); // 시작 시의 각자의 로컬 스케일 적용된 크기를 가져온다. 

            totalArea = worldHeight * worldWidth;

        }

        //라인 렌더러를 리스트에 추가하는 함수
        public void AddLineRenderer(LineRenderer lineRenderer, float penWidth)
        {
            lineRenderers.Add(lineRenderer);
            UpdateFilledArea(lineRenderer, penWidth);
            


        }

        // 라인 렌더러를 리스트에서 제거하는 함수
        public void RemoveLineRenderer(LineRenderer lineRenderer)
        {
            lineRenderers.Remove(lineRenderer);

        }

        private void InitializeSpriteSize()
        {
            // 텍스처의 크기 가져오기
            float textureWidth = spriteRenderer.sprite.rect.width;
            float textureHeight = spriteRenderer.sprite.rect.height;

            // 월드 크기로 변환 
            worldWidth = textureWidth / spriteRenderer.sprite.pixelsPerUnit;
            worldHeight = textureHeight / spriteRenderer.sprite.pixelsPerUnit;

            // 스케일 적용

            worldWidth *= spriteRenderer.transform.localScale.x;
            worldHeight *= spriteRenderer.transform.localScale.y;

        }

        // 월드 좌표를 텍스처 픽셀 좌표로 변환하는 함수
        public Vector2Int WorldToPixel(Vector3 worldPosition)
        {
            // 월드 좌표를 로컬 좌표로 변환
            Vector3 localPos = spriteRenderer.transform.InverseTransformPoint(worldPosition);

            // 스프라이트의 피벗 및 스케일 적용
            Vector2 pivot = spriteRenderer.sprite.pivot;

            // 로컬 좌표를 픽셀 좌표로 변환
            Vector2 pixelPos = new Vector2(
                (localPos.x * spriteRenderer.sprite.pixelsPerUnit) + pivot.x,
                (localPos.y * spriteRenderer.sprite.pixelsPerUnit) + pivot.y
            );

            // 픽셀 좌표를 반올림하여 정수 좌표로 변환
            Vector2Int roundedPixelPos = new Vector2Int(
                Mathf.RoundToInt(pixelPos.x),
                Mathf.RoundToInt(pixelPos.y)
            );

            return roundedPixelPos;
        }

        private bool IsPixelWithinTexture(Vector2Int pixel)
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null)
            {
                return false;
            }

            Texture2D texture = spriteRenderer.sprite.texture;
            bool withinBounds = pixel.x >= 0 && pixel.x < texture.width && pixel.y >= 0 && pixel.y < texture.height;
            Debug.Log($"Pixel Position: {pixel}, Within Texture Bounds: {withinBounds}");

            return withinBounds;
        }

        // 새로운 라인 렌더러의 영역을 계산하여 업데이트하는 함수 

        private void UpdateFilledArea(LineRenderer lineRenderer , float penWidth)
        {

            Debug.Log(lineRenderer.positionCount);
            

            for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            {

                Vector3 start = lineRenderer.GetPosition(i);
                Vector3 end = lineRenderer.GetPosition(i + 1);

                float width = penWidth;  // 라인 렌더러의 너비를 사용
                float segmentArea = CalculateSegmentArea(start, end, width);

                

                filledArea += segmentArea;
            }

            Debug.Log($"Filled Area: {filledArea}, Total Area: {totalArea}, Fill Percentage: {filledArea / totalArea * 100}%");
        }

        // 두 점과 너비 사이를 사용하여 영역을 계산하는 함수

        private float CalculateSegmentArea(Vector3 start, Vector3 end, float width)
        {
            float length = Vector3.Distance(start, end);

            return length * width;
        }

        // 스프라이트의 채워진 비율을 반환하는 함수
        public float GetFillPercentage()
        {
            return filledArea / totalArea * 100f;
        }

    }

}



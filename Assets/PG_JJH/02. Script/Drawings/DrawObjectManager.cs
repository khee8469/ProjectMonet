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

            


        }

        //라인 렌더러를 리스트에 추가하는 함수
        public void AddLineRenderer(LineRenderer lineRenderer)
        {
            lineRenderers.Add(lineRenderer);

        }

        // 라인 렌더러를 리스트에서 제거하는 함수
        public void RemoveLineRenderer(LineRenderer lineRenderer)
        {
            lineRenderers.Remove(lineRenderer);

        }

        // 스프라이트의 크기를 계산하는 함수 
        public Vector2 GetSpriteSize()
        {
            if (spriteRenderer == null)
            {
                Debug.Log("스프라이트 없음");
                return Vector2.zero;
            }

            return new Vector2(spriteRenderer.sprite.rect.width,
                spriteRenderer.sprite.rect.height);
        }

        // lineRenderer로 얼마나 채워졌는지를 체크.
        public float CalculateFilledArea()
        {
            if(texture==null)
            {
                Debug.Log("텍스처 설정 안됨.");
                return 0f;
            }

            int totalPixels = texture.width * texture.height;
            int drawnPixels = 0; // 그려진 픽셀 일단 0 으로 시작.

            // 각 픽셀을 검사하여 그려진 부분을 계산. 
            // 모든 line의 모든 position을 계산하는 방식
            foreach( var lineRenderer in lineRenderers)
            {
                for(int i=0;i <lineRenderer.positionCount -1;i++)
                {
                    Vector3 start = lineRenderer.GetPosition(i);
                    Vector3 end = lineRenderer.GetPosition(i + 1);

                    // start 와 end 사이의 픽셀을 계산 
                    drawnPixels += GetDrawnPixelsBetweenPoints(start, end);
                }
            }

            return (float)drawnPixels / totalPixels;
        }

        private int GetDrawnPixelsBetweenPoints(Vector3 start , Vector3 end)
        {
            int drawnPixels = 0;

            // 선형 보간으로 두 점 사이의 픽셀을 추적
            float distance = Vector3.Distance(start, end);
            int steps = Mathf.CeilToInt(distance * 10); // 해상도 조절 100 -> 10으로 낮춤 
            // 얼마나 세밀하게 샘플링 할지를 결정 --> 값이 높아질 수록 픽셀 위치 추적이 더 세밀하게 계산됨. 

            // 각 단계마다 점을 샘플링하여 그려진 픽셀 수를 계산
            for(int i=0; i<= steps; i++)
            {
                float t = i / (float)steps;
                Vector3 point = Vector3.Lerp(start, end, t);

                // 보간된 점을 픽셀 좌표로 변환
                Vector2Int pixel = WorldToPixel(point);

                if(IsPixelWithinTexture(pixel))
                {
                    drawnPixels++;
                }

                return drawnPixels;

            }


        }

        //월드 좌표를 픽셀 좌표로 변환하는 함수 
        private Vector2Int WorldToPixel(Vector3 worldPosition)
        {
            Vector2 localPos = spriteRenderer.transform.InverseTransformPoint(worldPosition);
            Vector2 spriteSize = spriteRenderer.sprite.rect.size;
            Vector2 pivot = spriteRenderer.sprite.pivot;
        }

        //픽셀이 텍슻퍼 범위 내에 있는지 확인하는 함수 
        private bool IsPixelWithinTexture(Vector2Int pixel)
        {

        }

        // 목표 이상 픽셀이 채워지면 부를 함수. 
        public bool IsFilledMoreThan(float percentage)
        {
            return CalculateFilledArea() > percentage;
        }

    }

}



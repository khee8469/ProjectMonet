using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJH  // 캔버스(그림 그려지는 곳 ) 에 붙을 스크립트. --> 차라리 진짜 이미지에 붙이는 방법으로 가보자. 
{
    // 자신의 알파값이 1F로 증가할 때 같은 ENUM인 친구들을 찾아서 걔네도 같이 알파값을 업데이트 해줘야한다. 
    public enum DrawBoardNumber
    {
        Compartment1, Compartment2, Compartment3, Compartment4, END
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
        [Tooltip("스프라이트의 전체 구역의 크기")]
        [SerializeField] private float totalArea = 0f;
        [Tooltip("투명한 부분을 제외한 구역의 크기")]
        [SerializeField] private float nonTransparentArea;

        [Tooltip("동일 부분을 체크할 해시셋")]
        [SerializeField]
        private HashSet<Vector2Int> checkPointsSet =
            new HashSet<Vector2Int>();
        [Tooltip("좌표 보정 값 float값 보정위함")]
        [SerializeField] private float tolerance = 0.02f;

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

                if (!texture.isReadable)
                {
                    
                    MakeTextureReadable(ref texture);
                }

            }


            InitializeSpriteSize(); // 시작 시의 각자의 로컬 스케일 적용된 크기를 가져온다. 
            totalArea = worldHeight * worldWidth;
            nonTransparentArea = CalculateNonTransparentArea();


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
            // 여기서 리스트 리무브 대신 발동  -->리스 삭제를 코루틴 내부로 이동 
            LineRemove(lineRenderer);
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

        private void UpdateFilledArea(LineRenderer lineRenderer, float penWidth)
        {
            for (int i = 0; i < lineRenderer.positionCount - 1; i++)
            {
                Vector3 start = lineRenderer.GetPosition(i);
                Vector3 end = lineRenderer.GetPosition(i + 1);

                Vector2Int normalizedStart = NormalizePoint(start, tolerance);
                Vector2Int normalizedEnd = NormalizePoint(end, tolerance);

                if (checkPointsSet.Contains(normalizedStart) && checkPointsSet.Contains(normalizedEnd))
                {
                    continue;
                }

                checkPointsSet.Add(normalizedStart);
                checkPointsSet.Add(normalizedEnd);

                float width = penWidth;  // 라인 렌더러의 너비를 사용
                float segmentArea = CalculateSegmentArea(start, end, width);

                filledArea += segmentArea;
            }

            Debug.Log($"Filled Area: {filledArea}, Total Area: {totalArea}, Fill Percentage: {filledArea / totalArea * 100}%");
        }

        private Vector2Int NormalizePoint(Vector3 point, float tolerance)
        {
            return new Vector2Int(Mathf.RoundToInt(point.x / tolerance),
                Mathf.RoundToInt(point.y / tolerance));


            // point.x 가 0.15 일 때 tolerance가 0.1 이면 0.15/0.1 -> 1.5 이므로 반올림int 하면 2가 된다.
            // point.x가 0.25 일 때 0.25 /0.1 이면 반올림int 하면 2가 된다. --> 0.15 와 0.25 가 같은 값이라고
            // 판단이 가능해진다. 대박사건!

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

        public void ImageAlphaUp()
        {
            Debug.Log("이미지 알파 업 함수발동");
            StartCoroutine(StartAlphaRoutine());

        }

        public void LineRemove(LineRenderer lineRenderer)
        {
            Debug.Log("라인 렌더러 삭제 함수 발동");
            StartCoroutine(RendererAlphaRoutine(lineRenderer));
        }


        private IEnumerator StartAlphaRoutine()
        {
            DrawObjectManager[] drawingBoards = FindObjectsOfType<DrawObjectManager>();

            //현재 오브젝트의 스크립트 가져오기
            DrawObjectManager currentDrawObjectManager = GetComponent<DrawObjectManager>();

            foreach (DrawObjectManager drawObjectManager in drawingBoards)
            {
                if(currentDrawObjectManager!=null && drawObjectManager.drawBoardNumber == currentDrawObjectManager.drawBoardNumber)
                {
                    StartCoroutine(SpriteAlphaUpRoutine(drawObjectManager));
                }
                
            }

            yield return null;
        }

        private IEnumerator SpriteAlphaUpRoutine(DrawObjectManager drawObjectManager)
        {
            SpriteRenderer spriteRenderer = drawObjectManager.GetComponent<SpriteRenderer>();
            Color spriteColor = spriteRenderer.color;

            float duration = 2f; //2초간 알파값 변경 진행
            float elaspedTime = 0f;

            while (elaspedTime < duration)
            {
                elaspedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elaspedTime / duration); // 0에서 1까지 점진적으로 증가

                spriteColor.a = alpha;
                spriteRenderer.color = spriteColor;
                yield return null;
            }

            spriteColor.a = 1f;
            spriteRenderer.color = spriteColor;
            drawObjectManager.gameObject.layer = 0;

        }

        // 해당 오브젝트 뿐만이 아닌.. 같은 id? 등을 가진 다른 오브젝트가 있으면 걔네도 켜줘야함.

        private IEnumerator RendererAlphaRoutine(LineRenderer lineRenderer)
        {
            // 생성된 라인렌더러의 마테리얼을 복제하여 생성 --> 원본 마테리얼에 영향이 가지 않도록
            Material materialInstance = Instantiate(lineRenderer.material);
            lineRenderer.material = materialInstance; //생성한 마테리얼로 변경 

            float duration = 2f; //2초간 알파값 변경 진행
            float elaspedTime = 0f;

            Color materialColor = materialInstance.color;

            // 초기 알파값
            float initialAlpha = materialColor.a;
            // 최종 알파값 (예: 0으로 설정하여 투명하게 만들기)
            float targetAlpha = 0f;


            while (elaspedTime < duration)
            {
                elaspedTime += Time.deltaTime;
                float t = elaspedTime / duration;

                // 알파값 보간
                float newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, t);
                materialColor.a = newAlpha;
                materialInstance.color = materialColor;

                yield return null;
            }

            // 최종 알파값 설정
            materialColor.a = targetAlpha;
            materialInstance.color = materialColor;


            lineRenderers.Remove(lineRenderer);

            yield return null;
        }




        private float CalculateNonTransparentArea()
        {

            Color32[] pixels = texture.GetPixels32();

            int trasparentPixelCount = 0;

            foreach (var pixel in pixels)
            {
                if (pixel.a == 0) //알파값이 0 이면 완전히 투명한 픽셀. 
                {
                    trasparentPixelCount++;
                }
            }

            // 투명하지 않은 픽셀의 숫자를 계산
            float nonTransparentPixelCount = pixels.Length - trasparentPixelCount;

            // 스프라이트의 전체 영역 계산
            float spriteArea = worldWidth * worldHeight;

            // 투명하지 않은 부분의 실제 영역 계산 

            float realArea = spriteArea * (nonTransparentPixelCount / pixels.Length);
            return realArea;

        }

        private void MakeTextureReadable(ref Texture2D texture)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(
         texture.width,
         texture.height,
         0,
         RenderTextureFormat.Default,
         RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, renderTexture);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D readableTexture = new Texture2D(texture.width, texture.height);
            readableTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            readableTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);

            texture = readableTexture;
        }


    }

}



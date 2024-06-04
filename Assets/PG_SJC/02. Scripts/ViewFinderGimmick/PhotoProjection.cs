using System.Collections;
using UnityEngine;


namespace Jc
{
    public class PhotoProjection : MonoBehaviour
    {

        // 사진을 투영할 Quad 프리팹
        // 캡쳐한 사진을 투영
        public GameObject photoQuadPrefab;
        // 사진을 표시할 머티리얼
        public Material photoMaterial; 

        [SerializeField]
        private PhotoCapture photoCapture;

        // 사진을 투영 및 오브젝트 생성 메서드
        public void ProjectPhoto(Texture2D photo, Vector3 position, Quaternion rotation)
        {
            // 사진을 투영할 Quad 생성
            GameObject quad = Instantiate(photoQuadPrefab, position, rotation);
            quad.GetComponent<Renderer>().material = photoMaterial;
            quad.GetComponent<Renderer>().material.mainTexture = photo;

            // 투영할 영역의 경계를 계산
            Bounds bounds = quad.GetComponent<MeshRenderer>().bounds;

            // 경계를 기반으로 오브젝트 컬링
            Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                Destroy(collider.gameObject);
            }

            // 캡처된 사진을 기반으로 새 메쉬 생성
            CreateMeshFromPhoto(photo, bounds, position, rotation);
        }

        private void CreateMeshFromPhoto(Texture2D photo, Bounds bounds, Vector3 position, Quaternion rotation)
        {
            // 메쉬를 생성할 빈 GameObject를 생성합니다.
            GameObject meshObject = new GameObject("PhotoMesh");
            meshObject.transform.position = position;
            meshObject.transform.rotation = rotation;

            // 메쉬 필터와 메쉬 렌더러를 추가합니다.
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            // 메쉬 렌더러의 머티리얼과 텍스처를 설정합니다.
            meshRenderer.material = photoMaterial;
            meshRenderer.material.mainTexture = photo;

            // 새 메쉬를 생성합니다.
            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            int width = photo.width; // 사진의 너비
            int height = photo.height; // 사진의 높이
            float quadWidth = bounds.size.x; // Quad의 너비
            float quadHeight = bounds.size.y; // Quad의 높이

            // 정점 배열과 UV 배열을 생성합니다.
            Vector3[] vertices = new Vector3[width * height];
            Vector2[] uv = new Vector2[width * height];
            // 삼각형 배열을 생성합니다.
            int[] triangles = new int[(width - 1) * (height - 1) * 6];

            // 각 픽셀에 대해 정점을 생성하고 UV 좌표를 설정합니다.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x; // 정점의 인덱스
                    float worldX = (x / (float)width) * quadWidth; // 월드 좌표로 변환된 X 값
                    float worldY = (y / (float)height) * quadHeight; // 월드 좌표로 변환된 Y 값

                    vertices[index] = new Vector3(worldX, worldY, 0); // 정점 위치 설정
                    uv[index] = new Vector2(x / (float)width, y / (float)height); // UV 좌표 설정
                }
            }

            // 삼각형 인덱스를 설정합니다.
            int triangleIndex = 0;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int currentIndex = y * width + x;
                    int nextRowCurrentIndex = (y + 1) * width + x;
                    int nextRowNextIndex = (y + 1) * width + (x + 1);
                    int nextIndex = y * width + (x + 1);

                    // 첫 번째 삼각형
                    triangles[triangleIndex++] = currentIndex;
                    triangles[triangleIndex++] = nextRowCurrentIndex;
                    triangles[triangleIndex++] = nextRowNextIndex;

                    // 두 번째 삼각형
                    triangles[triangleIndex++] = currentIndex;
                    triangles[triangleIndex++] = nextRowNextIndex;
                    triangles[triangleIndex++] = nextIndex;
                }
            }

            // 메쉬의 정점, UV 및 삼각형 데이터를 설정합니다.
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            // 노멀을 재계산합니다.
            mesh.RecalculateNormals();
        }
    }
}
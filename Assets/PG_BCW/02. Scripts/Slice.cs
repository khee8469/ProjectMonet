using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public GameObject objectToSlice; // 슬라이스할 오브젝트
    public Plane slicingPlane; // 절단면

    void Start()
    {
        // 절단면을 초기화합니다.
        slicingPlane = new Plane(Vector3.up, objectToSlice.transform.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SliceMesh();
        }
    }

    void SliceMesh()
    {
        MeshFilter meshFilter = objectToSlice.GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        Mesh originalMesh = meshFilter.mesh;
        List<Vector3> vertices = new List<Vector3>(originalMesh.vertices);
        List<int> triangles = new List<int>(originalMesh.triangles);

        List<Vector3> leftVertices = new List<Vector3>();
        List<Vector3> rightVertices = new List<Vector3>();

        List<int> leftTriangles = new List<int>();
        List<int> rightTriangles = new List<int>();

        // 절단면을 기준으로 버텍스를 분류합니다.
        for (int i = 0; i < vertices.Count; i++)
        {
            //절단면 위면 true
            if (slicingPlane.GetSide(vertices[i]))
            {
                rightVertices.Add(vertices[i]);
            }
            else
            {
                leftVertices.Add(vertices[i]);
            }
        }

        // 슬라이싱된 메시를 생성합니다.
        CreateMeshPart(leftVertices, leftTriangles, "LeftPart");
        CreateMeshPart(rightVertices, rightTriangles, "RightPart");

        Destroy(objectToSlice);
    }

    void CreateMeshPart(List<Vector3> vertices, List<int> triangles, string name)
    {
        GameObject newObject = new GameObject(name);
        Mesh newMesh = new Mesh();
        newMesh.SetVertices(vertices);
        newMesh.SetTriangles(triangles, 0);

        MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
        meshFilter.mesh = newMesh;

        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.material = objectToSlice.GetComponent<MeshRenderer>().material;

        newObject.transform.position = objectToSlice.transform.position;
        newObject.transform.rotation = objectToSlice.transform.rotation;
    }
}

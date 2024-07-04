using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainToMesh : MonoBehaviour
{
    [SerializeField]
    public Terrain terrain;
    [SerializeField]
    TerrainData terrainData;
    [SerializeField]
    Texture2D terrainTexture;

    void Start()
    {
        terrainData = terrain.terrainData;
        terrainTexture = GenerateTerrainTexture(terrainData);
        ApplyTextureToMesh(terrainTexture);
    }

    Texture2D GenerateTerrainTexture(TerrainData terrainData)
    {
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;
        float[,,] splatmaps = terrainData.GetAlphamaps(0, 0, width, height);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = 0, g = 0, b = 0, a = 0;

                // 각 레이어의 가중치를 색상에 반영
                for (int layer = 0; layer < layers; layer++)
                {
                    Color layerColor = GetLayerColor(layer); // 레이어 색상 함수
                    r += splatmaps[y, x, layer] * layerColor.r;
                    g += splatmaps[y, x, layer] * layerColor.g;
                    b += splatmaps[y, x, layer] * layerColor.b;
                    a += splatmaps[y, x, layer] * layerColor.a;
                }

                colors[y * width + x] = new Color(r, g, b, Mathf.Clamp01(a));
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    Color GetLayerColor(int layer)
    {
        // 각 레이어의 색상을 정의합니다. 필요에 따라 변경 가능합니다.
        switch (layer)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            default: return Color.white;
        }
    }

    void ApplyTextureToMesh(Texture2D texture)
    {
        // 새로운 게임 오브젝트를 생성하고 메쉬 렌더러와 필터를 추가합니다.
        GameObject meshObject = new GameObject("TerrainMesh");
        MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();

        // 메터리얼을 생성하고 텍스처를 할당합니다.
        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = texture;
        meshRenderer.material = material;
    }
}

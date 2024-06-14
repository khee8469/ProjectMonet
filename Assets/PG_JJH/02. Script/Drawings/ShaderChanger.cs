using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public enum ObjectType  // 해당 그림의 컬러와 연계해줄 type 설정. --> 매니저로 서로 연동 할 수 있도록 해주기 
    {
        Color1, Color2, Color3, Color4, END
    }

    public class ShaderChanger : MonoBehaviour
    {
        [Header("색이 돌아올 오브젝트 체크용 변수")]
        [Tooltip("컬러에 맞춰 돌아올 오브젝트의 타입 확인")]
        private ObjectType objectType;

        [Header("마테리얼 및 렌더러 관련")]
        [Tooltip("흑백용 마테리얼")]
        [SerializeField] private Material grayMaterial;

        [Tooltip("모든 자식 오브젝트의 원래 마테리얼 배열을 저장할 딕셔너리")]
        private Dictionary<MeshRenderer, Material[]> originalMaterialsDict = new Dictionary<MeshRenderer, Material[]>();

        [Tooltip("모든 자식 오브젝트의 텍스처를 저장할 딕셔너리")]
        private Dictionary<MeshRenderer, List<Texture>> originalTexturesDict = new Dictionary<MeshRenderer, List<Texture>>();

        [Tooltip("on / off 체크")]
        [SerializeField] private bool isGray = false;

        // 체크할 메인 텍스처 속성 이름 리스트
        private readonly string mainTexturePropertyName = "_MainTex";

        private void Start()
        {
            grayMaterial = Resources.Load<Material>("GrayMaterial");

            // 모든 자식 오브젝트의 MeshRenderer 컴포넌트를 가져오기
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            // 각 MeshRenderer의 원래 materials 배열과 텍스처 속성들을 딕셔너리에 저장
            foreach (MeshRenderer meshRenderer in childRenderers)
            {
                // 원래 materials 배열을 저장
                originalMaterialsDict[meshRenderer] = meshRenderer.materials;

                // baseMap 또는 MainTextur만 가져오기 위해 하나만 가져옴 (할당할 수 있는 텍스처는 하나 이기 때문에 )
                // 원래 텍스처를 저장할 리스트
                List<Texture> textures = new List<Texture>();

                foreach (Material material in meshRenderer.materials)
                {
                    Texture texture = null;

                    if (material.HasProperty(mainTexturePropertyName))
                    {
                        texture = material.GetTexture(mainTexturePropertyName);
                    }

                    textures.Add(texture);

                }

                originalTexturesDict[meshRenderer] = textures;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleMaterialsOnOff();
            }
        }

        private void ToggleMaterialsOnOff()
        {
            if (grayMaterial != null && originalMaterialsDict.Count > 0)
            {
                if (!isGray) // 만약 토글 가능하면 유지 아니면 삭제 후 최적화 진행
                {
                    // 모든 자식 오브젝트의 Renderer의 Material을 grayMaterial로 교체하고 텍스처를 할당
                    foreach (var kvp in originalMaterialsDict)
                    {
                        MeshRenderer meshRenderer = kvp.Key;
                        Material[] newMaterials = new Material[meshRenderer.materials.Length];

                        // 딕셔너리에서 특정키에 대한 값을 가져옴
                        List<Texture> textures = originalTexturesDict[meshRenderer];

                        // 각 Material을 grayMaterial로 교체하고 텍스처 할당
                        for (int i = 0; i < meshRenderer.materials.Length; i++)
                        {
                            newMaterials[i] = new Material(grayMaterial); // grayMaterial의 인스턴스 생성

                            if (textures[i]!=null)
                            {
                                newMaterials[i].SetTexture(mainTexturePropertyName, textures[i]);
                                Debug.Log($"Assigned texture {textures[i].name} to property {mainTexturePropertyName} in material {newMaterials[i].name}.");
                            }
                        }
                        // 새로운 materials 배열을 renderer에 설정
                        meshRenderer.materials = newMaterials;
                    }

                    isGray = true;
                    Debug.Log("Materials have been replaced with grayMaterial for all child renderers.");
                }
                else
                {
                    // 모든 자식 오브젝트의 Renderer의 Material을 원래 상태로 되돌림
                    foreach (var kvp in originalMaterialsDict)
                    {
                        MeshRenderer meshRenderer = kvp.Key;

                        // 원래의 materials 배열을 Renderer에 설정
                        meshRenderer.materials = kvp.Value;
                    }

                    isGray = false;
                    Debug.Log("Materials have been reverted to original for all child renderers.");
                }
            }
        }
    }
}


/*using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    public enum ObjectType
    {
        Color1, Color2, Color3, Color4, END
    }


    public class ShaderChanger : MonoBehaviour
    {

        [Header("색이 돌아올 오브젝트 체크용 변수")]
        [Tooltip("컬러에 맞춰 돌아올 오브젝트의 타입 확인")]
        private ObjectType objectType;

        [Header("마테리얼 및 렌더러 관련")]
        [Tooltip("흑백용 마테리얼")]
        [SerializeField] private Material grayMaterial;

        [Tooltip("모든 자식 오브젝트의 원래 마테리얼 배열을 저장할 딕셔너리")]
        private Dictionary<MeshRenderer, Material[]> originalMaterialsDict =
            new Dictionary<MeshRenderer, Material[]>();

        [Tooltip("모든 자식 오브젝트의 텍스처를 저장할 딕셔너리")]
        private Dictionary<MeshRenderer, List<Texture>> originalTexturesDict =
            new Dictionary<MeshRenderer, List<Texture>>();


        [Tooltip("on / off 체크")]
        [SerializeField] private bool isGray = false;

        [Tooltip("메인텍스처 상수로 캐싱")]
        private const string MainTexProperty = "_MainTex";

        private void Start()
        {
            grayMaterial = Resources.Load<Material>("GrayMaterial");

            // 모든 자식 오브젝트의 Renderer 컴포넌트 가져오기. --> 어차피 흑백 전환은 한 오브젝트 단위이므로 
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            // r각 Renderer의 원래 materials 배열 + 텍스처를 딕셔너리에 저장함. 
            foreach (MeshRenderer meshRenderer in childRenderers)
            {
                // 원래 materials 배열을 저장
                originalMaterialsDict[meshRenderer] = meshRenderer.materials;

                // 원래 텍스처 들을 저장할 리스트 --> 딕셔너리에 넣기위한. 
                List<Texture> textures = new List<Texture>();
                foreach (Material material in meshRenderer.materials)
                {
                    // 마테리얼의 메인 텍스처를 가져와서 딕셔너리에 삽입. --> 여러개의 마테리얼이 있으면
                    // 그 각각의 마테리얼의 메인 텍스처를 가져와야 하기때문에 list 사용
                    // main texture 는 base map 과 마찬가지 ( urp / lit )
                    if (material.HasProperty(MainTexProperty))
                    {
                        textures.Add(material.mainTexture);
                    }
                    else
                    {
                        textures.Add(null); // 텍스처가 없으면 null을 추가
                    }
                }

                originalTexturesDict[meshRenderer] = textures;  // 딕셔너리의 valeu 값이 list 
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleMaterialsOnOff();
            }
        }

        private void ToggleMaterialsOnOff()
        {
            if (grayMaterial != null && originalMaterialsDict.Count > 0)
            {
                if (!isGray) // 만약 토글 가능하면 유지 아니면 삭제 후 최적화 진행 
                {
                    // 모든 자식 오브젝트의 Renderer의 Material을 grayMaterial로 교체하고 텍스처를 할당
                    foreach (var kvp in originalMaterialsDict)
                    {
                        MeshRenderer meshRenderer = kvp.Key;
                        Material[] newMaterials = new Material[meshRenderer.materials.Length];

                        // 딕셔너리에서 특정키에 대한 값을 가져옴 
                        List<Texture> textures = originalTexturesDict[meshRenderer];

                        // 각 Material을 grayMaterial로 교체하고 텍스처 할당
                        for (int i = 0; i < meshRenderer.materials.Length; i++)
                        {
                            newMaterials[i] = new Material(grayMaterial); // grayMaterial의 인스턴스 생성
                            if (textures[i] != null)
                            {
                                newMaterials[i].mainTexture = textures[i]; // 저장된 텍스처를 할당
                                Debug.Log(textures[i].name);
                                 
                            }
                        }

                        // 새로운 materials 배열을 renderer에 설정
                        meshRenderer.materials = newMaterials;

                    }

                    isGray = true;
                }
                else
                {
                    // 모든 자식 오브젝트의 Renderer의 Material을 원래 상태로 되돌림
                    foreach (var kvp in originalMaterialsDict)
                    {
                        MeshRenderer meshRenderer = kvp.Key;

                        // 원래의 materials 배열을 Renderer에 설정
                        meshRenderer.materials = kvp.Value;
                    }

                    isGray = false;
                    Debug.Log("Materials have been reverted to original for all child renderers.");
                }
            }
        }
    }
}*/
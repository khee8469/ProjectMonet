using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2SunHole : PuzzleManager , IPuzzleable
{
    // 챕터 2 의 검은 구멍에 붙여 줄 스크립트

    [Tooltip("밤 용 스카이박스")] // 어차피 기본 상태에서는 이 스카이 박스를 쓰고 있을 거니까
    [SerializeField] public Material nightSkybox;

    // 맵의 
    [Tooltip("낮 용 스카이박스 --> Maybe 태양없는 skybox ")]
    [SerializeField] public Material afternoonSkybox;

    // 완료 이벤트 발동 시키면서 마테리얼 변경해주기.

    [Tooltip("Dark 상태에서의 Directional Light")]
    [SerializeField] Light nightDirectionalLight;

    [Tooltip("dark 버전 마테리얼")]
    [SerializeField] private Material darkMaterial;

    [Tooltip("화이트 버전 마테리얼")]
    [SerializeField] private Material whiteMaterial;

    [Tooltip("보상으로 인벤토리에 넣어줄 아이템 ID")]
    [SerializeField] public int ID;

    private MeshRenderer meshRenderer;

    /*[Tooltip("새로운 마테리얼 배열 -> 넣어둔 마테리얼을 변경 해주기 위해 새롭게 생성")]
    [SerializeField]
    Material[] mats = new Material[1];*/

    private void Start()
    {
        meshRenderer= GetComponent<MeshRenderer>();
        Material[] mat = meshRenderer.materials;
        mat[0] = darkMaterial;
        meshRenderer.materials = mat;
    }

    public void ChangeSkyBox(Material _Skybox)
    {
        RenderSettings.skybox = _Skybox;
        nightDirectionalLight.gameObject.SetActive(false);
        Material[] mats = meshRenderer.materials;
        mats[0] = whiteMaterial; // 첫 번째 메터리얼을 darkMaterial로 설정
        meshRenderer.materials = mats; // 변경된 배열 다시 설정
    }



    public void ItemAdd(int ID)
    {
        if(ID != 0)
        {
            Manager.Inventory.AddItem(ID); // item 추가. 
        }
        
    }

    public override void OnClearPuzzle()
    {
        base.OnClearPuzzle();
        ChangeSkyBox(afternoonSkybox);
        
        // itemID가 일치하는 아이템을 인벤토리로 Add 하는 함수가 필요함. -> 추후 작성 

    }

    public void ActiveSetting()
    {
        
    }

    public void CompleteSetting()  // 퀘스트가 완성되어 있는 상태 --> 등대 못 만지고 
    {
        
    }

    public void DisActiveSetting()
    {
        
    }

    public void ImHitByRay()
    {

    }

    public void RegistObject(PuzzleManager puzzle)
    {
        
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        
    }
}

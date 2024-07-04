using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2SunHole : PuzzleManager , IPuzzleable
{
    // 챕터 2 의 검은 구멍에 붙여 줄 스크립트

    [Tooltip("밤 용 스카이박스")] // 어차피 기본 상태에서는 이 스카이 박스를 쓰고 있을 거니까
    [SerializeField] Material nightSkybox;

    // 맵의 
    [Tooltip("낮 용 스카이박스 --> Maybe 태양없는 skybox ")]
    [SerializeField] Material afternoonSkybox;

    // 완료 이벤트 발동 시키면서 마테리얼 변경해주기.

    [Tooltip("Dark 상태에서의 Directional Light")]
    [SerializeField] Light nightDirectionalLight;

    [Tooltip("white 상태에서의 Directional Light")]
    [SerializeField] Light afternoonDirectionalLight;

    [Tooltip("자식으로 두고 있는 자신의 발광 라이트")]
    [SerializeField] Light sunRiseLight;

    [Tooltip("dark 버전 마테리얼")]
    [SerializeField] private Material darkMaterial;

    [Tooltip("화이트 버전 마테리얼")]
    [SerializeField] private Material whiteMaterial;

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


    public override void OnClearPuzzle()
    {
        base.OnClearPuzzle();
        ChangeSkyBox(afternoonSkybox);

    }

    public void ActiveSetting()
    {
        throw new System.NotImplementedException();
    }

    public void CompleteSetting()
    {
        throw new System.NotImplementedException();
    }

    public void DisActiveSetting()
    {
        throw new System.NotImplementedException();
    }

    public void ImHitByRay()
    {

    }

    public void RegistObject(PuzzleManager puzzle)
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
    {
        throw new System.NotImplementedException();
    }
}

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

    [Tooltip("자식으로 두고 있는 directional light")]
    [SerializeField] Light nightDirectionalLight;

    [Tooltip("자식으로 두고 있는 자신의 발광 라이트")]
    [SerializeField] Light sunRiseLight;

    private void Start()
    {
        
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ChangeSkyBox(afternoonSkybox);
            nightDirectionalLight.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            ChangeSkyBox(nightSkybox);
            nightDirectionalLight.gameObject.SetActive(true); // 다시 돌아올 일은 없긴함. 

        }
    }

    
    

    public void ChangeSkyBox(Material _Skybox)
    {
        RenderSettings.skybox = _Skybox;
    }


    public override void OnClearPuzzle()
    {
        base.OnClearPuzzle();
        // 퍼즐이 클리어 될 시 스카이박스 변경 

        



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

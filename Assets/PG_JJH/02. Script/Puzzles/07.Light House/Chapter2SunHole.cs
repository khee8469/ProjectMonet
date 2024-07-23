using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2SunHole : PaintRewardPuzzle
{
    // 챕터 2 의 검은 구멍에 붙여 줄 스크립트


    // 이러면 마테리얼이 아니라 스프라이트를 바꿔 줘야 하는듯? 

    // 마테리얼을 바꿔줘야 하는 실제 오브젝트가 아니게 되었으니까 그냥 따로 빼자. 
    
    [Tooltip("낮 용 스카이박스 --> Maybe 태양없는 skybox ")]
    [SerializeField] public Material afternoonSkybox;

    [Tooltip("dark 버전 마테리얼")]
    [SerializeField] private Material darkMaterial;

    [Tooltip("화이트 버전 마테리얼")]
    [SerializeField] private Material whiteMaterial;

    [Tooltip("moon Panel 을 ON off 해 줄 콜라이더")]
    [SerializeField] private BoxCollider boxCollider;

    [Tooltip("스프라이트 -> Moon 스프라이트가 붙어 있는 부모 게임 오브젝트--> 패널과 같이 On Off")]
    [SerializeField] public GameObject moonPanel;

    [SerializeField]
    private Light directionalLight;

    /*[Tooltip("스프라이트 -> Sun 스프라이트")]
    [SerializeField] private Sprite sunSprite; // 낮 용 --> Sun 스프라이트 */

    // moon이 스프라이트 인데 갑자기 구체를 멀리 생성하는 것 도 너무 어색할 듯 함.
    // 스카이 박스만 바꾸는 거는 어떤지 싶음. -> 태양 위치를 대충이라도 맞춰두고 

    private void Start()   // 렌더러가 아니라 sprite 상태 ++ 가까이 옮겼기 때문에 ... 
    {
        //meshRenderer= GetComponent<MeshRenderer>();
        //Material[] mat = meshRenderer.materials;
        //mat[0] = darkMaterial;
        //meshRenderer.materials = mat;
        moonPanel.SetActive(false); // 일단 시작 시에는 패널을 무조건 꺼두기. 
    }

    public void OnPanelOff()
    {
        moonPanel.SetActive(false);
    }

    public override void OnClearPuzzle()
    {
        base.OnClearPuzzle();
        OnPanelOff();

        directionalLight.intensity = 1.0f;
        boxCollider.enabled = false; // 콜라이더를 꺼버려서 다시 panel이 켜지는 일을 방지한다. 
        Debug.Log("등대 온클리어");

    }


    // 그냥 퀘스트 깻을 때만 달 켜주자. 
    /*private void OnTriggerEnter(Collider other) // 플레이어 등대 문 안으로 들어오면 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            moonPanel.SetActive(true); // 내부로 들어오면 켜주기. 

            Debug.Log("플레이어 트리거 진입");
        }
    }*/

}

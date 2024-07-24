using Jc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter2SunHole : PaintRewardPuzzle
{
    // 챕터 2 의 검은 구멍에 붙여 줄 스크립트


    // 이러면 마테리얼이 아니라 스프라이트를 바꿔 줘야 하는듯? 

    // 마테리얼을 바꿔줘야 하는 실제 오브젝트가 아니게 되었으니까 그냥 따로 빼자. 

    /*[Tooltip("dark 버전 마테리얼")]
    [SerializeField] private Material darkMaterial;

    [Tooltip("화이트 버전 마테리얼")]
    [SerializeField] private Material whiteMaterial;

    [Tooltip("moon Panel 을 ON off 해 줄 콜라이더")]
    [SerializeField] private BoxCollider boxCollider;*/

    [Header("스카이박스 변경 사항 ")]
    [Tooltip("자신의 skyboxMaterial")]
    [SerializeField] public Material skyBoxMaterial;

    [Tooltip("스프라이트 -> Moon 스프라이트가 붙어 있는 부모 게임 오브젝트--> 패널과 같이 On Off")]
    public GameObject moonPanel;

    [SerializeField]
    [Tooltip("씬의 다이렉트 라이트 -> baking X! ")]
    private Light directionalLight;

    [Tooltip("태양 오브젝트")]
    public GameObject sunObject;


    [Header("변경 시켜 줄 스카이박스의 값")]
    [Tooltip("스카이박스 컬러 색깔 -> 어둡게 만들어주는 색깔 ")]
    [SerializeField] private Color color = new Color(176 / 255f, 176 / 255f, 176 / 255f);

    [Tooltip("스카이박스 Exposure")]
    [SerializeField] private float exposure = 1f;

    [Tooltip("어두운 상태의 스카이 박스 Color")]
    [SerializeField] private Color darkColor = new Color(35 / 255f, 35 / 255f, 35 / 255f);

    [Tooltip("어두운 상태의 Exposure")]
    [SerializeField] private float darkExposure = 0.6f;



    /*[Tooltip("스프라이트 -> Sun 스프라이트")]
    [SerializeField] private Sprite sunSprite; // 낮 용 --> Sun 스프라이트 */

    // moon이 스프라이트 인데 갑자기 구체를 멀리 생성하는 것 도 너무 어색할 듯 함.
    // 스카이 박스만 바꾸는 거는 어떤지 싶음. -> 태양 위치를 대충이라도 맞춰두고 

    private void Awake()   // 렌더러가 아니라 sprite 상태 ++ 가까이 옮겼기 때문에 ... 
    {
        //meshRenderer= GetComponent<MeshRenderer>();
        //Material[] mat = meshRenderer.materials;
        //mat[0] = darkMaterial;
        //meshRenderer.materials = mat;
        moonPanel.SetActive(false); // 일단 시작 시에는 패널을 무조건 꺼두기. 
        if(sunObject != null )
        {
            sunObject.SetActive(false);
        }
    }

    public void MoonChange(bool boolean)
    {
        if(moonPanel!=null)
            moonPanel.SetActive(boolean);     
    }
    
    public void SunChange(bool boolean)
    {
        if(sunObject!=null)
            sunObject.SetActive(boolean);
    }


    public void DarkSkyBox()
    {
        if (skyBoxMaterial != null)
        {
            skyBoxMaterial.SetColor("_Tint", darkColor);
            skyBoxMaterial.SetFloat("_Exposure", darkExposure);
        }
    }


    public override void OnClearPuzzle()
    {
        base.OnClearPuzzle();
        MoonChange(false);
        SunChange(true);

        // 완성 시에 변경 시켜 줄 스카이박스  --> complet setting starting. 
        if(skyBoxMaterial!=null)
        {
            skyBoxMaterial.SetColor("_Tint", color );
            skyBoxMaterial.SetFloat("_Exposure", exposure);
        }


        directionalLight.intensity = 1.0f;
        //boxCollider.enabled = false; // 콜라이더를 꺼버려서 다시 panel이 켜지는 일을 방지한다. 
        Debug.Log("등대 온클리어");

    }


    // 그냥 퀘스트 깻을 때만 달 켜주자. ?? 달 이나 해는 그냥 계속 떠있어야하나??
    /*private void OnTriggerEnter(Collider other) // 플레이어 등대 문 안으로 들어오면 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            moonPanel.SetActive(true); // 내부로 들어오면 켜주기. 

            Debug.Log("플레이어 트리거 진입");
        }
    }*/

}

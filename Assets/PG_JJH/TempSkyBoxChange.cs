using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSkyBoxChange : MonoBehaviour
{
    [SerializeField] private Material skyBoxMaterial;

    [Tooltip("스카이박스 컬러 색깔 -> 어둡게 만들어주는 색깔 ")]
    [SerializeField] private Color color = new Color(176 / 255f, 176 / 255f, 176 / 255f);

    [Tooltip("스카이박스 Exposure")]
    [SerializeField] private float exposure = 1f;

    [Tooltip("어두운 상태의 스카이 박스 Color")]
    [SerializeField] private Color darkColor = new Color(35 / 255f, 35 / 255f, 35 / 255f);

    [Tooltip("어두운 상태의 Exposure")]
    [SerializeField] private float darkExposure = 0.6f;

    private Material originalMaterial;




    private void Start()
    {
        originalMaterial = RenderSettings.skybox;
        Material runtimeMaterial = new Material(skyBoxMaterial); // 런타임 중 진행.
        runtimeMaterial.SetColor("_Tint", color);
        runtimeMaterial.SetFloat("_Exposure", exposure);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (skyBoxMaterial != null)
            {
                skyBoxMaterial.SetColor("_Tint", color);
                skyBoxMaterial.SetFloat("_Exposure", exposure);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (skyBoxMaterial != null)
            {
                skyBoxMaterial.SetColor("_Tint", darkColor);
                skyBoxMaterial.SetFloat("_Exposure", darkExposure);
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("디스트로이드");
        RenderSettings.skybox = originalMaterial;
    }


}

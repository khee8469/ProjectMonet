using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    // 각 챕터에 존재하는 조명 
    // 이거 그냥 serialLizedFiled로 해서

    [Tooltip("조명과 연계되어 있는 그림 조각")]
    [SerializeField] GameObject myLinkDraw;

    [Tooltip("자신의 컬러 색깔 ")]
    [SerializeField]PaintTypeManager paintTypeManager;

    [Tooltip("색깔의 스크립터블 오브젝트와 연계된 컬러")]
    [SerializeField] public PaintTypeEnum lightPaintType;

    [Tooltip("자신의 레이어 와 그림을 연계")]
    [SerializeField]LayerMask layerMask;

    [Tooltip("자신의 마테리얼 ")]
    Material material;

    [Tooltip("자신의 컬러")]
    Color my_Color;

    [Tooltip("자신의 아이디")]
    public int IightID;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        
    }
    private void Start()
    {
        // start 에서 그림이 기본 defalut고 완료된 상태라면 local volume 으로 바꿔주고 있으므로 layer를 똑같이 따라가면 된다.
        DrawObjectManager draw = myLinkDraw.GetComponent<DrawObjectManager>();
    }

    public void drawLightLink(PaintTypeEnum currentPaintType)
    {
        if(lightPaintType == currentPaintType)
        {
            my_Color = paintTypeManager.GetColorByType(currentPaintType); // 스크립터블 오브젝트로 할당되어 있음. 

            material.SetColor("_BaseColor", my_Color);

            gameObject.layer = myLinkDraw.layer; // 레이어 맞추기. --> 인스펙터에서 그림이 할당되어 있음. 
        }
    }


}

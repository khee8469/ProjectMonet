using Jc;
using JJH;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PaintBucket : InteractObject
{
    // 얘가 진짜 물감이고 paint는 생성되는 물감임. 


    [Tooltip("생성 할 물감 색상")]
    [SerializeField]
    private PaintTypeEnum color;
    [Tooltip("물감 프리펩")]
    [SerializeField]
    private Paint paint;
    [Tooltip("물감 생성 위치")]
    [SerializeField]
    private Transform attach;

    [Tooltip("자신의 아이템 ID")]
    [SerializeField] int paintItemID;
    public int PaintItemID
    {
        get { return paintItemID; }
    }

    [Tooltip("각자 자신이 가지고 있는 컬러의 상태")]
    [SerializeField] private Color myColor;

    [Tooltip("컬러 데이터 스크립터블 오브젝트")]
    public PaintTypeManager paintTypeManager;


    protected override void OnEnable() // 자신이 켜 졌을 때 
    {
        base.OnEnable();
        if (paintTypeManager != null)
        {
            myColor = paintTypeManager.GetColorByType(color);
            GetComponent<Renderer>().material.color = myColor;
        }
    }

    private void Start()
    {
        
        if (paintTypeManager != null)
        {
            myColor = paintTypeManager.GetColorByType(color);
            GetComponent<Renderer>().material.color = myColor;
        }
    }

    //물감나오는거 구현
    public void PaintPlay()
    {
        Paint paintPrefab = Instantiate(paint, attach.position, Quaternion.identity);
        paintPrefab.paintType = color;

        //Destroy(gameObject); 생성 성공하면 파괴되어야함. 
    }

    public void PaintStop()
    {
        //paintParticle.Stop();
    }


    

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        Debug.Log("엔터드");
    }


    //레이를 쏴서 아래가 팔렛트면 물감을 생성한다

}

using Jc;
using JJH;
using System.Collections;
using UnityEngine;

public class PaintBucket : InteractObject
{
    [Tooltip("생성 할 물감 색상")]
    [SerializeField]
    private PaintTypeEnum color;
    [Tooltip("물감 프리펩")]
    [SerializeField]
    private Paint paint;
    [Tooltip("물감 생성 위치")]
    [SerializeField]
    private Transform attach;


    //물감나오는거 구현
    public void PaintPlay()
    {
        Paint paintPrefab = Instantiate(paint, attach.position, Quaternion.identity);
        paintPrefab.paintType = color;
    }

    public void PaintStop()
    {
        //paintParticle.Stop();
    }



    //레이를 쏴서 아래가 팔렛트면 물감을 생성한다

}

using Jc;
using JJH;
using System.Collections;
using UnityEngine;

public class PaintBucket : InteractObject
{
    [SerializeField]
    private Paint paint;
    [SerializeField]
    private Transform attach;


    //물감나오는거 구현
    public void PaintPlay()
    {
        //paintParticle.Play();
        
        coroutine = StartCoroutine(PaintInstant());
    }

    public void PaintStop()
    {
        //paintParticle.Stop();

        StopCoroutine(coroutine);
    }

    Coroutine coroutine;
    IEnumerator PaintInstant()
    {
        while (true)
        {
            Paint paintPrefab = Instantiate(paint, attach.position, Quaternion.identity);
            Destroy(paintPrefab, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }


    //레이를 쏴서 아래가 팔렛트면 물감을 생성한다

}

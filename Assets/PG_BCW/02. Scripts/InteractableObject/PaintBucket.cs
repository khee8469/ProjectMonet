using Jc;
using UnityEngine;

public class PaintBucket : InteractObject
{
    [SerializeField] 
    private ParticleSystem paintParticle;

    
    //물감나오는거 구현
    public void PaintPlay()
    {
        paintParticle.Play();
    }

    public void PaintStop()
    {
        paintParticle.Stop();
    }

    //레이를 쏴서 아래가 팔렛트면 물감을 생성한다

}

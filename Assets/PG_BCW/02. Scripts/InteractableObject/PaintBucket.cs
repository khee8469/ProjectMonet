using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.OpenXR;
using UnityEngine.XR.Interaction.Toolkit;

public class PaintBucket : InteractObject
{
    [SerializeField] ParticleSystem paintParticle;


    
    public void PaintPlay()
    {
        paintParticle.Play();
    }

    public void PaintStop()
    {
        paintParticle.Stop();
    }

}

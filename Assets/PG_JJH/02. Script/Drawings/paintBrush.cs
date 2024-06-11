using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;

namespace JJH
{
    public class paintBrush : XRGrabInteractable
    {

        [SerializeField] private CreateTrail createTrail;


        private void Start()
        {
            createTrail = GetComponent<CreateTrail>(); // 페인트에 이미 할당 되어 있는 스크립트.
        }


        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
            createTrail.StartTrail();

        }

        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            base.OnDeactivated(args);
            createTrail.EndTrail();

        }
    }

}


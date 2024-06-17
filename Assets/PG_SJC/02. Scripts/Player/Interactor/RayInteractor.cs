using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    /// <summary>
    /// Custom Ray Inetractor
    /// </summary>
    public class RayInteractor : XRRayInteractor
    {
        //[Header("커스텀")]

        // 오브젝트를 잡을 수 있는 거리체크
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            if (!GrabableDistance(interactable))
                return false;

            return base.CanHover(interactable);
        }
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!GrabableDistance(interactable))
                return false;

            return base.CanSelect(interactable);
        }

        private bool GrabableDistance(IXRInteractable interactable)
        {
            InteractObject itrObject = interactable as InteractObject;
            
            if (itrObject == null)
                return false;

            // 오브젝트의 그랩 허용 길이
            float grabDist = itrObject.GrabDistance;
            // 현재 오브젝트와의 거리
            float distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance > grabDist * grabDist)
            {
                return false;
            }

            return true;
        }

    }
}

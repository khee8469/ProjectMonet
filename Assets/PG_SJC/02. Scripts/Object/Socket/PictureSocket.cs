using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 소켓 인터랙터 상속 - 커스텀 소켓
    public class PictureSocket : CustomSocket
    {

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            // 소켓 거리 체크 후 Hover 활성화
            if ((interactable.transform.position - transform.position).sqrMagnitude > 1f)
                return false;

            return base.CanHover(interactable);
        }

        // Hover 콜백
        protected override void OnHoverEntering(HoverEnterEventArgs args)
        {
            base.OnHoverEntering(args);
        }
        protected override void OnHoverExiting(HoverExitEventArgs args)
        {
            base.OnHoverExiting(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }
    }
}
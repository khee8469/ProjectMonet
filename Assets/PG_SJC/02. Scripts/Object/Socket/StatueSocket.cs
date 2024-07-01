using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    // 스테이지 1-3 퍼즐 조각상 소켓
    public class StatueSocket : CustomSocket
    {
        [Header("전용 조각상")]
        [SerializeField]
        private StatueObject statue;

        [SerializeField]
        private GameObject[] roofObs;

        private void ActiveEvent()
        {
            foreach (GameObject go in roofObs)
                Destroy(go);
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            // 조각상 오브젝트가 아닐 경우
            if (interactable is not StatueObject)
                return false;

            return base.CanHover(interactable);
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            // 조각상 오브젝트가 아닐 경우
            if (interactable is not StatueObject)
                return false;

            return base.CanSelect(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            StatueObject ob = args.interactableObject as StatueObject;
            if (ob == null) return;
            if (ob != statue) return;

            ob.transform.position = attachTransform.position;
            ob.transform.rotation = attachTransform.rotation;
            ob.GetComponent<Rigidbody>().isKinematic = true;
            ob.GetComponent<Collider>().enabled = false;
            // 조각상이 끼워진 경우 이벤트 발생
            ActiveEvent();
        }
    }
}

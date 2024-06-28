using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    [RequireComponent(typeof(Rigidbody))]
    public class ReturnLobbyScene : InteractObject, IActivatable
    {
        public string sceneName;

        new Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            throwOnDetach = false;
        }
        public void Activate()
        {
            Manager.Scene.LoadScene(sceneName);
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
            //Activate();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            return;

        }

        public void ResetColliderPosition()
        {

        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    [RequireComponent(typeof(Rigidbody))]
    public class ReturnLobbyScene : XRSimpleInteractable, IActivatable ,IInteractable // 얘네는 그냥 로비씬으로 전환 시켜주는 --> 씬에 있는 그림에 붙여 줄 스크립트 
    {
        public string sceneName;

        new Rigidbody rigidbody;

        [SerializeField] private float distance = 5f;


        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            
        }
        public void Activate()
        {
            Manager.Scene.LoadScene(sceneName);
        }

        /*protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }*/

        /*protected override void OnActivated(ActivateEventArgs args)
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

        }*/
        public float GetInteractDistance()
        {
            return 10f;
        }

        public Transform GetTransform()
        {
            throw new System.NotImplementedException();
        }

        public float GetDistanceThreshold()
        {
            return distance;
        }
    }
}



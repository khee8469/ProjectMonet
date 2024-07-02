using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using JJH;
using Jc;
using UnityEngine.Events;

namespace JJH
{
    public class LightPanelButton : CustomButton
    {

        [Tooltip("OnSelected 에 할당 할 이벤트")]
        public static UnityEvent OnPushedEvent = new UnityEvent();
        public static UnityEvent OnReleaseEvent = new UnityEvent();

        private void Start()
        {
            
        }


        public void UpButtonPush()
        {
            Debug.Log("업 버튼 누름");
        }

        public void DownButtonPush()
        {
            Debug.Log("다운 버튼 누름");
        }

        public void LeftButtonPush()
        {
            Debug.Log("왼쪽 버튼 누름");
        }

        public void RightButtonPush()
        {
            Debug.Log("오른쪽 버튼 누름");
        }



        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            
        }
    }
}



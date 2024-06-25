using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class DrawWithPen : InteractObject
    {
        // Interactor Obejct 상속함. 
        [SerializeField] private Pen pen;

        private void Start()
        {
            pen = GetComponent<Pen>();
        }


        // 임시로 이거 계속 true로 변경해주자.

        private void FixedUpdate()
        {
            pen.StartDrawing();
            
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            
            base.OnActivated(args);
            pen.StartDrawing();  // 그리기 시작
            // 여기서 라인 렌더러 생성. 
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
        }


        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            base.OnDeactivated(args);
            //pen.DrawingStop();  임시 제거 해보기. 
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                pen.SwitchColor();
            }
        }

    }
}


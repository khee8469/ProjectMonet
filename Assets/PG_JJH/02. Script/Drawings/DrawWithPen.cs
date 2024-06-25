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
        protected override void OnActivated(ActivateEventArgs args)
        {
            Debug.Log("pen의 on activate");   
            base.OnActivated(args);
            pen.StartDrawing();  // 그리기 시작
            // 여기서 라인 렌더러 생성. 
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            Debug.Log("pen의 on selected exit");
        }


        protected override void OnDeactivated(DeactivateEventArgs args)
        {

            Debug.Log("pen의 de activate ");
            base.OnDeactivated(args);
            pen.DrawingStop(); 
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


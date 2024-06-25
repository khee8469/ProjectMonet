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
        [SerializeField] private Pen pen;

        private void Start()
        {
            pen = GetComponent<Pen>();
        }
        protected override void OnActivated(ActivateEventArgs args)
        {
            
            base.OnActivated(args);
            pen.StartDrawing();  // 그리기 시작
            // 여기서 라인 렌더러 생성. 
        }

        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            
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


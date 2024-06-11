using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class DrawWithPen : XRGrabInteractable
    {
        [SerializeField] private Pen pen;

        private void Start()
        {
            pen = GetComponent<Pen>();
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            Debug.Log("온 액티베이티드");
            base.OnActivated(args);
            pen.StartDrawing();  // 그리기 시작
            // 여기서 라인 렌더러 생성. 
        }

        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            Debug.Log("온 디액티베이티드");
            base.OnDeactivated(args);
            pen.DrawingStop(); 
            
            // 여기서 라인 렌더러 생성 중지. 
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("스위치 컬러"); 
                pen.SwitchColor();
            }
        }


    }
}


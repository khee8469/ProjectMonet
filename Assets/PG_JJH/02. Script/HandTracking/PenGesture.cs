using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    public class PenGesture : Gesture
    {
        // Interaction Manager 에 상호작용을 신청하는듯 하다. 
        // 이거를 그냥 DrawWithPen의 핸드 버전이라고 생각하고 작업하자.
        [SerializeField] Pen pen;


        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            pen = GetComponent<Pen>();
        }


        // Hand tracking 이 진입되고 나가는 부분 인듯하다. 
        public override void LeftGestureEnter()
        {
            pen.StartDrawing(); 
        }

        public override void LeftGestureExit()
        {
            pen.DrawingStop();
        }

        // hand tracking 이 진입 되고 나가는 부분인듯 하다. 
        public override void RightGestureEnter()
        {
            pen.StartDrawing();

        }
        public override void RightGestureExit()
        {
            pen.DrawingStop();

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.V))
            {
                pen.SwitchColor(); 
            }
                
        }
    }
}


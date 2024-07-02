using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;

namespace JJH
{
    public class LightHousePuzzle : PuzzleManager
    {
        // button 들에는 XrPushButton 이용 

        [Tooltip("등대 오브젝트의 머리 부분")]
        [SerializeField]
        private GameObject lightHouse;

        [Tooltip(" 등대의 최대 회전 각도")]
        private float limitRotation = 45;

        [Tooltip("버튼 오브젝트 관리")]
        List<GameObject> buttonList = new List<GameObject>();  

        private void Start()
        {
            // 클리어 이벤트 등록. 
            OnClear.AddListener(LightHouseClear);
            
        }

        // 이벤트 등록
        protected override void OnEnable()
        {
            
        }

        // 이벤트 해제 
        private void OnDisable()
        {
            
        }

        // 각 버튼 들에 할당해줄 이벤트 -> 1회에 5도 
        public void UpButton()
        {
            
        }

        public void DownButton() 
        {

        }

        public void LeftButton()
        {

        }

        public void RightButton()
        {

        }


        // 클리어 되었을 시 호출
        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
        }

        // 클리어 이후 진행 할 이벤트 (아이템 지급 등의 )
        public void LightHouseClear()
        {

        }

    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    /*public class Paint : MonoBehaviour
    {
        [Header("컬러 관련 데이터")]
        [Tooltip("컬러 열거형을 설정함.")]
        public PaintTypeEnum PaintType;

        [Tooltip("컬러 데이터 스크립터블 오브젝트")]
        public PaintTypeManager paintTypeManager;

        /private void Start()
        {
            if (paintTypeManager != null) // 스크립터블 오브젝트가 할당되어 있는 상태라면 
            {
                Color color = GetColorByType(PaintType);
                Color thisObj = GetComponent<Renderer>().material.color = color; // 게임 오브젝트 컬러 변경 ? 
                Debug.Log(thisObj);
            }

        }

        private Color GetColorByType(PaintTypeEnum paintType) // 매개변수로 열거형을 받아 컬러를 할당해줌. 
        {
            PaintTypeColor paintTypeColor
                = paintTypeManager.paintTypeColors.Find(ptc => ptc.type == type);

            if (paintTypeColor != null)
            {
                return paintTypeColor.color;
            }

            return Color.white; // 기본색상. 
        }

    }*/

}


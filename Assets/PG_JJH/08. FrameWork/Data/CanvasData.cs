using System;
using JJH;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    [System.Serializable]
    public class CanvasData  // 캔버스 데이터 이름 수정 할 것. --> Manager 도 이름 수정 필요.
        // 채색 기능은 json 으로 씬 전환 시 및 다시 시작 할 시 저장 되도록 해야 한다. 
    {

        [Tooltip("로비 그림 --> 이게 둘 중 에 하나만 있어도 아마 될 듯 한대 일단 그냥 진행함 나중에 수정하자 ")]
        // 그림 조각 저장용
        public bool[] myDrawPartCheckArr = new bool[20];

        // 그림 완성 상태 저장용
        public bool[] myDrawCompleteCheckArr = new bool[20];

        [Tooltip("포스트 프로세싱 컬러 / 흑백 전환 bool 변수 -> 0 1 2 3 각 스테이지 마다 체크 / 로딩루틴 ")]
        // 씬의 포스트 프로세싱 상태 저장용 
        public bool[] isColoredCheckArr = new bool[4];

        [Header("로비 씬 해금")]

        // 스테이지 언락은 일단 스크립터블로 진행 중 나중에 json 으로 바꾸던지하자.
        public List<bool> stageUnLockCheckList = new List<bool>();

        [Header("2스테이지 퍼즐 -5 원반 --> 딱히 저장 할 필요 없는 듯?")]
        public List<Quaternion> myPatternCheckList = new List<Quaternion>();  

    }


    

}


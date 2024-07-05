using System;
using JJH;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace JJH
{
    [System.Serializable]
    public class GameData
    {

        [Header("로비 그림")]
        // 그림 조각 저장용
        public bool[] myDrawPartCheckArr = new bool[20];

        // 그림 완성 상태 저장용
        public bool[] myDrawCompleteCheckArr = new bool[20];

        // 씬의 포스트 프로세싱 상태 저장용 
        public bool[] isColoredCheckArr = new bool[4];

        [Header("로비 씬 해금")]

        // 스테이지 언락은 일단 스크립터블로 진행 중 나중에 json 으로 바꾸던지하자.
        public List<bool> stageUnLockCheckList = new List<bool>();

        [Header("2스테이지 퍼즐 -5 원반 ")]
        public List<Quaternion> myPatternCheckList = new List<Quaternion>();  



    }


    

}


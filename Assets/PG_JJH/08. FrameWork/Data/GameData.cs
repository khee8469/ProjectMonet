using System;
using JJH;
using System.IO;
using System.Collections.Generic;

namespace JJH
{
    [System.Serializable]
    public class GameData
    {
        // 그림 조각 저장용
        public bool[] myDrawPartCheckArr = new bool[20];

        // 그림 완성 상태 저장용
        public bool[] myDrawCompleteCheckArr = new bool[20];

        // 씬의 포스트 프로세싱 상태 저장용 
        public bool[] isColoredCheckArr = new bool[4];

        // 스테이지 언락은 일단 스크립터블로 진행 중 나중에 json 으로 바꾸던지하자.
        public List<bool> stageUnLockCheckList = new List<bool>();
    }


    

}


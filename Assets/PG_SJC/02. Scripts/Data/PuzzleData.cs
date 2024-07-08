using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 로컬 저장용 퍼즐 데이터
    [System.Serializable]
    public struct PuzzleData
    {
        [Header("퍼즐 ID")]
        public int puzzleID;

        [Header("퍼즐 상태")]
        public int puzzleState;

        public PuzzleData(int puzzleID, int puzzleState)
        {
            this.puzzleID = puzzleID;
            this.puzzleState = puzzleState;
        }
    }
}

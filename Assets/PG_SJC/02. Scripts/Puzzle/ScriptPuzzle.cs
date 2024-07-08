using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 스크립트 전용 퍼즐매니저
    /// 이전 퍼즐의 수행 여부를 통해 해금됨.
    /// </summary>
    public class ScriptPuzzle : PuzzleManager
    {
        [Header("연계된 다음 퍼즐 매니저")]
        public PuzzleManager nextPuzzle;

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();

            nextPuzzle.ChangeState(PuzzleState.Proceed);
        }
    }
}

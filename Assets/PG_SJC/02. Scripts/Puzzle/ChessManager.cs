using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class ChessManager : PuzzleManager
    {
        [Header("체스 오브젝트 모음")]
        [SerializeField]
        private List<ChessObject> chessObjects;

        [SerializeField]
        private GameObject statueOb;

        public void ResetBoard()
        {
            foreach(ChessObject ob in chessObjects)
            {
                ob.ResetObject();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ResetBoard();
        }

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
            statueOb.SetActive(true);
        }
    }
}

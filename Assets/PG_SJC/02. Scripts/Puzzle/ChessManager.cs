using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

namespace Jc
{
    public class ChessManager : PaintRewardPuzzle
    {
        [Header("체스 오브젝트 모음")]
        [SerializeField]
        private List<ChessObject> chessObjects;
        [Header("태양 오브젝트")]
        [SerializeField]
        private GameObject sunObject;

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
            if (!Manager.Layer.playerLM.Contain(other.gameObject.layer))
                return;

            if(!sunObject.activeSelf)
                sunObject.SetActive(true);

            ResetBoard();
        }

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
            statueOb.SetActive(true);
        }
    }
}

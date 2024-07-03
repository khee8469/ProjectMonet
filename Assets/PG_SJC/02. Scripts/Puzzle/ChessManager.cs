using System.Collections;
using System.Collections.Generic;
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
        private StatueObject statueOb;

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

            Debug.Log("오두막 진입");

            if (!sunObject.activeSelf)
                sunObject.SetActive(true);

        }
        private void OnTriggerExit(Collider other)
        {
            if (!Manager.Layer.playerLM.Contain(other.gameObject.layer))
                return;

            Debug.Log("오두막 탈출");

            if (sunObject.activeSelf)
                sunObject.SetActive(false);
            
            ResetBoard();
        }

        public override void OnClearPuzzle()
        {
            base.OnClearPuzzle();
            statueOb.gameObject.SetActive(true);
        }
    }
}

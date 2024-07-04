using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;
namespace JJH
{
    public class GearSocket : XRSocketInteractor , IPuzzleable
    {

        [Header("퍼즐 매니저 에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;

        [Tooltip("퍼즐과 매칭 시킬 인덱스")]
        private int puzzleIndex;

        [Tooltip("기어 오브젝트와 매칭 시킬 소켓")]
        [SerializeField] private int socketID;

        



        public void ActiveSetting()
        {

        }

        public void CompleteSetting()
        {

        }

        public void DisActiveSetting()
        {

        }

        public void RegistObject(PuzzleManager puzzle)
        {

        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {

        }
    }
}



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

        [Tooltip("해당 소켓에 들어가야 할 기어 오브젝트")]
        private GearObject targetGear;


        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
            Debug.Log(puzzle.puzzleObjects.Count);
        }
        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            Debug.Log("기어 셀렉팅");
            base.OnSelectEntering(args);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            GearObject obj = args.interactableObject as GearObject;

            // 기어 라면 일단 들어는 가야한다.
            if (obj == null) return; // 기어가 아니면 리턴 시킨다. 


            puzzle.UpdateCondition(puzzleIndex);


        }







        

        public void ActiveSetting()
        {
            return;
        }

        // 각각의 소켓에 대해서 gearIDiTEM을 생성해놓는다. 
        public void CompleteSetting()
        {
            puzzle.UpdateCondition(puzzleIndex);

            // gear object 들은 회전 되고 있는 상태여야 한다. 
            // 
        }

        // 퍼즐 비활성화 상태 적용 
        public void DisActiveSetting()
        {
            return;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index); // 이거는 그냥 만들어만 두고 부르는 곳이 없는듯?
        }
    }
}



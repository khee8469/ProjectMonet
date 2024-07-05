using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using JetBrains.Annotations;

namespace JJH
{
    public class HiddenPatternObject : InteractObject, IPuzzleable
    {
        // 회전 되어야 하는 오브젝트임. 
        // --> 1. 가장 내부에 있는 오브젝트는 움직이지 않음.
        // --> 2. 가장 내부 부터 하나 씩 맞춰야 하기 때문에 1 2 3 4 이면
        // 최초 시작은 1 3 4 가 고정되어서 움직일 수 없게 해야 한다.
        // 2번 조각과 1번이 맞으면 1 2 를 고정 시키고 3을 움직일 수 있게 한다.

        // 추가로 회전 시키는 거니까 이거 xr sample 확인하자. 

        [Header("원반 Spec")]
        [Tooltip("각 원반 들의 아이디")]
        [SerializeField] int PatternID;

        [Header("퍼즐 매니저 5번 퍼즐")]
        [Tooltip("5번 퍼즐의 퍼즐 매니저")]
        [SerializeField] private HiddenPatternManager puzzle;

        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle); // 5번 매니저 퍼즐에 regist 등록. 
        }

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
            puzzle.puzzleObjects.Add(this);
        }


        // 원반이 하나 맞을 때 마다 update 돌리기. --> 돌리고 다음 원반의 interactionLayer를 everyThing으로 변경
        // 충돌은 그대로 가져가서 못 뚫으면서 grab이 되지는 않도록 해야한다.
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {

        }
    }

    [System.Serializable]
    public class 



}


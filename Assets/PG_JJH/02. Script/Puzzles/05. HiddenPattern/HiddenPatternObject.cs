using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using JetBrains.Annotations;
using UnityEngine.XR.Content.Interaction;
using UnityEditor.Rendering;

namespace JJH
{
    public class HiddenPatternObject : XRKnob, IPuzzleable
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

        [Tooltip("콜라이더")]
        [SerializeField] private new Collider []  collider;

        [Header("Base_Map")]
        [Tooltip("라이트에 비춰지지 않은 문양이 없는 상태의 baseMap")]
        [SerializeField] private Texture drawTexture;

        [Header("퍼즐 매니저 5번 퍼즐")]
        [Tooltip("5번 퍼즐의 퍼즐 매니저")]
        [SerializeField] private HiddenPatternManager puzzle;
        
      

        


        protected override void Awake()
        {
            base.Awake();
            RegistObject(puzzle); // 5번 매니저 퍼즐에 regist 등록. 
        }

        private void Start()
        {
            
        }



        Coroutine coroutine;
        public void StartCheckRoutine()
        {
            coroutine = StartCoroutine(CheckRoutine());
        }

        public void StopCheckRoutine()
        {
            if(coroutine != null)
            {
                StopCoroutine(CheckRoutine());
            }           
        }

        private IEnumerator CheckRoutine()
        {
            while(true)
            {
                //  1번과 2번의...  2번과 3번의.. 3번과 4번의.. 쿼너티언 값 체크. 




                CheckMyRotation(PatternID);
                yield return null;
            }
        }


        private void CheckMyRotation(int ID)
        {
            if(ID==2)
            {

            }
            else if(ID==3)
            {

            }
            else if(ID==4)
            {

            }
        }

        private void CorrectPatternRotation(int ID)
        {
            if(ID == PatternID)
            {
                for(int i=0;i< collider.Length;i++)
                {
                    collider[i].enabled = false; // 해당 ID는 완성 되었으므로 더이상 만지기 불가능.
                }
            }
        }


        public void ActiveSetting()
        {
            for (int i = 0; i < collider.Length; i++)
            {
                collider[i].enabled = false;
            }

        }

        // 추가로 완성된 상태이므로 그림도 나와 있는 상태여야 한다.
        public void CompleteSetting()
        {

        }
        public void DisActiveSetting()
        {
            for(int i=0;i <collider.Length;i++)
            {
                collider[i].enabled = false;
            }
            
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }


        // 원반이 하나 맞을 때 마다 update 돌리기. --> 돌리고 다음 원반의 interactionLayer를 everyThing으로 변경
        // 충돌은 그대로 가져가서 못 뚫으면서 grab이 되지는 않도록 해야한다.
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index);
        }
    }

    [System.Serializable] // json으로 저장해서 씬 간 저장해 둘 쿼터니언 값 
    public class PatternData
    {
        // 원반의 현재 회전값
        Quaternion rotation;

    }



}


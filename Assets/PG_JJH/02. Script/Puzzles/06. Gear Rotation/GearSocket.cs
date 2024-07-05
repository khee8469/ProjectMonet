using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
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
        // 이거 오브젝트가 이전 퍼즐에서 생성되는 오브젝트 이기 때문에 
        // 이런 식으로 미리 넣어 둘 수가 없기 때문에
        // 프리팹의 ID와 맞춰둬야 할 것 같다.
        // TARGET 으로 프리팹으로 빼둔 Gear를 넣어두고 그 ID와 매칭 시키자.
        private GearObject targetGear;

        [Tooltip("자신의 소켓 충돌 판정콜라이더")]
        [SerializeField] private Collider socketCollider;
        
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

            // 해당 톱니바퀴가 타겟과 일치한다면 퍼즐 상태를 Update 

            // 프리팹으로 설정해둔 itemID 와 실제 기어의 ItemID 가 일치해야 UPDATE 가능. 
            // 또는 진짜 GearID 설정해 둘거니까 그거로 체크해도 상관 x 
            if(obj.itemData.itemID == targetGear.itemData.itemID)
            {
                puzzle.UpdateCondition(puzzleIndex);
            }
        }


        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            GearObject obj = args.interactableObject as GearObject;
            
            if(obj.itemData.itemID == targetGear.itemData.itemID) // 만약 소켓에서 제외할 때 정답 기어 였다면
            {
                // False로 바꿔줘야함.
                puzzle.UpdateCondition(puzzleIndex);
            }

        }



        // 활성화 상태면 콜라이더 켜주기.
        public void ActiveSetting()
        {
            socketCollider.enabled = true;
        }

        // 각각의 소켓에 대해서 gearIDiTEM을 생성해놓는다. 
        public void CompleteSetting()
        {
            puzzle.UpdateCondition(puzzleIndex);

            // 기어를 자신의 소켓 위치에 instantiate 해주고 
            // attach 해주고 회전까지 시켜주기. 


        }

        // 퍼즐 비활성화 상태 적용 --> 콜라이더 꺼주기. 
        public void DisActiveSetting()
        {
            socketCollider.enabled = false;

        }

        // 퍼즐 등록 
        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        // 그냥 업데이트 condition 자체를 부르는 중 
        // 자체적으로 완료되면 onclear 호출 되도록 되어 있음. 
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index); // 이거는 그냥 만들어만 두고 부르는 곳이 없는듯?
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using EPOOutline.Demo;
namespace JJH
{
    public class GearSocket : XRSocketInteractor  , IPuzzleable
    {

        [Header("퍼즐 매니저 에디터 세팅")]
        [SerializeField]
        private GearManager gearPuzzle;

        [Tooltip("퍼즐과 매칭 시킬 인덱스")]
        [SerializeField] private int puzzleIndex;

        [Tooltip("해당 소켓에 들어가야 할 기어 오브젝트")]
        // 이거 오브젝트가 이전 퍼즐에서 생성되는 오브젝트 이기 때문에 
        // 이런 식으로 미리 넣어 둘 수가 없기 때문에
        // 프리팹의 ID와 맞춰둬야 할 것 같다.
        // TARGET 으로 프리팹으로 빼둔 Gear를 넣어두고 그 ID와 매칭 시키자.
        [SerializeField] private GearObject targetGear; // 이거도 프리팹으로 둬야함!!

        public GearObject TargetGear { get { return targetGear; } }

        [Tooltip("자신의 소켓 충돌 판정콜라이더")]
        [SerializeField] private Collider socketCollider;

        protected override void Awake()
        {
            base.Awake();
            RegistObject(gearPuzzle);
            
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            GearObject obj = args.interactableObject as GearObject;
            // 기어 라면 일단 들어는 가야한다.
            if (obj == null) return; // 기어가 아니면 리턴 시킨다. 

            if (this.hasSelection) // 현재 소켓에 아이템이 들어가 있다. 
            {
                IXRSelectInteractor interactor = obj.firstInteractorSelecting;

                if (interactor is XRBaseControllerInteractor)
                {
                    Debug.Log("소켓에 이미 아이템이 있고 사람이 넣으려고 했음");
                    obj.interactionManager.SelectExit(interactor as IXRSelectInteractor,
                        obj as IXRSelectInteractable);
                }

            }

            // 해당 톱니바퀴가 타겟과 일치한다면 퍼즐 상태를 Update 
            // 프리팹으로 설정해둔 itemID 와 실제 기어의 ItemID 가 일치해야 UPDATE 가능. 
            // 또는 진짜 GearID 설정해 둘거니까 그거로 체크해도 상관 x 
             gearPuzzle.realGears.Add(obj);

            if (obj.itemData.itemID == targetGear.itemData.itemID)
            {
                gearPuzzle.UpdateCondition(puzzleIndex);
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            GearObject obj = args.interactableObject as GearObject;
             gearPuzzle.realGears.Remove(obj);
            if (obj.itemData.itemID == targetGear.itemData.itemID) // 만약 소켓에서 제외할 때 정답 기어 였다면
            {
                // False로 바꿔줘야함.
                gearPuzzle.UpdateCondition(puzzleIndex, false);
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
            gearPuzzle.UpdateCondition(puzzleIndex);
            gearPuzzle.OnClearPuzzle();

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



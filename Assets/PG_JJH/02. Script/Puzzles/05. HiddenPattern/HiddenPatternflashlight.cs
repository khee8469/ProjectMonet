using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH

{
    public class HiddenPatternflashlight : InteractObject, IPuzzleable
    {
        // 패턴 퍼즐 용 플래시 라이트 
        // 더이상 잡지 못하도록
        [Tooltip("랜턴의 콜라이더")]
        [SerializeField] private new Collider collider;

        [Tooltip("5번 퍼즐의 퍼즐 매니저")]
        [SerializeField] private HiddenPatternManager puzzle;

        [Tooltip("랜턴이 원상 복귀 될 위치")]
        [SerializeField] private Vector3 flashLightPosition;
        [Tooltip("랜턴이 원상 복귀 될 회전값")]
        [SerializeField] private Quaternion flashLightRotation;

        [Tooltip("랜턴 불 빛 spot Light ")]
        [SerializeField] Light spotLight;

        private new void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }

        private void Start()
        {
            // 씬 시작 시 원래 위치 저장.
            flashLightPosition = transform.localPosition;
            flashLightRotation = transform.localRotation;
            collider = GetComponent<Collider>();

        }

        public void FlashLightReturn() // 원 위치 복귀
        {
            transform.localPosition = flashLightPosition;
            transform.localRotation = flashLightRotation;
        }


        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            // 사람에게 잡히면 랜턴 불빛이 켜져야 한다.
            // player의 손에 Custom Check 붙여주기. --> 손 판단용임. 
            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {

            }


        }




        // 퍼즐 저장에 대해서 신경 쓰지 말고
        // 그냥 시작 전 상태 완료 상태 두 가지만 생각하자. 
        public void ActiveSetting()
        {
            collider.enabled = true; // 퍼즐이 활성화 되면 만질 수 있도록.
        }

        public void CompleteSetting()
        {
            collider.enabled = false; // 더 이상 만지지 못함. 
        }

        public void DisActiveSetting()
        {
            collider.enabled = false;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        // 구현 할 필요 x 
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {

        }
    }
}



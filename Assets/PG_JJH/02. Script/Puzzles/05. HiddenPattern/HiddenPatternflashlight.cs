using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        private new void Awake()
        {
            base.Awake();
            RegistObject(puzzle);
        }

        private void Start()
        {
            flashLightPosition = transform.localPosition;
            flashLightRotation = transform.localRotation;

        }

        public void FlashLightReturn() // 원 위치 복귀
        {
            transform.localPosition = flashLightPosition;
            transform.localRotation = flashLightRotation;
        }


        // 어차피 다음 번에 로딩 할 때 발동되니까 퍼즐 깬 순간은 상관 할 필요 x -> 어차피 나가면 다시 돌아가기 때문에.
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class CustomLever : XRLever, IInteractable, IPuzzleable
    {
        [Space(10)]
        [Header(" --- 커스텀 세팅 --- ")]
        [Space(5)]
        [Header("퍼즐 매니저")]
        [SerializeField]
        private PuzzleManager puzzle;

        [Header("양손 그랩 가능여부")]
        [SerializeField]
        protected bool isSingleGrab;
        [Header("잡을 수 있는 최소거리")]
        [SerializeField]
        protected float grabDistanceThreshold;
        [Header("레버 스틱 트랜스폼")]
        [SerializeField]
        private Transform leverStickTr;
        [Header("레버 스틱 충돌체")]
        [SerializeField]
        private Collider leverStickCol;
        [Header("최초 레버 회전 값")]
        [SerializeField]
        private float baseAngle;
        [Header("성공 레버 회전 값")]
        [SerializeField]
        private float targetAngle;

        private void FixedUpdate()
        {
            if (isSelected)
                CheckValue();
        }

        private void CheckValue()
        {
            if(leverStickTr.localEulerAngles.x >= targetAngle && leverStickTr.localEulerAngles.x <= maxAngle)
            {
                interactionManager.SelectExit(m_Interactor, this);

                if (leverStickCol != null)
                    leverStickCol.enabled = false;
                else
                    Debug.Log("레버스틱 충돌체 에러");

                UpdatePuzzleManager(puzzle);
            }
        }

        #region 인터페이스 재정의
        // IPuzzleable
        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
        {
            puzzle.OnClearPuzzle();
        }

        public void ActiveSetting()
        {
            handle = leverStickTr;
            if (leverStickCol != null)
                leverStickCol.enabled = true;
            else
                Debug.Log("레버스틱 충돌체 에러");
        }

        public void DisActiveSetting()
        {
            if (leverStickCol != null)
                leverStickCol.enabled = false;
            else
                Debug.Log("레버스틱 충돌체 에러");
        }

        public void CompleteSetting()
        {
            if (leverStickCol != null)
                leverStickCol.enabled = false;
            else
                Debug.Log("레버스틱 충돌체 에러");
            leverStickTr.transform.localRotation = Quaternion.Euler(new Vector3(maxAngle, leverStickTr.localEulerAngles.y, leverStickTr.localEulerAngles.z));
        }

        // IInteractable
        public float GetDistanceThreshold()
        {
            return grabDistanceThreshold;
        }
        public float GetInteractDistance()
        {
            return 0f;
        }
        public bool GetSingleGrab()
        {
            return isSingleGrab;
        }
        public Transform GetTransform()
        {
            return transform;
        }
        #endregion
    }
}

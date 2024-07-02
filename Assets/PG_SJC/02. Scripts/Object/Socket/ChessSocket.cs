using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class ChessSocket : CustomSocket, IPuzzleable
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;
        [SerializeField]
        private int puzzleIndex;            // 퍼즐 인덱스
        [SerializeField]
        private GameObject pillar;          // 체스말 받침대

        [SerializeField]
        private ChessObject targetChess; // 타깃 체스말
        [SerializeField]
        private float targetScale;          // 타깃 스케일
        [SerializeField]
        private float scaleThreshHold;      // 스케일 임계치

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            Debug.Log("체스 셀렉팅");
            base.OnSelectEntering(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log("체스 셀렉");
            base.OnSelectEntered(args);

            ChessObject temp = args.interactableObject as ChessObject;
            // 오브젝트 체크
            if (temp == null)
                return;
            temp.transform.position = attachTransform.position;
            // 타겟 체크
            if (temp != targetChess)
                return;
            // 타겟 스케일 체크
            if (temp.transform.localScale.x > targetScale + scaleThreshHold || temp.transform.localScale.x < targetScale - scaleThreshHold)
                return;

            // 조건 값 충족
            temp.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            temp.transform.parent = transform;
            temp.IsEnable = true;
            temp.colliders[0].enabled = false;
            StartCoroutine(PillarRoutine(temp));
            Debug.Log("조건 값이 충족되었습니다.");
        }

        IEnumerator PillarRoutine(ChessObject obj)
        {
            float rate = 0f;
            Vector3 startPos = pillar.transform.localPosition;
            Vector3 endPos = new Vector3(pillar.transform.localPosition.x, -1.3f, pillar.transform.localPosition.z);
            while(rate < 1f)
            {
                rate += Time.deltaTime / 3f;
                pillar.transform.localPosition = Vector3.Lerp(startPos, endPos, rate);
                yield return null;
            }
            puzzle.UpdateCondition(puzzleIndex);
            pillar.transform.localPosition = endPos;
            yield return null;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index);
        }
        public void ActiveSetting(){}
        public void DisActiveSetting(){}
        public void CompleteSetting()
        {
            // 완성 세팅
            targetChess.transform.position = attachTransform.position;
            targetChess.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            targetChess.transform.parent = transform;
            targetChess.IsEnable = true;
            targetChess.colliders[0].enabled = false;
            pillar.transform.localPosition = new Vector3(pillar.transform.localPosition.x, -1.3f, pillar.transform.localPosition.z); ;
            puzzle.UpdateCondition(puzzleIndex);
        }
    }
}

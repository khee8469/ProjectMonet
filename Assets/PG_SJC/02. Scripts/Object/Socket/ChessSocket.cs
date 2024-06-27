using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    public class ChessSocket : CustomSocket
    {
        [Header("에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;
        [SerializeField]
        private int puzzleIndex;            // 퍼즐 인덱스
        [SerializeField]
        private GameObject pillar;          // 체스말 받침대

        [SerializeField]
        private ResizingObject targetChess; // 타깃 체스말
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

            ResizingObject temp = args.interactableObject as ResizingObject;
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

            temp.transform.localScale = new Vector3(targetScale, targetScale, targetScale);

            // 조건 값 충족
            Debug.Log("조건 값이 충족되었습니다.");
        }
    }
}

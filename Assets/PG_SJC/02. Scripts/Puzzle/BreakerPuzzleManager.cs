using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 스테이지 4 차단기 레버 퍼즐 매니저
    public class BreakerPuzzleManager : PuzzleManager
    {
        [Space(10)]
        [Header(" --- 차단기 퍼즐 매니저 --- ")]
        [Space(10)]
        [Header("좌측 차단기 힌지 트랜스폼")]
        [SerializeField]
        private Transform lftBreakerHingeTr;
        [Header("우측 차단기 힌지 트랜스폼")]
        [SerializeField]
        private Transform fwdBreakerHingeTr;

        private const float openRot = -90f;
        private const float closeRot = 0f;

        private Coroutine clearRoutine;

        protected override void InitPuzzleSetting()
        {
            base.InitPuzzleSetting();

            // 차단기 활성/비활성화
            switch(State)
            {
                case PuzzleState.DisActive:
                case PuzzleState.Proceed:
                    lftBreakerHingeTr.localEulerAngles = new Vector3(0f, 0f, openRot);
                    fwdBreakerHingeTr.localEulerAngles = new Vector3(0f, 0f, closeRot);
                    break;
                case PuzzleState.Clear:
                    lftBreakerHingeTr.localEulerAngles = new Vector3(0f, 0f, closeRot);
                    fwdBreakerHingeTr.localEulerAngles = new Vector3(0f, 0f, openRot);
                    break;
            }
        }
        
        public override void OnClearPuzzle()
        {
            if(clearRoutine == null)
            clearRoutine = StartCoroutine(ClearRoutine());
        }
        IEnumerator ClearRoutine()
        {
            yield return HingeRotationRoutine();
            base.OnClearPuzzle();
            yield return null;
        }
        IEnumerator HingeRotationRoutine()
        {
            float rate = 0f;
            Quaternion openedRot = Quaternion.Euler(new Vector3(0f, 0f, openRot));
            Quaternion closedRot = Quaternion.Euler(new Vector3(0f, 0f, closeRot));

            while(rate < 1f)
            {
                rate += Time.deltaTime / 3f;
                fwdBreakerHingeTr.transform.rotation = Quaternion.Lerp(closedRot, openedRot, rate);
                lftBreakerHingeTr.transform.rotation = Quaternion.Lerp(openedRot, closedRot, rate);
                yield return null;
            }
        }
    }
}
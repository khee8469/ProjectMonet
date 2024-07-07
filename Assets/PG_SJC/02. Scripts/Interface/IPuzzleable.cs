using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 퍼즐 관련 오브젝트에 적용할 인터페이스
/// </summary>
namespace Jc
{
    public interface IPuzzleable
    {
        // 퍼즐 매니저에 오브젝트 등록
        public void RegistObject(PuzzleManager puzzle);

        // 퍼즐 매니저의 컨디션 업데이트
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1);

        // 퍼즐 활성화 상태 적용
        public void ActiveSetting();
        // 퍼즐 비활성화 상태 적용
        public void DisActiveSetting();
        // 퍼즐 완료 상태 적용
        public void CompleteSetting();
    }
}

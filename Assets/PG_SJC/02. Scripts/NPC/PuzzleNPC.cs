using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 스크립트 출력 전용 NPC
    /// 퀘스트 진행 중 대사 출력 (퀘스트 흐름에 관여)
    /// </summary>
    public class PuzzleNPC : NPC, IPuzzleable
    {
        [Header("퍼즐 NPC - 에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;

        [Header("나레이션 번들 ID")]
        [SerializeField]
        private int narrationBundleID;

        [Space(10)]
        [Header("퍼즐 NPC - 밸런싱")]
        // 대화 인덱스 
        [SerializeField]
        private int dialogIndex = 0;

        [SerializeField]
        private List<string> narrationList;    // 나레이션 리스트

        // 대화가 가능한 상태인지?
        private bool isTalkable = false;

        protected override void Start()
        {
            LoadNarrationData();
        }

        private void LoadNarrationData()
        {
            if(!Manager.Data.NarrationBundleDic.ContainsKey(narrationBundleID))
            {
                Debug.Log($"Narration Bundle ID : {narrationBundleID} 에 해당하는 나레이션 번들이 존재하지 않습니다.");
                return;
            }

            foreach(int id in Manager.Data.NarrationBundleDic[narrationBundleID])
            {
                if(!Manager.Data.NarrationDataDic.ContainsKey(id))
                {
                    Debug.Log($"Narration ID : {id} 에 해당하는 나레이션이 존재하지 않습니다.");
                    break;
                }

                // 나레이션 텍스트 할당
                narrationList.Add(Manager.Data.NarrationDataDic[id].text);
            }
        }

        public override bool OnInteract(PlayerQuestController questController)
        {
            if (!isTalkable)
                return false;

            if(narrationList == null || narrationList.Count < 1)
            {
                Debug.Log($"NPC ID : {id}에 할당된 나레이션이 존재하지 않습니다.");
                return false;
            }

            UpdateDialog(questController);
            return true;
        }

        protected override void UpdateDialog(PlayerQuestController questController)
        {
            // 대화 종료시
            if(dialogIndex >= narrationList.Count)
            {
                dialogText.enabled = false;
                UpdatePuzzleManager(puzzle);
                return;
            }

            dialogText.text = narrationList[dialogIndex++];
            // 플로팅 애니메이션
            floatingAnim.SetTrigger(Manager.Param.OnFloating);
        }

        public override void OnExitInteract()
        {
            dialogText.enabled = false;

            dialogIndex = 0;
        }

        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index = -1)
        {
            isTalkable = false;
            puzzle.OnClearPuzzle();
        }

        public void ActiveSetting()
        {
            isTalkable = true;
        }

        public void DisActiveSetting()
        {
            isTalkable = false;
        }

        public void CompleteSetting()
        {
            isTalkable = false;
            puzzle.OnClearPuzzle();
        }
    }
}

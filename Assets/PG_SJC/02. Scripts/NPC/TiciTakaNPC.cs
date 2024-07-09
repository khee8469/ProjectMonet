using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class TiciTakaNPC : NPC
    {
        [Header("티키타카 NPC 세팅")]
        [Tooltip("대화를 출력할 텍스트창")]
        [SerializeField]
        private TextMeshProUGUI secondDialogText;

        [Tooltip("대화 출력 인덱스")]
        [SerializeField]
        private int secondDialogIndex;

        [Tooltip("대화 출력 애니메이션")]
        [SerializeField]
        private Animator secondFloatingAnim;

        [Tooltip("빌보드 UI")]
        [SerializeField]
        private BuilboardUI secondBuilboard;

        protected override void UpdateDialog(PlayerQuestController questController)
        {
            builboardUI.EnableBuilboard = true;
            dialogText.enabled = true;
            secondDialogText.enabled = false;

            // 현재 할당된 퀘스트가 없는 경우
            if (currentQuest == null)
            {
                // 플로팅 애니메이션
                floatingAnim.SetTrigger(Manager.Param.OnFloating);

                if (curBasicDialogIndex >= basicNarrations.Count)
                {
                    dialogText.enabled = false;
                    builboardUI.EnableBuilboard = false;
                }
                else
                    dialogText.text = Manager.Data.NarrationDataDic[basicNarrations[curBasicDialogIndex++]].text;
                return;
            }

            // 할당된 퀘스트가 있는 경우
            // 퀘스트 상태에 따른 대화 출력
            switch (currentQuest.State)
            {
                // 퀘스트 수주
                case QuestState.Active:
                    // 대화 종료 체크
                    if (curQuestDialogIndex >= currentQuest.receiveNarrations.Count - 1)
                    {
                        builboardUI.EnableBuilboard = false;
                        dialogText.enabled = false;
                        // 최초 등록 (수주 시에만 최초로 등록)
                        // 플레이어에 퀘스트 등록
                        questController.ReceiveQuest(currentQuest);
                        // 퀘스트 진행중 상태로 변경
                        currentQuest.ChangeState(QuestState.Proceed);
                        return;
                    }
                    // 티키타카
                    if (curQuestDialogIndex == secondDialogIndex)
                    {
                        dialogText.enabled = false;
                        builboardUI.EnableBuilboard = false;
                        secondDialogText.enabled = true;
                        secondBuilboard.EnableBuilboard = true;

                        // 플로팅 애니메이션
                        secondFloatingAnim.SetTrigger(Manager.Param.OnFloating);
                        secondDialogText.text = currentQuest.receiveNarrations[curQuestDialogIndex++].text;
                    }
                    // 일반 NPC
                    else
                    {
                        dialogText.enabled = true;
                        builboardUI.EnableBuilboard = true;
                        secondDialogText.enabled = false;
                        secondBuilboard.EnableBuilboard = false;

                        // 플로팅 애니메이션
                        floatingAnim.SetTrigger(Manager.Param.OnFloating);
                        dialogText.text = currentQuest.receiveNarrations[curQuestDialogIndex++].text;
                    }
                    break;
                // 퀘스트 진행중
                case QuestState.Proceed:
                    // 플로팅 애니메이션
                    floatingAnim.SetTrigger(Manager.Param.OnFloating);
                    dialogText.text = currentQuest.receiveNarrations[currentQuest.receiveNarrations.Count - 1].text;
                    break;
                // 퀘스트 완료
                case QuestState.Clear:
                    // 대화 종료 체크
                    if (curQuestDialogIndex >= currentQuest.clearNarrations.Count - 1)
                    {
                        builboardUI.EnableBuilboard = false;
                        dialogText.enabled = false;
                        // 퀘스트 완료 상태로 변경
                        currentQuest.ChangeState(QuestState.Complete);
                        // 리워드 지급은 퀘스트 자체에서 진행
                        // NPC 상태 변경
                        return;
                    }
                    // 대화 진행
                    // 플로팅 애니메이션
                    floatingAnim.SetTrigger(Manager.Param.OnFloating);
                    dialogText.text = currentQuest.clearNarrations[curQuestDialogIndex++].text;
                    break;
                default:
                    break;
            }
        }

    }
}
using System;
using UnityEngine;

namespace JJH
{
    public class UnLockController : MonoBehaviour
    {
        public StageDoor[] doors;  //상호작용 할 스테이지 입구 -> 여기에 완성된 그림 오브젝트를 넣어야 할듯?
        // 또는 1챕터는 기본으로 접근 가능해야 하다면 1챕터만 그냥 기본 상태랑 완성상태랑 차라리 두개 넣어두고
        // on off 되도 어차피 둘 다 1챕터로 연결해놓기도 괜찮음. 


        private void Start()
        {
            ChapterManager.Instance.stageEvent.AddListener(StageUnLock); //lobby 에서 씬의 해금 상태를 start 에서 체크
            LoadStageData(); // 씬이 시작될 때 스테이지 데이터 로드 
        }

        private void LoadStageData() // 저장된 스테이지 데이터 로드 
        {
            doors = FindObjectsOfType<StageDoor>();

            Array.Sort(doors); //DoorId를 기준으로 정렬시도.

            for (int i = 0; i < doors.Length; i++)
            {
                bool isUnlocked = i < ChapterManager.Instance.runtimeStageData.stageUnlockStatus.Count
                    ? ChapterManager.Instance.runtimeStageData.stageUnlockStatus[i] : false;


                if (i >= ChapterManager.Instance.runtimeStageData.stageUnlockStatus.Count)
                {
                    ChapterManager.Instance.runtimeStageData.stageUnlockStatus.Add(isUnlocked);

                }

                if(i==0)
                {
                    isUnlocked = true;
                    ChapterManager.Instance.runtimeStageData.stageUnlockStatus[i] = true;
                }

                // -1 --> everything .. 0 --> notthing 
                doors[i].interactionLayers = isUnlocked ? -1 : 0;

                //doors[i].GetComponent<Renderer>().material.color = isUnlocked ? Color.yellow : Color.gray;
                // 임시로 마테리얼 색상 변경으로 unlock 확인. 

            }
        }

        public void SaveStageData()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                ChapterManager.Instance.runtimeStageData.stageUnlockStatus[i] = doors[i].interactionLayers == -1;
            }
        }

        public void UnlockStage(int stageIndex)
        {
            if (stageIndex >= 0 && stageIndex < ChapterManager.Instance.runtimeStageData.stageUnlockStatus.Count)
            {
                ChapterManager.Instance.runtimeStageData.stageUnlockStatus[stageIndex] = true;
            }
        }


        private void StageUnLock(int number, bool unLock) //  < 이벤트 > 에 등록된 메서드 --> 외부에서 이벤트 발동시켜주기. 
        {
            number = number - 1; // 외부에서 2챕터 부르면 2챕터 unlock 시키기 위해. ( 내부는 0 부터 시작하므로)

            print("챕터 개방 이벤트 발동됨");

            if (number >= 0 && number < doors.Length)
            {
                print("스테이지 언락 if문 내부 진입");
                ChapterManager.Instance.runtimeStageData.stageUnlockStatus[number] = unLock; // bool 값을 변경 시켜줌. 
                doors[number].interactionLayers = unLock ? -1 : 0; // 상호작용 가능하게 변경시켜줌 --> true면 everything
                //doors[number].GetComponent<Renderer>().material.color = unLock ? Color.yellow : Color.gray;
                // 임시로 언락 상태 확인하기 위함. 
            }
        }


    }

}


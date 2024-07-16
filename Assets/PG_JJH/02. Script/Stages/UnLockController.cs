using System;
using UnityEngine;

namespace JJH
{
    public class UnLockController : MonoBehaviour // 이거 어차피 로비에만 있어.
    {
        public StageDoor[] doors;  //상호작용 할 스테이지 입구 -> 여기에 완성된 그림 오브젝트를 넣어야 할듯?
        // 또는 1챕터는 기본으로 접근 가능해야 하다면 1챕터만 그냥 기본 상태랑 완성상태랑 차라리 두개 넣어두고
        // on off 되도 어차피 둘 다 1챕터로 연결해놓기도 괜찮음. 


        private void Start()
        {
            ChapterManager.Instance.stageEvent.AddListener(StageUnLock); //lobby 에서 씬의 해금 상태를 start 에서 체크 이벤트 등록???? 

            // 이 순간에 json 이용 해서 

            // Lobby 씬에서 Load 할 때 --> 이 부분을 Json을 이용해서 Josn 한 번 Load 해주고
            // 그 값을 통해서 체크 한 다음에 씬 해금 상태를 확인 할 것. 
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

                if(i==0) // 아 이게 정렬 할 때 0 번 인덱스만 열리도록 해놔서 door ID가 겹치는 부분이 있으면 이거 안열림. 
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

        // interaction layer 를 바꿔줘서 더이상 select 하지 못하도록 만든다. 
        public void SaveStageData()  // 야 이거 Save 인데 왜 참조가 0 개인지?? 
        {
            for (int i = 0; i < doors.Length; i++)
            {
                ChapterManager.Instance.runtimeStageData.stageUnlockStatus[i] = doors[i].interactionLayers == -1;
            }
        }

        // bool 변수를 true로 만들어서 해방시킨다. 
        public void UnlockStage(int stageIndex)
        {
            if (stageIndex >= 0 && stageIndex < ChapterManager.Instance.runtimeStageData.stageUnlockStatus.Count)
            {
                ChapterManager.Instance.runtimeStageData.stageUnlockStatus[stageIndex] = true;
            }
        }

        private void StageUnLock(int number, bool unLock) //  < 이벤트 > 에 등록된 메서드 --> 외부에서 이벤트 발동시켜주기. 
        {           
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


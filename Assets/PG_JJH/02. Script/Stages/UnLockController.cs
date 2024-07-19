using System;
using UnityEngine;
using JJH;
using Jc;

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


            //LoadStageData(); // 씬이 시작될 때 스테이지 데이터 로드 --> 이거를 json 으로 해주고.

            Manager.PlayableData.LoadCanvasData(); // 캔버스 데이터 로드 -> 여기 list가 같이 있음. 이거 그냥 배열로 바꾸는게 나을 것 같은데. 
            LoadStageData();

        }

        private void LoadStageData() // 저장된 스테이지 데이터 로드 
        {
            Array.Sort(doors); //DoorId를 기준으로 정렬시도.

            for (int i = 0; i < doors.Length; i++)    // 이게 List 라서 이렇게 하고 있는데 이거 배열로 바꿔서 한다면? 
            {
                bool isUnlocked = Manager.PlayableData.CanvasData.stageUnlockStatus[i]; // json에 저장된 bool 변수와 매칭해서 bool 값을 정해준다.

                if(i==0) // 아 이게 정렬 할 때 0 번 인덱스만 열리도록 해놔서 door ID가 겹치는 부분이 있으면 이거 안열림. 
                {
                    isUnlocked = true;
                    //ChapterManager.Instance.runtimeStageData.stageUnlockStatus[i] = true;
                    Manager.PlayableData.CanvasData.stageUnlockStatus[i] = isUnlocked; // 일단 0 번은 무조건 켜주기는 하는데... --> 첫 스테이지 이므로                
                }
                // -1 --> everything .. 0 --> notthing 
                doors[i].interactionLayers = isUnlocked ? -1 : 0; // true면 만질 수 있도록 아니면 unlock 상태 . 
            }
        }

        /*// interaction layer 를 바꿔줘서 더이상 select 하지 못하도록 만든다. 
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
        }*/

        private void StageUnLock(int number, bool unLock) //  < 이벤트 > 에 등록된 메서드 --> 외부에서 이벤트 발동시켜주기. 결국 여기가 핵심이네. 
        {           

            if (number >= 0 && number < doors.Length)
            {
                
                //ChapterManager.Instance.runtimeStageData.stageUnlockStatus[number] = unLock; // bool 값을 변경 시켜줌. 
                Manager.PlayableData.CanvasData.stageUnlockStatus[number] = unLock; // 사실 이거 어차피 무조건 트루긴 함.. 
                doors[number].interactionLayers = unLock ? -1 : 0;

                // doors[number].CoverOff(); 

                Manager.PlayableData.SaveCanvasData(); // 값 변경 시키고 여기서 저장. 

                // 여기서 Josn Save 한 번 하죠.
                


            }
        }


    }

}


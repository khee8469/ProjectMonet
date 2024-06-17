using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using JJH;

namespace JJH
{
    public class ChapterManager : Singleton<ChapterManager>
    {
        [SerializeField] private static int numberOfScenes = 4;


        public StageData stageData; //스크립터블 오브젝트 
        public StageData runtimeStageData; //에디터 런타임용 스크립터블 오브젝트 

        public UnityEvent<int, bool> stageEvent = new UnityEvent<int, bool>();

        [Header("그림 관련 변수들")]
        [Tooltip("그림 완료 기능 bool 변수 -> 로딩루틴에서 이용")]
        public static bool[] is_Colored = new bool[numberOfScenes]; // 4개의 씬 false로 체크 
            

        // 어차피 내부적으로 돈 디스트로이 awake 다 진행함.

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            // 런타임에 스크립터블 오브젝트를 복제하여 원본 자산에 영향을 주지 않도록 함
            runtimeStageData = Instantiate(stageData);
#else
        runtimeStageData = stageData;
#endif

        }
        public void UnlockStage(int stageIndex, bool unlock) // 챕터 언락용 함수. 
        {

            if (stageIndex >= 0 && stageIndex < runtimeStageData.stageUnlockStatus.Count)
            {
                Debug.Log("언락 스테이 발동" + stageIndex);
                runtimeStageData.stageUnlockStatus[stageIndex] = true;
                stageEvent.Invoke(stageIndex, true);
            }
        }






    }
}



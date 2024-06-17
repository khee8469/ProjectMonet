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
        [Tooltip("실제 씬 갯수 만큼 삽입")]
        [SerializeField] private static int sceneCount = 4;

        [SerializeField] private static int drawingNumbers; // 어찌 쓸지 고민해보자. 


        public StageData stageData; //스크립터블 오브젝트 
        public StageData runtimeStageData; //에디터 런타임용 스크립터블 오브젝트 

        public UnityEvent<int, bool> stageEvent = new UnityEvent<int, bool>();
        

        [Header("그림 관련 변수들")]
        [Tooltip("static bool 인스펙터 체크 용도 변수")]
        [SerializeField]
        private bool[] isColoredInspector; // 인스펙터에서 값을 설정하는 변수


        [Tooltip("그림 포스트프로세싱 관련 bool 변수 -> 로딩루틴에서 이용")]
        public static bool[] is_Colored ; //씬의 갯수만큼 첫 start 에서 가져온다. (실제 빌드에서) 

        [Tooltip("그림 조각이 다 그려졌는지 확인할 bool static 변수")]
        public static bool[] isDrawing_Complete;

        [Tooltip("딕셔너리 체크용 인스펙터")]
        [SerializeField] List<DrawObjectManager> checkInspector = new List<DrawObjectManager> ();

        [Tooltip("컬러 스크립터블 오브젝트")]
        [SerializeField] private PaintTypeManager paintTypeManager;

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

        private void Start()
        {
            // 같은 이름의 오브젝트여도 서로 다른 오브젝트라면 다른 key로 판단 가능. 
            DrawObjectManager[] drawObjectManager = GameObject.FindObjectsOfType<DrawObjectManager>();

            is_Colored = new bool[sceneCount];

            for(int i=0; i < sceneCount; i++)
            {
                is_Colored[i] = false; // start 에서 한 번 false 로 
            }

            isColoredInspector = is_Colored;

            isDrawing_Complete = new bool [drawObjectManager.Length]; // 그림들의 배열의 길이만큼 bool 변수의 크기를 정해준다.

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

        // 챕터의 흑백효과 해제 시켜줄 함수--> 그림 완전히 완성시에 호출 시켜줄 것. 
        public void CheckDrawComplete(int coloredScene , bool isColored) // 이거 그림 완성되면 호출해서 static bool 바꾸기
        {
            is_Colored[coloredScene] = isColored; // 해당하는 씬을 숫자를 통해 컬러로 바꿔주기. 
            // 각 씬의 로딩 루틴에서는 인덱스를 통해 접근함.

        }

    }
}



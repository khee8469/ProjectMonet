using Jc;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace JJH
{
    public class ChapterManager : Singleton<ChapterManager>
    {
        [Tooltip("실제 씬 갯수 만큼 삽입")]
        [SerializeField] private static int sceneCount = 4;

        private const int DrawingCount = 12;

        public UnityEvent<int, bool> stageEvent = new UnityEvent<int, bool>();

        [Header("그림 관련 변수들")]
        [Tooltip("static bool 인스펙터 체크 용도 변수")]
        [SerializeField]
        private bool[] isColoredInspector; // 인스펙터에서 값을 설정하는 변수

        [Tooltip("그림 포스트프로세싱 관련 bool 변수 -> 로딩루틴에서 이용")]
        public static bool[] is_Colored; //씬의 갯수만큼 첫 start 에서 가져온다. (실제 빌드에서)  --> 이거는 챕터 해금이랑 따로 관리되는 것 같은데 이 부분은 내가 해주는게 맞을듯? 챕터 해금은 진짜 다음 챕터 열리는 거니까. 

        [Tooltip("그림 조각이 다 그려졌는지 확인할 bool static 변수")]
        public static bool[] isDrawing_Complete { get; set; }

        [Tooltip("진짜 그림 하나 하나 씩 체크 해서 알파값을 올려 줄 BOOL 변수")]
        public static bool[] drawPartCheck { get; set; }

        [Tooltip("딕셔너리 체크용 인스펙터")]
        [SerializeField] List<DrawObjectManager> checkInspector = new List<DrawObjectManager>();

        [Tooltip("컬러 스크립터블 오브젝트")]
        [SerializeField] private PaintTypeManager paintTypeManager;

        [SerializeField] private DrawObjectManager[] drawObjectManager { get; set; }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            isColoredInspector = is_Colored; // 인스펙터 창에서 보기 위해 변수 할당 

            is_Colored = new bool[sceneCount];
            drawPartCheck = new bool[DrawingCount];
            Manager.PlayableData.LoadCanvasData();  // start 에서 Load 받아서 이닛 세팅 실행. 

            
            InitSetting();
        }

        public void InitSetting()
        {
            if (!File.Exists(SystemPath.GetPath(DataPath.LocalCanvasData)))
            {                           
                return;
            }

            for (int i = 0; i < Manager.PlayableData.CanvasData.isColoredCheckArr.Length; i++)
            {
                is_Colored[i] = Manager.PlayableData.CanvasData.isColoredCheckArr[i];
            }

            for (int i = 0; i < DrawingCount; i++)
            {
                drawPartCheck[i] = Manager.PlayableData.CanvasData.myDrawPartCheckArr[i]; 
            }

        }

        public void UnlockStage(int stageIndex, bool unlock) // 챕터 언락용 함수. 
        {

            if (stageIndex >= 0 && stageIndex < Manager.PlayableData.CanvasData.stageUnlockStatus.Length)
            {
                Manager.PlayableData.CanvasData.stageUnlockStatus[stageIndex] = true;  // json 저장 
                stageEvent.Invoke(stageIndex, true);
                Manager.PlayableData.SaveCanvasData(); // save json 

            }
        }

        // 챕터의 흑백효과 해제 시켜줄 함수--> 그림 완전히 완성시에 호출 시켜줄 것. 
        public void CheckDrawComplete(int coloredScene, bool isColored) // 이거 그림 완성되면 호출해서 static bool 바꾸기
        {
            is_Colored[coloredScene] = isColored; // 해당하는 씬을 숫자를 통해 컬러로 바꿔주기. 

            Manager.PlayableData.CanvasData.isColoredCheckArr[coloredScene] = isColored;
        }

    }
}



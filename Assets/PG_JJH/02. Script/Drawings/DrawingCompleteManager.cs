using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JJH;
using Jc;

namespace JJH
{
    public class DrawingCompleteManager : MonoBehaviour // 얘는 로비 포함 스테이지 마다 있어야 한다. 
    {
        // 그냥 모든 씬에 두고 할당해주자. --> 이거 나중에 i 말고 id 값으로 해서 모든 씬에 다 그림 놓을 필요 없게 해야함. 


        [Tooltip("그림의 bool 변수 체크해줄 딕셔너리")]
        [SerializeField]
        public Dictionary<DrawObjectManager, bool> drawCompleteCheckDic
           = new Dictionary<DrawObjectManager, bool>();

        [Tooltip("그림의 컬러를 비교해 줄 스크립터블 오브젝트")]
        [SerializeField] private PaintTypeManager paintTypeManager;

        [Tooltip("딕셔너리의 무결성 유지를 위한 임시 key 저장용 리스트")]
        List<DrawObjectManager> keysToModifty = new List<DrawObjectManager>();

        [Header("씬에 존재하는 그림 위 색깔조명들")]
        [Tooltip("각 씬에 둘 그림 연계 라이트들")]
        [SerializeField]
        public LightColor [] lights; // 각 씬 마다 직접 할당해서 배열을 주면 어차피 start 에서 배열의 크기가 달라짐. 

        [Tooltip("드로우 오브젝트 매니저 할당해주기.")]
        [SerializeField]
        private DrawObjectManager[] drawObjectManagers;

        private void Start()
        {
            // DrawObjectManager 에 붙어있는 IComparable 을 이용하여 정렬한다.
            Array.Sort(drawObjectManagers); //어차피 둘이 같은 타입을 찾는 find를 하기 때문에 길이는 무조건 같

            // 여기 직접 Add 해주지 말고. 저장 한 다음에 가져다 쓰는 방식으로 Int로 ID값 받고 OBJECT들은 각자 자신의 Draw id값이 있으니까
            // 그 값을 이용해서 ChapterManager.DrawPartCheck[drawId] 이런식으로 쓰면 Start에서 단 한번만 돌리고도
            // Dictionary를 계속 재사용 가능할 것이다.
            // json을 잘 이용해서? 아니면 start 순서 이용해서 한 번만 돌 수 있게 해줘야함. 계속 Add 말고.
            // 이게 계속 Add 하게 되면 Drawing part를 각 씬에 전부 배치해야 하는데 너무 극혐임.

            for (int i = 0; i < drawObjectManagers.Length; i++) //어차피 이 둘은 길이가 똑같음. 여기서 id 로만 int key 가능하면 어떻게든 될텐데... 
            {
                drawCompleteCheckDic.Add(drawObjectManagers[i], ChapterManager.drawPartCheck[i]);
                // 어차피 씬 넘어갈 때마다 이 딕셔너리는 초기화 되기 때문에 그냥 Add 해주면된다. 
                // json을 이용한다면 ... 챕터 매니저에 저장된 drawPartCheck 를 이용해서 

                if (drawCompleteCheckDic.TryGetValue(drawObjectManagers[i], out bool isTrue))
                {
                    DrawObjectManager dr = drawObjectManagers[i].GetComponent<DrawObjectManager>();

                    if (isTrue && dr.currentPaintType != PaintTypeEnum.None) // 예상대로라면 여기서 챕터에 있는 안쓰는 애들은 None이니까 if문 내부 안들어가도 된다. 
                    {
                        if (dr != null)
                        {
                            SpriteRenderer sprite = dr.GetComponent<SpriteRenderer>();
                            Color color = sprite.color;
                            color.a = 1f;
                            sprite.color = color;

                            dr.gameObject.layer = 17; // 혹시 모르니까 여기서 로컬블룸으로 변경하는 거인듯? 
                            ChangeLight(dr.currentPaintType);  // 자신의 드로우 오브젝트에 할당되어 있는 색깔 받아서 켜주기. 
                        }
                    }
                }
            }

        }

        // 그림 조각이 하나 그려질 때 마다 이 DrawComplete가 불러진다.
        // -> 여기서 이제 AllComplete를 체크해서 한 스테이지의 그림이 모두 완성되었는지를 체크한다.
        public void DrawComplete(int drawingNumber, bool finishied, int instanceID) // 그림이 완성되었을 때 (완전히) 진행할 함수 
                                                                                    // 챕터도 해금 시켜줘야 하고. 포스트프로세싱도 종료 시켜줘야하기 때문에                                                                                    
        {          
            ChapterManager.drawPartCheck[instanceID] = finishied;
            Manager.PlayableData.CanvasData.myDrawPartCheckArr[instanceID] = finishied; // true로 변경 
            Manager.PlayableData.SaveCanvasData();

            // 임시 키 저장용 리스트 초기화
            keysToModifty.Clear(); // 초기화 안하면 이거 계속 들어있음. 

            foreach (var obj in drawCompleteCheckDic)
            {
                if (obj.Key.drawBoardNumber == (DrawBoardNumber)drawingNumber) // 결국은 매개변수를 다른곳에서 받아야함
                {
                    if (obj.Key.DrawID == instanceID)
                    {
                        keysToModifty.Add(obj.Key);
                    }
                }
            }
            // foreach 문 종료 후 딕셔너리 수정 
            foreach (var key in keysToModifty)
            {
                drawCompleteCheckDic[key] = finishied;  // 이 부분은 지금 PART1의 모든 그림이 완성되었는지를 체크하고 있는 로직이다. 
            }

            // 모든 오브젝트가 완성되었는지 확인하기. --> 타입을 매개변수로 받기 때문에 그 순간의 매개변수로 체크
            bool allComplete = true;

            foreach (var kvp in drawCompleteCheckDic)
            {
                if ((kvp.Key.drawBoardNumber == (DrawBoardNumber)drawingNumber) && kvp.Value == false) // 같은 PART 에 있는 그림이 전부 그려졌는지를 체크하는 반복문 
                {                   
                    allComplete = false;
                    break;
                }
            }

            if (allComplete) // 이게 지금 모두 true 라면 
            {
                // 0 1 2 3 --> 4개의 씬 
                ChapterManager.Instance.CheckDrawComplete(drawingNumber, true); // 씬의 필터 해제 --> isColored를 변경해주는 함수 
                if(drawingNumber<=2)
                {
                    ChapterManager.Instance.UnlockStage(drawingNumber + 1, true); // 3 부터는 인덱스 터짐. -> 여기서 stage 개방 json 저장 실행한다.  
                }              
                // 완성본 그림 알파값 업그레이드 해주기

                FinishedDraw.FinishAlphaUp.Invoke(drawingNumber); // 각 씬 마다 UnityEvent 를 부르는데
                // 챕터에는 어차피 finish 붙은게 하나만 있을 거니까 괜찮을듯. 
            }
        }

        public void ChangeLight(PaintTypeEnum _currentPaintType)
        {
            if(lights.Length>=1) //1개 이상 할당이 되어 있으면. 
            {
                for(int i=0;i<lights.Length; i++)
                {
                    lights[i].drawLightLink(_currentPaintType);
                }
            }
        }

    }
}

